using LeadsHub.Core.Interfaces.IBac;
using LeadsHub.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace LeadsHub.Api.Controllers
{
    public class SalesPipelineController : BaseController
    {
        private readonly ISalesPipelineBac _salesPipelineBac;
        public SalesPipelineController(ISalesPipelineBac salesPipelineBac)
        {
            _salesPipelineBac = salesPipelineBac;
        }

        [HttpGet]
        public async Task<IActionResult> FetchSalesPipelinesAsync()
        {
            int id = 1; // Simulação de ID do pipeline

            /*var pipelines = new List<SalesPipeline>
            {
                new SalesPipeline
                {
                    Id = 1,
                    Name = "Real Estate",
                    Stages = new List<PipelineStage>
                    {
                        new PipelineStage
                        {
                            Id = 1,
                            Title = "New",
                            StageOrder = 1,
                            PipelineId = 1,
                            Leads = new List<LeadStage>
                            {
                                new LeadStage { LeadId = 1, LeadName = "John Doe", PhoneNumber = "1234", Position = 1, StageId = 1 },
                                new LeadStage { LeadId = 2, LeadName = "Jane Smith", PhoneNumber = "5678", Position = 2, StageId = 1 }
                            }
                        },
                        new PipelineStage
                        {
                            Id = 2,
                            Title = "Contacted",
                            StageOrder = 2,
                            PipelineId = 1,
                            Leads = new List<LeadStage>
                            {
                                new LeadStage { LeadId = 3, LeadName = "Maria Green", PhoneNumber = "9999", Position = 1, StageId = 2 }
                            }
                        }
                    }
                }
            };

            var pipeline = pipelines.FirstOrDefault(p => p.Id == id);

            if (pipeline == null)
                return NotFound();*/

            var response = await _salesPipelineBac.FetchPipelineByIdAsync(id);
            if (response.HasAnyErrorMessage)
                return BadRequest(response);


            response.Model.Stages = response.Model.Stages.OrderBy(s => s.StageOrder).ToList();
            foreach (var stage in response.Model.Stages)
            {
                stage.Leads = stage.Leads.OrderBy(l => l.Position).ToList();
            }

            return Ok(response);
        }
    }
}
