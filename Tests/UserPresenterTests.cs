using Moq;
using Microsoft.AspNetCore.Mvc;
using RemoteControllerMaster.Database.Repositories.Interfaces;
using RemoteControllerMaster.Database.Models;
using RemoteControllerMaster.Dtos.Auth;
using RemoteControllerMaster.Dtos.User;
using RemoteControllerMaster.Helpers.Authorize;
using RemoteControllerMaster.Presenters.Realization;


namespace Tests
{
    [TestClass]
    public class UserPresenterTests
    {
        private Mock<IUserRepository> _userRepoMock;
        private Mock<IUser2PermissionRepository> _user2PermRepoMock;
        private Mock<IAuthorizeTokenRepository> _tokenRepoMock;
        private Mock<IUser2MachineRepository> _user2MachineRepoMock;
        private Mock<IMachineRepository> _machineRepoMock;
        private UserPresenter _presenter;

        [TestInitialize]
        public void Setup()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _user2PermRepoMock = new Mock<IUser2PermissionRepository>();
            _tokenRepoMock = new Mock<IAuthorizeTokenRepository>();
            _user2MachineRepoMock = new Mock<IUser2MachineRepository>();
            _machineRepoMock = new Mock<IMachineRepository>();

            _presenter = new UserPresenter(
                _userRepoMock.Object,
                _user2PermRepoMock.Object,
                _tokenRepoMock.Object,
                _user2MachineRepoMock.Object,
                _machineRepoMock.Object
            );
        }

