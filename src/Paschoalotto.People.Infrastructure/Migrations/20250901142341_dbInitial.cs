using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Paschoalotto.People.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class dbInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Individuals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FullName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Cpf = table.Column<string>(type: "TEXT", maxLength: 11, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Gender = table.Column<int>(type: "INTEGER", nullable: false),
                    PhotoPath = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    Street = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    Number = table.Column<string>(type: "TEXT", maxLength: 15, nullable: false),
                    Complement = table.Column<string>(type: "TEXT", maxLength: 60, nullable: true),
                    District = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    City = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    State = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    ZipCode = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Country = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Individuals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LegalEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CorporateName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    TradeName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Cnpj = table.Column<string>(type: "TEXT", maxLength: 14, nullable: false),
                    StateRegistration = table.Column<string>(type: "TEXT", maxLength: 40, nullable: true),
                    MunicipalRegistration = table.Column<string>(type: "TEXT", maxLength: 40, nullable: true),
                    LegalRepresentativeName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    LegalRepCpf = table.Column<string>(type: "TEXT", maxLength: 11, nullable: false),
                    LogoPath = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    Street = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    Number = table.Column<string>(type: "TEXT", maxLength: 15, nullable: false),
                    Complement = table.Column<string>(type: "TEXT", maxLength: 60, nullable: true),
                    District = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    City = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    State = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    ZipCode = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Country = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalEntities", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Individuals_Cpf",
                table: "Individuals",
                column: "Cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Individuals_Email",
                table: "Individuals",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Individuals_FullName",
                table: "Individuals",
                column: "FullName");

            migrationBuilder.CreateIndex(
                name: "IX_LegalEntities_Cnpj",
                table: "LegalEntities",
                column: "Cnpj",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LegalEntities_CorporateName",
                table: "LegalEntities",
                column: "CorporateName");

            migrationBuilder.CreateIndex(
                name: "IX_LegalEntities_Email",
                table: "LegalEntities",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_LegalEntities_LegalRepCpf",
                table: "LegalEntities",
                column: "LegalRepCpf");

            migrationBuilder.CreateIndex(
                name: "IX_LegalEntities_TradeName",
                table: "LegalEntities",
                column: "TradeName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Individuals");

            migrationBuilder.DropTable(
                name: "LegalEntities");
        }
    }
}
