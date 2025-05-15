using Microsoft.AspNetCore.Mvc;
using RemoteControllerMaster.Database.Models;
using RemoteControllerMaster.Database.Repositories.Interfaces;
using RemoteControllerMaster.Presenters.Interfaces;
using System.Text.Json;


namespace RemoteControllerMaster.Controllers
{
    [Route("api")]
    [ApiController]
    public partial class Controller : ControllerBase
    {
        private readonly IUserLogRepository _userLogRepository;
        private readonly IUserPresenter _userPresenter;
        private readonly IMachinePresenter _machinePresenter;

        public Controller(
            IUserLogRepository userLogRepository, 
            IUserPresenter userPresenter,
            IMachinePresenter machinePresenter
        )
        {
            _userLogRepository = userLogRepository;
            _userPresenter = userPresenter;
            _machinePresenter = machinePresenter;
        }

        private async Task UserLogAsync(Guid? userId = null, object? request = null, object? respose = null, object? aditional = null)
        {
            await _userLogRepository.AddUserLogAsync(new UserLog()
            {
                UserLogId = Guid.NewGuid(),
                UserId = userId,
                Request = JsonSerializer.Serialize(request),
                Response = JsonSerializer.Serialize(respose),
                CreatedAt = DateTime.UtcNow
            });
        }
    }
}
