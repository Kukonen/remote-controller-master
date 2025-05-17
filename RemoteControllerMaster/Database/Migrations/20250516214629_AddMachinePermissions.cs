using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RemoteControllerMaster.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddMachinePermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO enum.permissions (id, name) 
                    VALUES (5, 'Machine_Read'),
                           (6, 'Machine_Write')
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
