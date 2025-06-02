using Microsoft.AspNetCore.Mvc;
using RemoteControllerMaster.Dtos.Commands;


namespace RemoteControllerMaster.Controllers
{
    public partial class Controller
    {
        [HttpGet("GetAllCommands")]
        public async Task<IActionResult> GetAllCommands()
        {
            return await _commandPresenter.GetAllCommands();
        }

        [HttpPost("AddCommand")]
        public async Task<IActionResult> CreateCommand(CommandDto commandDto)
        {
            return await _commandPresenter.CreateCommand(commandDto);
        }

        [HttpPost("UpdateCommand")]
        public async Task<IActionResult> UpdateCommand(CommandDto commandDto)
        {
            return await _commandPresenter.UpdateCommand(commandDto);
        }

        [HttpDelete("DeleteCommand")]
        public async Task<IActionResult> DeleteCommand([FromQuery] Guid commandId)
        {
            return await _commandPresenter.DeleteCommand(commandId);
        }

        [HttpPost("ExecuteCommand")]
        [RequestSizeLimit(long.MaxValue)]
        [RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> ExecuteCommand([FromForm] Guid commandId, [FromForm] Guid machineId, [FromForm] IFormFile file)
        {
            ExecuteCommandDto executeCommandDto = new ExecuteCommandDto()
            {
                CommandId = commandId,
                MachineId = machineId,
            };

            if (file != null)
            {
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                executeCommandDto.File = memoryStream.ToArray();
                executeCommandDto.FileName = file.FileName;
            }

            return await _commandPresenter.ExecuteCommand(executeCommandDto);
        }
    }
}
