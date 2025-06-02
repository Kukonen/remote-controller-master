using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RemoteControllerMaster.Database.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCommands : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "commands_permissions",
                schema: "core");

            migrationBuilder.AddColumn<string>(
                name: "name",
                schema: "core",
                table: "commands",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "name",
                schema: "core",
                table: "commands");

            migrationBuilder.CreateTable(
                name: "commands_permissions",
                schema: "core",
                columns: table => new
                {
                    command_id = table.Column<Guid>(type: "uuid", nullable: false),
                    permission = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_commands_permissions", x => new { x.command_id, x.permission });
                    table.ForeignKey(
                        name: "FK_commands_permissions_permissions_permission",
                        column: x => x.permission,
                        principalSchema: "enum",
                        principalTable: "permissions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_commands_permissions_users_command_id",
                        column: x => x.command_id,
                        principalSchema: "core",
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_commands_permissions_permission",
                schema: "core",
                table: "commands_permissions",
                column: "permission");
        }
    }
}
