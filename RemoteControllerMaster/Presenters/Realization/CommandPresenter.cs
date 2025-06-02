using Microsoft.AspNetCore.Mvc;
using RemoteControllerCore.Commands;
using RemoteControllerMaster.Database.Repositories.Interfaces;
using RemoteControllerMaster.Dtos.Commands;
using RemoteControllerMaster.Mappers;
using RemoteControllerMaster.Presenters.Interfaces;
using System.Text.Json;


namespace RemoteControllerMaster.Presenters.Realization
{
    public class CommandPresenter : ICommandPresenter
    {
        private readonly ICommandRepository _commandRepository;
        private readonly IMachineRepository _machineRepository;
        private readonly HttpClient _httpClient;

        public CommandPresenter(
            ICommandRepository commandRepository,
            HttpClient httpClient,
            IMachineRepository machineRepository
        )
        {
            _commandRepository = commandRepository;
            _httpClient = httpClient;
            _machineRepository = machineRepository;
        }

        public async Task<IActionResult> GetAllCommands()
        {
            var commands = await _commandRepository.GetAll();

            return new OkObjectResult(commands.Select(c => c.MapToDto()));
        }

        public async Task<IActionResult> CreateCommand(CommandDto commandDto)
        {
            await _commandRepository.AddCommand(commandDto.MapToDo());

            return new CreatedResult();
        }
        public async Task<IActionResult> UpdateCommand(CommandDto commandDto)
        {
            await _commandRepository.UpdateCommand(commandDto.MapToDo());

            return new OkResult();
        }
        public async Task<IActionResult> DeleteCommand(Guid commandId)
        {
            await _commandRepository.RemoveCommand(commandId);

            return new OkResult();
        }

        public async Task<IActionResult> ExecuteCommand(ExecuteCommandDto executeCommandDto)
        {
            var command = (await _commandRepository.GetByCommandsIds([executeCommandDto.CommandId])).FirstOrDefault();
            var machine = (await _machineRepository.GetByMachinesIds([executeCommandDto.MachineId])).FirstOrDefault();

            if (command == null || machine == null)
            {
                return new BadRequestResult();
            }

            var commandParsed = new Command()
            {
                CommandId = command.CommandId,
                CommandType = (CommandType)((int)command.CommandType),
                CommandText = command.CommandText,
                AdditionalInformationText = command.AdditionalInformationText,
                File = executeCommandDto.File,
                FileName = executeCommandDto.FileName
            };
            string commandStr = JsonSerializer.Serialize(commandParsed);
            var commandContent = new StringContent(commandStr, System.Text.Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync($"http://{machine.IpAddress}/ExecuteCommand", commandContent);
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseParsed = JsonSerializer.Deserialize<CommandResult>(responseContent);

                return new OkObjectResult(responseContent);
            }
            catch (HttpRequestException ex)
            {
                return new StatusCodeResult(502);
            }
        }
    }
}
