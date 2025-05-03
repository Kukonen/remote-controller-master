using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RemoteControllerMaster.Database.Migrations
{
    /// <inheritdoc />
    public partial class AuthTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "enum");

            migrationBuilder.RenameColumn(
                name: "password",
                schema: "core",
                table: "users",
                newName: "password_hash");

            migrationBuilder.CreateTable(
                name: "authorize_tokens",
                schema: "core",
                columns: table => new
                {
                    authorize_token_id = table.Column<Guid>(type: "uuid", nullable: false),
                    refresh_token = table.Column<string>(type: "text", nullable: false),
                    expiry_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_authorize_tokens", x => x.authorize_token_id);
                    table.ForeignKey(
                        name: "FK_authorize_tokens_user_id",
                        column: x => x.user_id,
                        principalSchema: "core",
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "permissions",
                schema: "enum",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permissions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users_permissions",
                schema: "core",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    permission = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users_permissions", x => new { x.user_id, x.permission });
                    table.ForeignKey(
                        name: "FK_users_permissions_Permission",
                        column: x => x.permission,
                        principalSchema: "enum",
                        principalTable: "permissions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_users_permissions_User_id",
                        column: x => x.user_id,
                        principalSchema: "core",
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_authorize_tokens_user_id",
                schema: "core",
                table: "authorize_tokens",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_permissions_permission",
                schema: "core",
                table: "users_permissions",
                column: "permission");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "authorize_tokens",
                schema: "core");

            migrationBuilder.DropTable(
                name: "users_permissions",
                schema: "core");

            migrationBuilder.DropTable(
                name: "permissions",
                schema: "enum");

            migrationBuilder.RenameColumn(
                name: "password_hash",
                schema: "core",
                table: "users",
                newName: "password");
        }
    }
}
