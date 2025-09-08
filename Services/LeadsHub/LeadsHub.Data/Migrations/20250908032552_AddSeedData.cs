using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LeadsHub.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Company",
                columns: new[] { "Id", "BrandName", "ConsultantId", "CreatedAt", "Email", "Enabled", "IdentificationNumber", "LegalName", "PhoneNumber", "UpdatedAt" },
                values: new object[,]
                {
                    { 1L, "TechCorp Solutions", null, new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)), "contato@techcorp.com.br", true, "12.345.678/0001-90", "TechCorp Solutions Ltda", "(11) 99999-9999", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 2L, "Inovação Digital", null, new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)), "vendas@inovacaodigital.com.br", true, "98.765.432/0001-10", "Inovação Digital S.A.", "(21) 88888-8888", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 3L, "StartupHub", null, new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)), "info@startuphub.com.br", true, "11.222.333/0001-44", "StartupHub Tecnologia Ltda", "(31) 77777-7777", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                table: "Contact",
                columns: new[] { "Id", "BirthDate", "CPF", "CreatedAt", "Email", "Name", "PhoneNumber", "UpdatedAt" },
                values: new object[,]
                {
                    { 1L, new DateTime(1985, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), "123.456.789-00", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)), "carlos.mendes@email.com", "Carlos Eduardo Mendes", "(11) 98765-4321", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 2L, new DateTime(1990, 7, 22, 0, 0, 0, 0, DateTimeKind.Utc), "234.567.890-11", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)), "ana.ferreira@email.com", "Ana Paula Ferreira", "(21) 97654-3210", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 3L, new DateTime(1988, 11, 8, 0, 0, 0, 0, DateTimeKind.Utc), "345.678.901-22", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)), "roberto.lima@email.com", "Roberto Silva Lima", "(31) 96543-2109", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 4L, new DateTime(1992, 5, 30, 0, 0, 0, 0, DateTimeKind.Utc), "456.789.012-33", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)), "fernanda.santos@email.com", "Fernanda Costa Santos", "(11) 95432-1098", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 5L, new DateTime(1987, 9, 12, 0, 0, 0, 0, DateTimeKind.Utc), "567.890.123-44", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)), "pedro.oliveira@email.com", "Pedro Henrique Oliveira", "(21) 94321-0987", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 6L, new DateTime(1993, 1, 25, 0, 0, 0, 0, DateTimeKind.Utc), "678.901.234-55", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)), "juliana.alves@email.com", "Juliana Rodrigues Alves", "(31) 93210-9876", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "Id", "Category", "CreatedAt", "Description", "Name", "Price", "ProductCode", "UpdatedAt" },
                values: new object[,]
                {
                    { 1L, "Software", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)), "Sistema completo de gestão de relacionamento com clientes", "Sistema CRM Completo", 299.90m, "CRM-001", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 2L, "Software", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)), "Solução completa para loja virtual", "Plataforma de E-commerce", 499.90m, "ECO-001", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 3L, "Consultoria", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)), "Consultoria especializada em transformação digital para empresas", "Consultoria em Transformação Digital", 150.00m, "CON-001", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 4L, "Desenvolvimento", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)), "Desenvolvimento de aplicativo mobile personalizado", "Desenvolvimento de App Mobile", 2500.00m, "APP-001", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 5L, "Infraestrutura", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)), "Hospedagem em nuvem com alta disponibilidade", "Hospedagem Cloud Premium", 99.90m, "HOS-001", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                table: "Address",
                columns: new[] { "Id", "City", "CompanyId", "CompanyId1", "ContactId", "CreatedAt", "Neighborhood", "Number", "State", "Street", "UpdatedAt", "ZipCode" },
                values: new object[,]
                {
                    { 1L, "São Paulo", 1L, null, null, new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)), "Bela Vista", "1000", "SP", "Av. Paulista", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)), "01234-567" },
                    { 2L, "Rio de Janeiro", 2L, null, null, new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)), "Centro", "500", "RJ", "Rua da Carioca", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)), "20000-000" },
                    { 3L, "Belo Horizonte", 3L, null, null, new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)), "Centro", "2000", "MG", "Av. Afonso Pena", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)), "30000-000" }
                });

            migrationBuilder.InsertData(
                table: "WhatsAppConfig",
                columns: new[] { "Id", "AccessToken", "BusinessAccountId", "CompanyId", "CreatedAt", "Enabled", "Name", "PhoneNumberId", "UpdatedAt", "WebHookSecret" },
                values: new object[,]
                {
                    { 1L, "token_1_static-guid-123", "business_1", 1L, new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)), true, "WhatsApp - TechCorp Solutions", "phone_1", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)), "secret_1_static-guid-123" },
                    { 2L, "token_2_static-guid-123", "business_2", 2L, new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)), true, "WhatsApp - Inovação Digital", "phone_2", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)), "secret_2_static-guid-123" },
                    { 3L, "token_3_static-guid-123", "business_3", 3L, new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)), true, "WhatsApp - StartupHub", "phone_3", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)), "secret_3_static-guid-123" }
                });

            migrationBuilder.InsertData(
                table: "Integration",
                columns: new[] { "Id", "CompanyId", "CreatedAt", "UpdatedAt", "WhatsAppConfigId" },
                values: new object[,]
                {
                    { 1L, 1L, new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)), 1L },
                    { 2L, 2L, new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)), 2L },
                    { 3L, 3L, new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)), 3L }
                });

            migrationBuilder.InsertData(
                table: "Lead",
                columns: new[] { "Id", "AdCode", "CampaignId", "Channel", "CompanyId", "ConsultantId", "ContactId", "CreatedAt", "IntegrationId", "Phase", "SaleNote", "UpdatedAt" },
                values: new object[,]
                {
                    { 1L, "AD001", null, 1, 1L, null, 1L, new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)), 1L, (short)1, "Cliente interessado em CRM, aguardando proposta", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 2L, "FB001", null, 2, 1L, null, 2L, new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)), 1L, (short)2, "Cliente em processo de negociação para e-commerce", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 3L, "AD002", null, 1, 2L, null, 3L, new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)), 2L, (short)3, "Reunião agendada para apresentação da solução", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                table: "Lead",
                columns: new[] { "Id", "AdCode", "CampaignId", "Channel", "CompanyId", "ConsultantId", "ContactId", "CreatedAt", "IntegrationId", "Phase", "SaleNote", "Status", "UpdatedAt" },
                values: new object[] { 4L, "GOOGLE001", null, 3, 2L, null, 4L, new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)), 2L, (short)4, "Venda concluída - Consultoria em Transformação Digital", (short)4, new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "Lead",
                columns: new[] { "Id", "AdCode", "CampaignId", "Channel", "CompanyId", "ConsultantId", "ContactId", "CreatedAt", "IntegrationId", "Phase", "SaleNote", "UpdatedAt" },
                values: new object[] { 5L, "AD003", null, 1, 3L, null, 5L, new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)), 3L, (short)2, "Cliente interessado em desenvolvimento de app", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "Lead",
                columns: new[] { "Id", "AdCode", "CampaignId", "Channel", "CompanyId", "ConsultantId", "ContactId", "CreatedAt", "IntegrationId", "Phase", "SaleNote", "Status", "UpdatedAt" },
                values: new object[] { 6L, "FB002", null, 2, 3L, null, 6L, new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)), 3L, (short)4, "Cliente optou por concorrente", (short)3, new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 0, 0, 0, 0)) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Address",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Address",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Address",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Lead",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Lead",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Lead",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Lead",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "Lead",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "Lead",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "Contact",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Contact",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Contact",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Contact",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "Contact",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "Contact",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "Integration",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Integration",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Integration",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "WhatsAppConfig",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "WhatsAppConfig",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "WhatsAppConfig",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Company",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Company",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Company",
                keyColumn: "Id",
                keyValue: 3L);
        }
    }
}
