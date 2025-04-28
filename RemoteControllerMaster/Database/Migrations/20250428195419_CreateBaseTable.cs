using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RemoteControllerMaster.Database.Migrations
{
    /// <inheritdoc />
    public partial class CreateBaseTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "core");

            migrationBuilder.EnsureSchema(
                name: "analytics");

            migrationBuilder.CreateTable(
                name: "machines",
                schema: "core",
                columns: table => new
                {
                    machine_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_machines", x => x.machine_id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "core",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    password = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.user_id);
                });

            migrationBuilder.Sql(@"
                CREATE TABLE analytics.statistics (
                    statistic_id UUID NOT NULL,
                    machine_id UUID NOT NULL,
                    date TIMESTAMPTZ NOT NULL,
                    PRIMARY KEY (statistic_id, date),
                    FOREIGN KEY (machine_id) REFERENCES core.machines (machine_id)
                )
                PARTITION BY RANGE (date);
            ");

            migrationBuilder.CreateIndex(
                name: "IX_statistics_machine_id",
                schema: "analytics",
                table: "statistics",
                column: "machine_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "statistics",
                schema: "analytics");

            migrationBuilder.DropTable(
                name: "users",
                schema: "core");

            migrationBuilder.DropTable(
                name: "machines",
                schema: "core");
        }
    }
}
