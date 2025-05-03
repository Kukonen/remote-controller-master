using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RemoteControllerMaster.Database.Migrations
{
    /// <inheritdoc />
    public partial class UserLogTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE TABLE core.user_log (
                    user_log_id UUID NOT NULL,
                    user_id UUID NOT NULL,
                    request TEXT,
                    response TEXT,
                    created_at TIMESTAMPTZ NOT NULL,
                    PRIMARY KEY (user_log_id, created_at),
                    FOREIGN KEY (user_id) REFERENCES core.users(user_id)
                )
                PARTITION BY RANGE (created_at);
            ");

            migrationBuilder.CreateIndex(
                name: "IX_user_log_user_id",
                schema: "core",
                table: "user_log",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_log",
                schema: "core");
        }
    }
}