        [TestMethod]
        public async Task Register_WithExistingUser()
        {
            var request = new RegisterRequest
            {
                Login = "alice",
                Password = "pass",
                Permissions = new List<RemoteControllerMaster.Enums.Permission> { RemoteControllerMaster.Enums.Permission.User_Read }
            };
            _userRepoMock
                .Setup(r => r.ExistsByLoginAsync("alice"))
                .ReturnsAsync(true);

            var result = await _presenter.Register(request);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task Register()
        {
            var request = new RegisterRequest
            {
                Login = "bob",
                Password = "secret",
                Permissions = new List<RemoteControllerMaster.Enums.Permission> { RemoteControllerMaster.Enums.Permission.User_Write, RemoteControllerMaster.Enums.Permission.Machine_Read }
            };
            _userRepoMock
                .Setup(r => r.ExistsByLoginAsync("bob"))
                .ReturnsAsync(false);

            var result = await _presenter.Register(request);

            _userRepoMock.Verify(r => r.AddAsync(It.Is<User>(u =>
                u.Login == request.Login &&
                !string.IsNullOrWhiteSpace(u.PasswordHash)
            )), Times.Once);

            _user2PermRepoMock.Verify(r => r.AddPermissionsAsync(
                It.IsAny<Guid>(),
                It.Is<List<RemoteControllerMaster.Enums.Permission>>(perms => perms.SequenceEqual(request.Permissions))
            ), Times.Once);

            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task Login_WithNoUser()
        {
            var loginDto = new LoginRequestDto { Login = "charlie", Password = "pwd" };
            _userRepoMock
                .Setup(r => r.GetByLoginAsync("charlie"))
                .ReturnsAsync((User)null);

            var result1 = await _presenter.Login(loginDto);
            Assert.IsInstanceOfType(result1, typeof(UnauthorizedResult));

            var existingUser = new User
            {
                UserId = Guid.NewGuid(),
                Login = "charlie",
                PasswordHash = PasswordHelper.HashPassword(new User { Login = "charlie" }, "correct")
            };
            _userRepoMock
                .Setup(r => r.GetByLoginAsync("charlie"))
                .ReturnsAsync(existingUser);

            var result2 = await _presenter.Login(loginDto);

            Assert.IsInstanceOfType(result2, typeof(UnauthorizedResult));
        }

        [TestMethod]
        public async Task Login()
        {
            var loginDto = new LoginRequestDto { Login = "david", Password = "pass123" };
            var userId = Guid.NewGuid();
            var userInDb = new User
            {
                UserId = userId,
                Login = "david",
                PasswordHash = PasswordHelper.HashPassword(new User { Login = "david" }, "pass123")
            };
            _userRepoMock
                .Setup(r => r.GetByLoginAsync("david"))
                .ReturnsAsync(userInDb);

            _user2PermRepoMock
                .Setup(r => r.GetPermissionsAsync(userId))
                .ReturnsAsync(new User2Permission[]
                {
                    new User2Permission { UserId = userId, Permission = RemoteControllerMaster.Enums.Permission.User_Read },
                    new User2Permission { UserId = userId, Permission = RemoteControllerMaster.Enums.Permission.Machine_Write }
                });

            var result = await _presenter.Login(loginDto);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var ok = (OkObjectResult)result;
            Assert.IsInstanceOfType(ok.Value, typeof(LoginResponseDto));

            _tokenRepoMock.Verify(r => r.AddAsync(It.Is<AuthorizeToken>(t =>
                t.UserId == userId &&
                !string.IsNullOrWhiteSpace(t.RefreshToken)
            )), Times.Once);
        }

        [TestMethod]
        public async Task InvalidRefreshToken()
        {
            var refreshReq = new RefreshRequest { RefreshToken = "nope" };
            _tokenRepoMock
                .Setup(r => r.GetByRefreshTokenAsync("nope"))
                .ReturnsAsync((AuthorizeToken)null);

            var result1 = await _presenter.Refresh(refreshReq);
            Assert.IsInstanceOfType(result1, typeof(UnauthorizedResult));

            var expiredToken = new AuthorizeToken
            {
                AuthorizeTokenId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                RefreshToken = "old",
                ExpiryDate = DateTime.UtcNow.AddDays(-1)
            };
            _tokenRepoMock
                .Setup(r => r.GetByRefreshTokenAsync("old"))
                .ReturnsAsync(expiredToken);

            var result2 = await _presenter.Refresh(new RefreshRequest { RefreshToken = "old" });
            Assert.IsInstanceOfType(result2, typeof(UnauthorizedResult));
        }

        [TestMethod]
        public async Task ValidRefreshToken()
        {
            var oldToken = new AuthorizeToken
            {
                AuthorizeTokenId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                RefreshToken = "valid",
                ExpiryDate = DateTime.UtcNow.AddDays(1)
            };
            _tokenRepoMock
                .Setup(r => r.GetByRefreshTokenAsync("valid"))
                .ReturnsAsync(oldToken);

            var userInDb = new User { UserId = oldToken.UserId, Login = "eve" };
            _userRepoMock
                .Setup(r => r.GetByUserIdAsync(oldToken.UserId))
                .ReturnsAsync(userInDb);

            _user2PermRepoMock
                .Setup(r => r.GetPermissionsAsync(oldToken.UserId))
                .ReturnsAsync(Array.Empty<User2Permission>());

            var result = await _presenter.Refresh(new RefreshRequest { RefreshToken = "valid" });

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var ok = (OkObjectResult)result;
            Assert.IsInstanceOfType(ok.Value, typeof(RefreshResponse));

            _tokenRepoMock.Verify(r => r.UpdateAsync(It.Is<AuthorizeToken>(t =>
                t.AuthorizeTokenId == oldToken.AuthorizeTokenId &&
                t.RefreshToken != "valid" &&
                t.ExpiryDate > DateTime.UtcNow
            )), Times.Once);
        }

        [TestMethod]
        public async Task GetUsersWithPermissionsAndMachines()
        {
            var u1 = new User { UserId = Guid.NewGuid(), Login = "user1" };
            var u2 = new User { UserId = Guid.NewGuid(), Login = "user2" };
            _userRepoMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(new User[] { u1, u2 });

            var u2mList = new List<User2Machine>
            {
                new User2Machine { UserId = u1.UserId, MachineId = Guid.NewGuid() },
                new User2Machine { UserId = u2.UserId, MachineId = Guid.NewGuid() }
            };
            _user2MachineRepoMock
                .Setup(r => r.GetByUsersIdsAsync(It.IsAny<Guid[]>()))
                .ReturnsAsync(u2mList);

            var machineDo = new Machine
            {
                MachineId = u2mList[0].MachineId,
                MachineName = "M1",
                IpAddress = "ip1"
            };
            var machineDo2 = new Machine
            {
                MachineId = u2mList[1].MachineId,
                MachineName = "M2",
                IpAddress = "ip2"
            };
            _machineRepoMock
                .Setup(r => r.GetByMachinesIds(It.IsAny<Guid[]>()))
                .ReturnsAsync(new Machine[] { machineDo, machineDo2 });

            _user2PermRepoMock
                .Setup(r => r.GetPermissionsAsync(u1.UserId))
                .ReturnsAsync(new User2Permission[]
                {
                    new User2Permission { UserId = u1.UserId, Permission = RemoteControllerMaster.Enums.Permission.Command_Read }
                });
            _user2PermRepoMock
                .Setup(r => r.GetPermissionsAsync(u2.UserId))
                .ReturnsAsync(new User2Permission[]
                {
                    new User2Permission { UserId = u2.UserId, Permission = RemoteControllerMaster.Enums.Permission.User_Write }
                });

            var result = await _presenter.GetUsersWithPermissionsAndMachines();

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var ok = (OkObjectResult)result;
            var response = ok.Value as IEnumerable<UsersWithPermissionsResponse>;
            Assert.IsNotNull(response);

            var list = response.ToList();
            Assert.AreEqual(2, list.Count);

            var first = list.First(r => r.User.UserId == u1.UserId);
            Assert.AreEqual("user1", first.User.Login);
            CollectionAssert.AreEqual(
                new[] { RemoteControllerMaster.Enums.Permission.Command_Read },
                first.Permissions
            );
            Assert.AreEqual(2, first.Machines.Length);
            Assert.AreEqual(machineDo.MachineId, first.Machines[0].MachineId);
        }

        [TestMethod]
        public async Task UpdateUserPermission()
        {
            var userId = Guid.NewGuid();
            var request = new User2PermissionUpdateRequestDto
            {
                UserId = userId,
                Permissions = new[] { RemoteControllerMaster.Enums.Permission.Command_Write, RemoteControllerMaster.Enums.Permission.Machine_Read }
            };

            var result = await _presenter.UpdateUserPermission(request);

            _user2PermRepoMock.Verify(r => r.RemovePermissionsAsync(userId), Times.Once);
            _user2PermRepoMock.Verify(r => r.AddPermissionsAsync(userId, It.Is<RemoteControllerMaster.Enums.Permission[]>(perms =>
                perms.SequenceEqual(request.Permissions)
            )), Times.Once);

            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task UpdateUserMachines()
        {
            var userId = Guid.NewGuid();
            var request = new User2MachineUpdateRequestDto
            {
                UserId = userId,
                MachinesIds = new[] { Guid.NewGuid(), Guid.NewGuid() }
            };

            var result = await _presenter.UpdateUserMachines(request);

            _user2MachineRepoMock.Verify(r => r.RemoveMachinesAsync(userId), Times.Once);
            _user2MachineRepoMock.Verify(r => r.AddMachinesByUserIdAsync(userId, It.Is<Guid[]>(ids =>
                ids.SequenceEqual(request.MachinesIds)
            )), Times.Once);

            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task DeleteUser()
        {
            var userId = Guid.NewGuid();

            var result = await _presenter.DeleteUser(userId);

            _user2PermRepoMock.Verify(r => r.RemovePermissionsAsync(userId), Times.Once);
            _user2MachineRepoMock.Verify(r => r.RemoveMachinesAsync(userId), Times.Once);
            _userRepoMock.Verify(r => r.RemoveAsync(userId), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task GetUserWithPermissionsAndMachines_WithNoUser()
        {
            var userId = Guid.NewGuid();
            _userRepoMock
                .Setup(r => r.GetByUserIdAsync(userId))
                .ReturnsAsync((User)null);

            var result = await _presenter.GetUserWithPermissionsAndMachines(userId);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task GetUserWithPermissionsAndMachines()
        {
            var userId = Guid.NewGuid();
            var dbUser = new User { UserId = userId, Login = "frank" };
            _userRepoMock
                .Setup(r => r.GetByUserIdAsync(userId))
                .ReturnsAsync(dbUser);

            var u2mList = new List<User2Machine>
            {
                new User2Machine { UserId = userId, MachineId = Guid.NewGuid() }
            };
            _user2MachineRepoMock
                .Setup(r => r.GetByUsersIdsAsync(It.IsAny<Guid[]>()))
                .ReturnsAsync(u2mList);

            var machineDo = new Machine
            {
                MachineId = u2mList[0].MachineId,
                MachineName = "MachineX",
                IpAddress = "1.1.1.1"
            };
            _machineRepoMock
                .Setup(r => r.GetByMachinesIds(It.IsAny<Guid[]>()))
                .ReturnsAsync(new Machine[] { machineDo });

            _user2PermRepoMock
                .Setup(r => r.GetPermissionsAsync(userId))
                .ReturnsAsync(new User2Permission[]
                {
                    new User2Permission { UserId = userId, Permission = RemoteControllerMaster.Enums.Permission.User_Read },
                    new User2Permission { UserId = userId, Permission = RemoteControllerMaster.Enums.Permission.Command_Write }
                });

            var result = await _presenter.GetUserWithPermissionsAndMachines(userId);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var ok = (OkObjectResult)result;
            var response = ok.Value as UsersWithPermissionsResponse;
            Assert.IsNotNull(response);

            Assert.AreEqual("frank", response.User.Login);
            CollectionAssert.AreEquivalent(
                new[] { RemoteControllerMaster.Enums.Permission.User_Read, RemoteControllerMaster.Enums.Permission.Command_Write },
                response.Permissions
            );
            Assert.AreEqual(1, response.Machines.Length);
            Assert.AreEqual(machineDo.MachineId, response.Machines[0].MachineId);
        }
    }
}
