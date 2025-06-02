using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RemoteControllerMaster.Database.Migrations
{
    /// <inheritdoc />
    public partial class BaseCommands : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE EXTENSION IF NOT EXISTS pgcrypto;

                INSERT INTO core.commands (
                    command_id,
                    command_type,
                    command_text,
                    additional_information_text,
                    name
                )
                VALUES
                    (
                        gen_random_uuid(),
                        1,
                        'Expand-Archive -Path ''app.zip'' -DestinationPath ''installer_temp'' -Force; Start-Process msiexec.exe -ArgumentList ''/i'', ''installer_temp\setup.msi'', ''/quiet'', ''/norestart'' -Wait',
                        '',
                        'Установка приложения | Windows | zip | msi'
                    ),
                    (
                        gen_random_uuid(),
                        1,
                        'Expand-Archive -Path ''app.zip'' -DestinationPath ''installer_temp'' -Force; Start-Process ''installer_temp\setup.exe'' -ArgumentList ''/quiet'', ''/norestart'' -Wait',
                        '',
                        'Установка приложения | Windows | zip | exe'
                    ),
                    (
                        gen_random_uuid(),
                        1,
                        'tar -xf app.tar.gz && cd app && ./configure && make && sudo make install',
                        '',
                        'Linux | tar | установка из исходников'
                    ),
                    (
                        gen_random_uuid(),
                        1,
                        'tar -xf app.tar.gz && cd app && chmod +x setup && ./setup',
                        '',
                        'Linux | tar | собранное из бинарного файла'
                    );

            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
