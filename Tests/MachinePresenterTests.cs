using Moq;
using Microsoft.AspNetCore.Mvc;
using RemoteControllerMaster.Database.Repositories.Interfaces;
using RemoteControllerMaster.Dtos.Machine;
using RemoteControllerMaster.Presenters.Realization;
using RemoteControllerMaster.Database.Models;
using RemoteControllerMaster.Models.App;

namespace Tests
{
    [TestClass]
    public class MachinePresenterTests
    {
        private Mock<IMachineRepository> _machineRepoMock;
        private Mock<IUser2MachineRepository> _user2MachineRepoMock;
        private MachinePresenter _presenter;

        [TestInitialize]
        public void Setup()
        {
            _machineRepoMock = new Mock<IMachineRepository>();
            _user2MachineRepoMock = new Mock<IUser2MachineRepository>();
            _presenter = new MachinePresenter(
                _machineRepoMock.Object,
                _user2MachineRepoMock.Object
            );
        }

        [TestMethod]
        public async Task GetAllMachines()
        {
            var m1 = new Machine
            {
                MachineId = Guid.NewGuid(),
                MachineName = "M1",
                IpAddress = "192.168.0.1"
            };
            var m2 = new Machine
            {
                MachineId = Guid.NewGuid(),
                MachineName = "M2",
                IpAddress = "10.0.0.2"
            };
            _machineRepoMock
                .Setup(r => r.GetAll())
                .ReturnsAsync(new Machine[] { m1, m2 });

            var result = await _presenter.GetAllMachines();

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var ok = (OkObjectResult)result;
            var dtos = ok.Value as IEnumerable<MachineDto>;
            Assert.IsNotNull(dtos);
            var list = dtos.ToList();
            Assert.AreEqual(2, list.Count);

            CollectionAssert.AreEquivalent(
                new List<Guid> { m1.MachineId, m2.MachineId },
                list.Select(d => d.MachineId).ToList()
            );
            CollectionAssert.AreEquivalent(
                new List<string> { m1.MachineName, m2.MachineName },
                list.Select(d => d.MachineName).ToList()
            );
        }

        [TestMethod]
        public async Task CreateMachine()
        {
            var dto = new MachineDto
            {
                MachineId = Guid.NewGuid(),
                MachineName = "NewM",
                IpAddress = "127.0.0.1"
            };

            var result = await _presenter.CreateMachine(dto);

            _machineRepoMock.Verify(r => r.AddMachine(It.Is<Machine>(m =>
                m.MachineId == dto.MachineId &&
                m.MachineName == dto.MachineName &&
                m.IpAddress == dto.IpAddress
            )), Times.Once);

            Assert.IsInstanceOfType(result, typeof(CreatedResult));
        }

        [TestMethod]
        public async Task GetUserMachines()
        {
            var allMachines = new Machine[]
            {
                new Machine { MachineId = Guid.NewGuid(), MachineName = "A", IpAddress = "1" },
                new Machine { MachineId = Guid.NewGuid(), MachineName = "B", IpAddress = "2" },
                new Machine { MachineId = Guid.NewGuid(), MachineName = "C", IpAddress = "3" }
            };
            _machineRepoMock
                .Setup(r => r.GetAll())
                .ReturnsAsync(allMachines);

            var userId = Guid.NewGuid();
            var user2machines = new List<User2Machine>
            {
                new User2Machine { UserId = userId, MachineId = allMachines[0].MachineId },
                new User2Machine { UserId = userId, MachineId = allMachines[2].MachineId }
            };
            _user2MachineRepoMock
                .Setup(r => r.GetByUsersIdsAsync(It.IsAny<Guid[]>()))
                .ReturnsAsync(user2machines);

            var context = new UserContext { UserId = userId };
            var result = await _presenter.GetUserMachines(context);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var ok = (OkObjectResult)result;
            var machines = ok.Value as IEnumerable<Machine>;
            Assert.IsNotNull(machines);
            var list = machines.ToList();

            CollectionAssert.AreEquivalent(
                new List<Guid> { allMachines[0].MachineId, allMachines[2].MachineId },
                list.Select(m => m.MachineId).ToList()
            );
        }

        [TestMethod]
        public async Task UpdateMachine()
        {
            var dto = new MachineDto
            {
                MachineId = Guid.NewGuid(),
                MachineName = "UpdM",
                IpAddress = "10.10.10.10"
            };

            var result = await _presenter.UpdateMachine(dto);

            _machineRepoMock.Verify(r => r.UpdateMachine(It.Is<Machine>(m =>
                m.MachineId == dto.MachineId &&
                m.MachineName == dto.MachineName &&
                m.IpAddress == dto.IpAddress
            )), Times.Once);

            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task DeleteMachine()
        {
            var id = Guid.NewGuid();

            var result = await _presenter.DeleteMachine(id);

            _machineRepoMock.Verify(r => r.RemoveMachine(id), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }
    }
}
