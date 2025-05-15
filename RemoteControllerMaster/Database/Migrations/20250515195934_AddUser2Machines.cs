using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RemoteControllerMaster.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddUser2Machines : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "machine_name",
                schema: "core",
                table: "machines",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "users_machines",
                schema: "core",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    machine_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users_machines", x => new { x.user_id, x.machine_id });
                    table.ForeignKey(
                        name: "FK_users_machines_machines_machine_id",
                        column: x => x.machine_id,
                        principalSchema: "core",
                        principalTable: "machines",
                        principalColumn: "machine_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_users_machines_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "core",
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_users_machines_machine_id",
                schema: "core",
                table: "users_machines",
                column: "machine_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "users_machines",
                schema: "core");

            migrationBuilder.DropColumn(
                name: "machine_name",
                schema: "core",
                table: "machines");
        }
    }
}
