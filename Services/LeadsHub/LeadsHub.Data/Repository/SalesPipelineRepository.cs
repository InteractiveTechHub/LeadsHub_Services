
using AdaptiveKitCore.Responses;
using Dapper;
using LeadsHub.Core.Interfaces.IRepository;
using LeadsHub.Core.Models;
using LeadsHub.Core.Utility;
using Npgsql;
using System.ComponentModel.DataAnnotations;

namespace LeadsHub.Data.Repository
{
    public sealed class SalesPipelineRepository : ISalesPipelineRepository
    {
        public async Task<SimpleResponse<SalesPipeline>> FetchPipelineByIdAsync(long pipelineId)
        {
            SimpleResponse<SalesPipeline> response = new(); 

            string query = "SELECT " +
                "sp.\"Id\", " +
                "sp.\"Name\", " +
                "sp.\"CompanyId\", " +
                "sp.\"ConsultantId\", " +
                "stg.\"Id\", " +
                "stg.\"Title\", " +
                "stg.\"StageOrder\"," +
                "ltg.\"Id\", " +
                "ltg.\"LeadId\", " +
                "ltg.\"PipelineStageId\", " +
                "ltg.\"Position\"," +
                "c.\"Id\", " +
                "c.\"Name\" AS LeadName, " +
                "c.\"PhoneNumber\", " +
                "c.\"Email\", " +
                "ld.\"Id\" As LeadId," +
                "ld.\"Identifier\", " +
                "ld.\"CompanyId\", " +
                "ld.\"CreatedAt\", " +
                "cs.\"Id\" AS ConsultantId," +
                "cs.\"IdentityId\" AS UserIdentityId, " +
                "cs.\"FullName\" AS ConsultantName, " +
                "lm.\"LastMessage\", " +
                "lm.\"LastMessageDate\"," +
                "lm.\"Status\" " +
                "FROM \"SalesPipeline\" sp " +
                "INNER JOIN \"PipelineStage\" stg ON sp.\"Id\" = stg.\"SalesPipelineId\" " +
                "LEFT JOIN \"LeadStage\" ltg ON stg.\"Id\" = ltg.\"PipelineStageId\" " +
                "LEFT JOIN \"Lead\" ld ON  ltg.\"LeadId\" = ld.\"Id\" " +
                "LEFT JOIN \"Contact\" c ON ld.\"ContactId\" = c.\"Id\" " +
                "LEFT JOIN \"Consultant\" cs ON ld.\"ConsultantId\" = cs.\"Id\" " +
                "LEFT JOIN \"LastMessage\" lm ON lm.\"LeadId\" = ld.\"Id\" " +
                "WHERE sp.\"Id\" = @PipelineId";

            try
            {
                using var connection = new NpgsqlConnection(SD.ConnectString);
                await connection.OpenAsync();

                var stageDict = new Dictionary<long, PipelineStage>();
                SalesPipeline? pipeline = null;

                var result = await connection.QueryAsync<SalesPipeline, PipelineStage, LeadStage, LeadCard, SalesPipeline>(
                    query,
                    (sp, stage, leadStage, leadCard) =>
                    {
                        if (pipeline == null)
                        {
                            pipeline = sp;
                        }

                        if (!stageDict.TryGetValue(stage.Id, out var currentStage))
                        {
                            currentStage = stage;
                            currentStage.Leads = new List<LeadStage>();
                            pipeline.Stages.Add(currentStage);
                            stageDict[stage.Id] = currentStage;
                        }

                        if (leadStage != null && leadStage.Id != 0)
                        {
                            leadStage.LeadCard = leadCard;
                            currentStage.Leads.Add(leadStage);
                        }

                        return pipeline;
                    },
                    param: new { PipelineId = pipelineId },
                    splitOn: "Id"
                );

                response.Model = pipeline ?? new();
            }
            catch (Exception ex)
            {
                // Handle exception
                response.AddExceptionMessage(ex.Message);
                Console.WriteLine($"Error fetching pipeline: {ex.Message}");
            }

            return response;
        }
    }
}
