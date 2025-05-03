using Microsoft.AspNetCore.Mvc;
using RemoteControllerMaster.Database.Models;
using RemoteControllerMaster.Database.Repositories.Interfaces;
using System.Text.Json;


namespace RemoteControllerMaster.Controllers
{
    [Route("api")]
    [ApiController]
    public partial class Controller : ControllerBase
    {
        private IUserLogRepository _userLogRepository;

        public Controller(IUserLogRepository userLogRepository)
        {
            _userLogRepository = userLogRepository;
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
