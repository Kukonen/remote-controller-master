using Microsoft.AspNetCore.Mvc;
using RemoteControllerMaster.Database.Models;
using RemoteControllerMaster.Database.Repositories.Interfaces;
using RemoteControllerMaster.Dtos.Auth;
using RemoteControllerMaster.Dtos.User;
using RemoteControllerMaster.Helpers.Authorize;
using RemoteControllerMaster.Mappers;
using RemoteControllerMaster.Presenters.Interfaces;


namespace RemoteControllerMaster.Presenters.Realization
{
    public class UserPresenter : IUserPresenter
    {
        private readonly IUserRepository _userRepository;
        private readonly IUser2PermissionRepository _user2PermissionRepository;
        private readonly IAuthorizeTokenRepository _tokenRepository;
        private readonly IUser2MachineRepository _user2MachineRepository;
        private readonly IMachineRepository _machineRepository;

        public UserPresenter(
            IUserRepository userRepository,
            IUser2PermissionRepository user2PermissionRepository,
            IAuthorizeTokenRepository tokenRepository,
            IUser2MachineRepository user2MachineRepository,
            IMachineRepository machineRepository
        )
        {
            _userRepository = userRepository;
            _user2PermissionRepository = user2PermissionRepository;
            _tokenRepository = tokenRepository;
            _user2MachineRepository = user2MachineRepository;
            _machineRepository = machineRepository;
        }

        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (await _userRepository.ExistsByLoginAsync(request.Login))
                return new BadRequestResult();

            var user = new User
            {
                UserId = Guid.NewGuid(),
                Login = request.Login,
                PasswordHash = PasswordHelper.HashPassword(new User()
                {
                    Login = request.Login,
                }, request.Password)
            };

            await _userRepository.AddAsync(user);
            await _user2PermissionRepository.AddPermissionsAsync(user.UserId, request.Permissions);

            return new OkResult();
        }

        public async Task<IActionResult> Login(LoginRequestDto loginRequest)
        {
            var user = await _userRepository.GetByLoginAsync(loginRequest.Login);

            if (user == null || !PasswordHelper.VerifyPassword(user, loginRequest.Password))
                return new UnauthorizedResult();

            var permissions = (await _user2PermissionRepository.GetPermissionsAsync(user.UserId))
                                .Select(p => p.Permission);

            var accessToken = JwtTokenHelper.GenerateToken(user);
            var refreshToken = JwtTokenHelper.GenerateRefreshToken();

            var tokenEntity = new AuthorizeToken
            {
                AuthorizeTokenId = Guid.NewGuid(),
                UserId = user.UserId,
                RefreshToken = refreshToken,
                ExpiryDate = DateTime.UtcNow.AddDays(7)
            };

            await _tokenRepository.AddAsync(tokenEntity);

            return new OkObjectResult(new LoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }

        public async Task<IActionResult> Refresh(RefreshRequest refreshRequest)
        {
            var token = await _tokenRepository.GetByRefreshTokenAsync(refreshRequest.RefreshToken);

            if (token == null || token.ExpiryDate <= DateTime.UtcNow)
                return new UnauthorizedResult();

            var user = await _userRepository.GetByUserIdAsync(token.UserId);
            if (user == null)
                return new UnauthorizedResult();

            var permissions = (await _user2PermissionRepository.GetPermissionsAsync(user.UserId))
                                .Select(p => p.Permission);

            var newAccessToken = JwtTokenHelper.GenerateToken(user);
            var newRefreshToken = JwtTokenHelper.GenerateRefreshToken();

            token.RefreshToken = newRefreshToken;
            token.ExpiryDate = DateTime.UtcNow.AddDays(7);

            await _tokenRepository.UpdateAsync(token);

            return new OkObjectResult(new RefreshResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }

        public async Task<IActionResult> GetUsersWithPermissionsAndMachines()
        {
            List<UsersWithPermissionsResponse> response = new();

            var users = await _userRepository.GetAllAsync();

            var users2machines = await _user2MachineRepository.GetByUsersIdsAsync(users.Select(u => u.UserId).ToArray());

            foreach (var userDo in users)
            {
                var user = userDo.MapToDto();
                var machines = (await _machineRepository.GetByMachinesIds(users2machines
                    .Where(u2m => u2m.UserId == userDo.UserId)
                    .Select(u2m => u2m.MachineId)
                    .ToArray()
                )).Select(m => m.MapToDto());
                var permissions = (await _user2PermissionRepository.GetPermissionsAsync(userDo.UserId)).Select(p => p.Permission);

                response.Add(new UsersWithPermissionsResponse
                {
                    User = user,
                    Machines = machines?.ToArray(),
                    Permissions = permissions?.ToArray()
                });
            }

            return new OkObjectResult(response);
        }

        public async Task<IActionResult> UpdateUserPermission(User2PermissionUpdateRequestDto request)
        {
            await _user2PermissionRepository.RemovePermissionsAsync(request.UserId);
            await _user2PermissionRepository.AddPermissionsAsync(request.UserId, request.Permissions);

            return new OkResult();
        }

        public async Task<IActionResult> UpdateUserMachines(User2MachineUpdateRequestDto request)
        {
            await _user2MachineRepository.RemoveMachinesAsync(request.UserId);
            await _user2MachineRepository.AddMachinesByUserIdAsync(request.UserId, request.MachinesIds);

            return new OkResult();
        }
    }
}
