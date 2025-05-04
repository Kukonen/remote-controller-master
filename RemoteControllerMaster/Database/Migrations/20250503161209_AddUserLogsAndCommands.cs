using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RemoteControllerMaster.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddUserLogsAndCommands : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "command_types",
                schema: "enum",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_command_types", x => x.id);
                });

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

            migrationBuilder.CreateTable(
                name: "commands",
                schema: "core",
                columns: table => new
                {
                    command_id = table.Column<Guid>(type: "uuid", nullable: false),
                    command_type = table.Column<int>(type: "integer", nullable: false),
                    command_text = table.Column<string>(type: "text", nullable: false),
                    additional_information_text = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_commands", x => x.command_id);
                    table.ForeignKey(
                        name: "FK_commands_command_types_command_type",
                        column: x => x.command_type,
                        principalSchema: "enum",
                        principalTable: "command_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_commands_command_type",
                schema: "core",
                table: "commands",
                column: "command_type");

            migrationBuilder.CreateIndex(
                name: "IX_commands_permissions_permission",
                schema: "core",
                table: "commands_permissions",
                column: "permission");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "commands",
                schema: "core");

            migrationBuilder.DropTable(
                name: "commands_permissions",
                schema: "core");

            migrationBuilder.DropTable(
                name: "command_types",
                schema: "enum");
        }
    }
}
