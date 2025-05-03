using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RemoteControllerMaster.Database.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserLogToUserIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "user_log_user_id_fkey",
                schema: "analytics",
                table: "user_log");

            migrationBuilder.AlterColumn<Guid>(
                name: "user_id",
                schema: "analytics",
                table: "user_log",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_user_log_users_user_id",
                schema: "analytics",
                table: "user_log",
                column: "user_id",
                principalSchema: "core",
                principalTable: "users",
                principalColumn: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_log_users_user_id",
                schema: "analytics",
                table: "user_log");

            migrationBuilder.AlterColumn<Guid>(
                name: "user_id",
                schema: "analytics",
                table: "user_log",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_user_log_users_user_id",
                schema: "analytics",
                table: "user_log",
                column: "user_id",
                principalSchema: "core",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
