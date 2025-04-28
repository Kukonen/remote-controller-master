using Microsoft.AspNetCore.Mvc;
using RemoteControllerMaster.Database.Models;
using RemoteControllerMaster.Database.Repositories.Interfaces;


namespace RemoteControllerMaster.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<User>> Get()
        {
            return await _userRepository.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<User?> Get(Guid userId)
        {
            return await _userRepository.GetByUserIdAsync(userId);
        }

        [HttpPost]
        public async Task Post(User user)
        {
            await _userRepository.InsertAsync(user);
        }
    }
}
