using Microsoft.AspNetCore.Mvc;
using RemoteControllerMaster.Database.Models;
using RemoteControllerMaster.Database.Repositories.Interfaces;
using RemoteControllerMaster.Enums;
using RemoteControllerMaster.Helpers.Authorize;


namespace RemoteControllerMaster.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUser2PermissionRepository _user2PermissionRepository;
        private readonly IAuthorizeTokenRepository _tokenRepository;

        public AuthController(
            IUserRepository userRepository,
            IUser2PermissionRepository user2PermissionRepository,
            IAuthorizeTokenRepository tokenRepository)
        {
            _userRepository = userRepository;
            _user2PermissionRepository = user2PermissionRepository;
            _tokenRepository = tokenRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(string login, string password, List<Enums.Permission> permissions)
        {
            if (await _userRepository.ExistsByLoginAsync(login))
                return BadRequest("Пользователь уже существует");

            var user = new User
            {
                UserId = Guid.NewGuid(),
                Login = login,
                PasswordHash = PasswordHelper.HashPassword(null!, password)
            };

            await _userRepository.AddAsync(user);
            await _user2PermissionRepository.AddPermissionsAsync(user.UserId, permissions);

            return Ok("Регистрация прошла успешно");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string login, string password)
        {
            var user = await _userRepository.GetByLoginAsync(login);

            if (user == null || !PasswordHelper.VerifyPassword(user, password))
                return Unauthorized("Неверный логин или пароль");

            var permissions = (await _user2PermissionRepository.GetPermissionsAsync(user.UserId))
                                .Select(permission => permission.Permission);

            var accessToken = JwtTokenHelper.GenerateToken(user, permissions);
            var refreshToken = JwtTokenHelper.GenerateRefreshToken();

            var tokenEntity = new AuthorizeToken
            {
                AuthorizeTokenId = Guid.NewGuid(),
                UserId = user.UserId,
                RefreshToken = refreshToken,
                ExpiryDate = DateTime.UtcNow.AddDays(7)
            };

            await _tokenRepository.AddAsync(tokenEntity);

            return Ok(new
            {
                token = accessToken,
                refreshToken = refreshToken
            });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(string refreshToken)
        {
            var token = await _tokenRepository.GetByRefreshTokenAsync(refreshToken);

            if (token == null || token.ExpiryDate <= DateTime.UtcNow)
                return Unauthorized("Неверный или истёкший refresh token");

            var user = await _userRepository.GetByUserIdAsync(token.UserId);
            if (user == null)
                return Unauthorized("Пользователь не найден");

            var permissions = (await _user2PermissionRepository.GetPermissionsAsync(user.UserId))
                                .Select(permission => permission.Permission);

            var newAccessToken = JwtTokenHelper.GenerateToken(user, permissions);
            var newRefreshToken = JwtTokenHelper.GenerateRefreshToken();

            token.RefreshToken = newRefreshToken;
            token.ExpiryDate = DateTime.UtcNow.AddDays(7);

            await _tokenRepository.UpdateAsync(token);

            return Ok(new
            {
                token = newAccessToken,
                refreshToken = newRefreshToken
            });
        }
    }

}
