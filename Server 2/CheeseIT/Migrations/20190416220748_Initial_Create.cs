using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CheeseIT.Migrations
{
    public partial class Initial_Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cheeses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Base64Image = table.Column<string>(nullable: true),
                    IdealHumidity = table.Column<float>(nullable: false),
                    HumidityThreshold = table.Column<float>(nullable: false),
                    IdealTemperature = table.Column<float>(nullable: false),
                    TemperatureThreshold = table.Column<float>(nullable: false),
                    DaysToRipe = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cheeses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Experiments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    IdealHumidity = table.Column<float>(nullable: false),
                    HumidityThreshold = table.Column<float>(nullable: false),
                    IdealTemperature = table.Column<float>(nullable: false),
                    TemperatureThreshold = table.Column<float>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EstimatedEndTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Experiments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ripenings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CheeseId = table.Column<Guid>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ripenings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ripenings_Cheeses_CheeseId",
                        column: x => x.CheeseId,
                        principalTable: "Cheeses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Observation",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Base64Image = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    DateTime = table.Column<DateTime>(nullable: false),
                    ExperimentId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Observation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Observation_Experiments_ExperimentId",
                        column: x => x.ExperimentId,
                        principalTable: "Experiments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Measurement",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Humidity = table.Column<float>(nullable: false),
                    Temperature = table.Column<float>(nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false),
                    ExperimentId = table.Column<Guid>(nullable: true),
                    RipeningId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Measurement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Measurement_Experiments_ExperimentId",
                        column: x => x.ExperimentId,
                        principalTable: "Experiments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Measurement_Ripenings_RipeningId",
                        column: x => x.RipeningId,
                        principalTable: "Ripenings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Measurement_ExperimentId",
                table: "Measurement",
                column: "ExperimentId");

            migrationBuilder.CreateIndex(
                name: "IX_Measurement_RipeningId",
                table: "Measurement",
                column: "RipeningId");

            migrationBuilder.CreateIndex(
                name: "IX_Observation_ExperimentId",
                table: "Observation",
                column: "ExperimentId");

            migrationBuilder.CreateIndex(
                name: "IX_Ripenings_CheeseId",
                table: "Ripenings",
                column: "CheeseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Measurement");

            migrationBuilder.DropTable(
                name: "Observation");

            migrationBuilder.DropTable(
                name: "Ripenings");

            migrationBuilder.DropTable(
                name: "Experiments");

            migrationBuilder.DropTable(
                name: "Cheeses");
        }
    }
}
