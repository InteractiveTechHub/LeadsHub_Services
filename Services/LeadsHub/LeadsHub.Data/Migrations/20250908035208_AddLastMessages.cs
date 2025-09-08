using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LeadsHub.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLastMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "LastMessage",
                columns: new[] { "LeadId", "LastMessageDate", "LastMessage", "Status", "TimelineId" },
                values: new object[,]
                {
                    { 1L, new DateTimeOffset(new DateTime(2024, 1, 1, 10, 10, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Perfeito! Qual seria o melhor horário para a demonstração? Preciso de algo que funcione bem para minha equipe de 5 pessoas.", (short)3, 3L },
                    { 2L, new DateTimeOffset(new DateTime(2024, 1, 1, 11, 10, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Sim, por favor! E qual seria o prazo para desenvolvimento? Preciso lançar em 2 meses.", (short)3, 6L },
                    { 3L, new DateTimeOffset(new DateTime(2024, 1, 1, 14, 15, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Excelente! Confirmado para quinta às 14h. Vou preparar uma apresentação com nosso plano de transformação digital. Até lá!", (short)3, 10L },
                    { 4L, new DateTimeOffset(new DateTime(2024, 1, 1, 14, 15, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Excelente! Confirmado para quinta às 14h. Vou preparar uma apresentação com nosso plano de transformação digital. Até lá!", (short)3, 10L },
                    { 5L, new DateTimeOffset(new DateTime(2024, 1, 1, 15, 15, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Sim! Já desenvolvemos 3 apps de delivery. Posso enviar os cases e agendar uma call para detalhar o projeto?", (short)3, 14L },
                    { 6L, new DateTimeOffset(new DateTime(2024, 1, 1, 16, 5, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Sem problemas, Juliana! Se precisar de algo no futuro, estaremos aqui. Boa sorte com o projeto!", (short)3, 16L }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LastMessage",
                keyColumn: "LeadId",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "LastMessage",
                keyColumn: "LeadId",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "LastMessage",
                keyColumn: "LeadId",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "LastMessage",
                keyColumn: "LeadId",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "LastMessage",
                keyColumn: "LeadId",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "LastMessage",
                keyColumn: "LeadId",
                keyValue: 6L);
        }
    }
}
