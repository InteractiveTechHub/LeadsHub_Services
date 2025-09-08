using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LeadsHub.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUsersAndConsultants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "Enabled", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UpdatedAt", "UserName" },
                values: new object[,]
                {
                    { "user-001", 0, "ConcurrencyStamp123", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "joao.silva@techcorp.com.br", true, true, true, null, "JOAO.SILVA@TECHCORP.COM.BR", "JOAO.SILVA@TECHCORP.COM.BR", "AQAAAAEAACcQAAAAEHashExample123", "(11) 99999-9999", true, "SecurityStamp123", false, new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "joao.silva@techcorp.com.br" },
                    { "user-002", 0, "ConcurrencyStamp456", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "maria.santos@inovacaodigital.com.br", true, true, true, null, "MARIA.SANTOS@INOVACAODIGITAL.COM.BR", "MARIA.SANTOS@INOVACAODIGITAL.COM.BR", "AQAAAAEAACcQAAAAEHashExample123", "(21) 88888-8888", true, "SecurityStamp456", false, new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "maria.santos@inovacaodigital.com.br" },
                    { "user-003", 0, "ConcurrencyStamp789", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "pedro.oliveira@startuphub.com.br", true, true, true, null, "PEDRO.OLIVEIRA@STARTUPHUB.COM.BR", "PEDRO.OLIVEIRA@STARTUPHUB.COM.BR", "AQAAAAEAACcQAAAAEHashExample123", "(31) 77777-7777", true, "SecurityStamp789", false, new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "pedro.oliveira@startuphub.com.br" }
                });

            migrationBuilder.InsertData(
                table: "Consultant",
                columns: new[] { "Id", "CreatedAt", "Enabled", "FullName", "IdentityId", "NickName", "PhotoUrl", "TimeLastLeadAssigned", "UpdatedAt", "UserIdentityId" },
                values: new object[,]
                {
                    { 1L, new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), true, "João Silva", "user-001", "João", "https://example.com/joao.jpg", null, new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 2L, new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), true, "Maria Santos", "user-002", "Maria", "https://example.com/maria.jpg", null, new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 3L, new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), true, "Pedro Oliveira", "user-003", "Pedro", "https://example.com/pedro.jpg", null, new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null }
                });

            migrationBuilder.UpdateData(
                table: "Lead",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConsultantId",
                value: 1L);

            migrationBuilder.UpdateData(
                table: "Lead",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConsultantId",
                value: 1L);

            migrationBuilder.UpdateData(
                table: "Lead",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConsultantId",
                value: 2L);

            migrationBuilder.UpdateData(
                table: "Lead",
                keyColumn: "Id",
                keyValue: 4L,
                column: "ConsultantId",
                value: 2L);

            migrationBuilder.UpdateData(
                table: "Lead",
                keyColumn: "Id",
                keyValue: 5L,
                column: "ConsultantId",
                value: 3L);

            migrationBuilder.UpdateData(
                table: "Lead",
                keyColumn: "Id",
                keyValue: 6L,
                column: "ConsultantId",
                value: 3L);

            migrationBuilder.UpdateData(
                table: "Timeline",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConsultantId",
                value: 1L);

            migrationBuilder.UpdateData(
                table: "Timeline",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConsultantId",
                value: 1L);

            migrationBuilder.UpdateData(
                table: "Timeline",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConsultantId",
                value: 1L);

            migrationBuilder.UpdateData(
                table: "Timeline",
                keyColumn: "Id",
                keyValue: 4L,
                column: "ConsultantId",
                value: 1L);

            migrationBuilder.UpdateData(
                table: "Timeline",
                keyColumn: "Id",
                keyValue: 5L,
                column: "ConsultantId",
                value: 1L);

            migrationBuilder.UpdateData(
                table: "Timeline",
                keyColumn: "Id",
                keyValue: 6L,
                column: "ConsultantId",
                value: 1L);

            migrationBuilder.UpdateData(
                table: "Timeline",
                keyColumn: "Id",
                keyValue: 7L,
                column: "ConsultantId",
                value: 2L);

            migrationBuilder.UpdateData(
                table: "Timeline",
                keyColumn: "Id",
                keyValue: 8L,
                column: "ConsultantId",
                value: 2L);

            migrationBuilder.UpdateData(
                table: "Timeline",
                keyColumn: "Id",
                keyValue: 9L,
                column: "ConsultantId",
                value: 2L);

            migrationBuilder.UpdateData(
                table: "Timeline",
                keyColumn: "Id",
                keyValue: 10L,
                column: "ConsultantId",
                value: 2L);

            migrationBuilder.UpdateData(
                table: "Timeline",
                keyColumn: "Id",
                keyValue: 11L,
                column: "ConsultantId",
                value: 3L);

            migrationBuilder.UpdateData(
                table: "Timeline",
                keyColumn: "Id",
                keyValue: 12L,
                column: "ConsultantId",
                value: 3L);

            migrationBuilder.UpdateData(
                table: "Timeline",
                keyColumn: "Id",
                keyValue: 13L,
                column: "ConsultantId",
                value: 3L);

            migrationBuilder.UpdateData(
                table: "Timeline",
                keyColumn: "Id",
                keyValue: 14L,
                column: "ConsultantId",
                value: 3L);

            migrationBuilder.UpdateData(
                table: "Timeline",
                keyColumn: "Id",
                keyValue: 15L,
                column: "ConsultantId",
                value: 3L);

            migrationBuilder.UpdateData(
                table: "Timeline",
                keyColumn: "Id",
                keyValue: 16L,
                column: "ConsultantId",
                value: 3L);

            migrationBuilder.InsertData(
                table: "ConsultantCompany",
                columns: new[] { "CompanyId", "ConsultantId", "IdentityId", "CreatedAt", "Id", "UpdatedAt" },
                values: new object[,]
                {
                    { 1L, 1L, "user-001", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1L, new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 2L, 2L, "user-002", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2L, new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 3L, 3L, "user-003", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 3L, new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-001");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-002");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-003");

            migrationBuilder.DeleteData(
                table: "ConsultantCompany",
                keyColumns: new[] { "CompanyId", "ConsultantId", "IdentityId" },
                keyValues: new object[] { 1L, 1L, "user-001" });

            migrationBuilder.DeleteData(
                table: "ConsultantCompany",
                keyColumns: new[] { "CompanyId", "ConsultantId", "IdentityId" },
                keyValues: new object[] { 2L, 2L, "user-002" });

            migrationBuilder.DeleteData(
                table: "ConsultantCompany",
                keyColumns: new[] { "CompanyId", "ConsultantId", "IdentityId" },
                keyValues: new object[] { 3L, 3L, "user-003" });

            migrationBuilder.DeleteData(
                table: "Consultant",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Consultant",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Consultant",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.UpdateData(
                table: "Lead",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConsultantId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Lead",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConsultantId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Lead",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConsultantId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Lead",
                keyColumn: "Id",
                keyValue: 4L,
                column: "ConsultantId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Lead",
                keyColumn: "Id",
                keyValue: 5L,
                column: "ConsultantId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Lead",
                keyColumn: "Id",
                keyValue: 6L,
                column: "ConsultantId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Timeline",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConsultantId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Timeline",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConsultantId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Timeline",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConsultantId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Timeline",
                keyColumn: "Id",
                keyValue: 4L,
                column: "ConsultantId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Timeline",
                keyColumn: "Id",
                keyValue: 5L,
                column: "ConsultantId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Timeline",
                keyColumn: "Id",
                keyValue: 6L,
                column: "ConsultantId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Timeline",
                keyColumn: "Id",
                keyValue: 7L,
                column: "ConsultantId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Timeline",
                keyColumn: "Id",
                keyValue: 8L,
                column: "ConsultantId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Timeline",
                keyColumn: "Id",
                keyValue: 9L,
                column: "ConsultantId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Timeline",
                keyColumn: "Id",
                keyValue: 10L,
                column: "ConsultantId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Timeline",
                keyColumn: "Id",
                keyValue: 11L,
                column: "ConsultantId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Timeline",
                keyColumn: "Id",
                keyValue: 12L,
                column: "ConsultantId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Timeline",
                keyColumn: "Id",
                keyValue: 13L,
                column: "ConsultantId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Timeline",
                keyColumn: "Id",
                keyValue: 14L,
                column: "ConsultantId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Timeline",
                keyColumn: "Id",
                keyValue: 15L,
                column: "ConsultantId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Timeline",
                keyColumn: "Id",
                keyValue: 16L,
                column: "ConsultantId",
                value: null);
        }
    }
}
