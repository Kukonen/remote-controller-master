using RemoteControllerMaster.Database.Models;
using RemoteControllerMaster.Dtos.Commands;

namespace RemoteControllerMaster.Mappers
{
    public static class CommandMapper
    {
        public static CommandDto MapToDto(this Command command)
        {
            return new CommandDto()
            {
                CommandId = command.CommandId,
                Name = command.Name,
                CommandType = command.CommandType,
                CommandText = command.CommandText,
                AdditionalInformationText = command.AdditionalInformationText,
            };
        }

        public static Command MapToDo(this CommandDto commandDto)
        {
            return new Command()
            {
                CommandId = commandDto.CommandId,
                Name = commandDto.Name,
                CommandType = commandDto.CommandType,
                CommandText = commandDto.CommandText,
                AdditionalInformationText = commandDto.AdditionalInformationText,
            };
        }
    }
}
