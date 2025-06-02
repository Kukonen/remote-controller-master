using Moq;
using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using RemoteControllerMaster.Database.Repositories.Interfaces;
using RemoteControllerMaster.Dtos.Commands;
using RemoteControllerMaster.Presenters.Realization;
using RemoteControllerMaster.Database.Models;
using Tests.Stubs;


namespace Tests
{
    [TestClass]
    public class CommandPresenterTests
    {
        private Mock<ICommandRepository> _commandRepoMock;
        private Mock<IMachineRepository> _machineRepoMock;
        private HttpClient _httpClient;
        private CommandPresenter _presenter;

        [TestInitialize]
        public void Setup()
        {
            _commandRepoMock = new Mock<ICommandRepository>();
            _machineRepoMock = new Mock<IMachineRepository>();

            _httpClient = new HttpClient(new HttpMessageHandlerStub(null, HttpStatusCode.OK, "{}"));
            _presenter = new CommandPresenter(
                _commandRepoMock.Object,
                _httpClient,
                _machineRepoMock.Object
            );
        }

        [TestMethod]
        public async Task GetAllCommands()
        {
            // Arrange
            var cmd1 = new Command
            {
                CommandId = Guid.NewGuid(),
                Name = "First",
                CommandType = RemoteControllerMaster.Enums.CommandType.Console,
                CommandText = "echo 1",
                AdditionalInformationText = "info1"
            };
            var cmd2 = new Command
            {
                CommandId = Guid.NewGuid(),
                Name = "Second",
                CommandType = RemoteControllerMaster.Enums.CommandType.HTTP,
                CommandText = "GET /",
                AdditionalInformationText = "info2"
            };
            _commandRepoMock
                .Setup(r => r.GetAll())
                .ReturnsAsync(new Command[] { cmd1, cmd2 });

            var result = await _presenter.GetAllCommands();

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var ok = (OkObjectResult)result;
            var dtos = ok.Value as IEnumerable<CommandDto>;
            Assert.IsNotNull(dtos);
            var list = dtos.ToList();
            Assert.AreEqual(2, list.Count);

            CollectionAssert.AreEquivalent(
                new List<Guid> { cmd1.CommandId, cmd2.CommandId },
                list.Select(d => d.CommandId).ToList()
            );
            CollectionAssert.AreEquivalent(
                new List<string> { cmd1.Name, cmd2.Name },
                list.Select(d => d.Name).ToList()
            );
        }

        [TestMethod]
        public async Task CreateCommand()
        {
            var dto = new CommandDto
            {
                CommandId = Guid.NewGuid(),
                Name = "NewCmd",
                CommandType = RemoteControllerMaster.Enums.CommandType.Console,
                CommandText = "echo hi",
                AdditionalInformationText = "none"
            };

            var result = await _presenter.CreateCommand(dto);

            _commandRepoMock.Verify(r => r.AddCommand(It.Is<Command>(c =>
                c.CommandId == dto.CommandId &&
                c.Name == dto.Name &&
                c.CommandText == dto.CommandText
            )), Times.Once);

            Assert.IsInstanceOfType(result, typeof(CreatedResult));
        }

        [TestMethod]
        public async Task UpdateCommand()
        {
            var dto = new CommandDto
            {
                CommandId = Guid.NewGuid(),
                Name = "UpdCmd",
                CommandType = RemoteControllerMaster.Enums.CommandType.HTTP,
                CommandText = "POST /",
                AdditionalInformationText = "data"
            };

            var result = await _presenter.UpdateCommand(dto);

            _commandRepoMock.Verify(r => r.UpdateCommand(It.Is<Command>(c =>
                c.CommandId == dto.CommandId &&
                c.Name == dto.Name &&
                c.CommandText == dto.CommandText
            )), Times.Once);

            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task DeleteCommand()
        {
            var id = Guid.NewGuid();

            var result = await _presenter.DeleteCommand(id);

            _commandRepoMock.Verify(r => r.RemoveCommand(id), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task ExecuteCommand_WithCommandOrMachineNotFound()
        {
            _commandRepoMock
                .Setup(r => r.GetByCommandsIds(It.IsAny<Guid[]>()))
                .ReturnsAsync(Array.Empty<Command>());

            _machineRepoMock
                .Setup(r => r.GetByMachinesIds(It.IsAny<Guid[]>()))
                .ReturnsAsync(Array.Empty<RemoteControllerMaster.Database.Models.Machine>());

            var dto = new ExecuteCommandDto
            {
                CommandId = Guid.NewGuid(),
                MachineId = Guid.NewGuid(),
                File = null,
                FileName = null
            };

            var result = await _presenter.ExecuteCommand(dto);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task ExecuteCommand_WithHttpRequestException()
        {
            var commandId = Guid.NewGuid();
            var machineId = Guid.NewGuid();

            var dbCmd = new Command
            {
                CommandId = commandId,
                Name = "Cmd",
                CommandType = RemoteControllerMaster.Enums.CommandType.Console,
                CommandText = "echo",
                AdditionalInformationText = "info"
            };
            var dbMachine = new RemoteControllerMaster.Database.Models.Machine
            {
                MachineId = machineId,
                MachineName = "M1",
                IpAddress = "127.0.0.1"
            };

            _commandRepoMock
                .Setup(r => r.GetByCommandsIds(It.IsAny<Guid[]>()))
                .ReturnsAsync(new Command[] { dbCmd });
            _machineRepoMock
                .Setup(r => r.GetByMachinesIds(It.IsAny<Guid[]>()))
                .ReturnsAsync(new RemoteControllerMaster.Database.Models.Machine[] { dbMachine });

            var handler = new HttpMessageHandlerStub(
                new HttpRequestException("fail"),
                HttpStatusCode.BadGateway,
                null
            );
            _httpClient = new HttpClient(handler);
            _presenter = new CommandPresenter(_commandRepoMock.Object, _httpClient, _machineRepoMock.Object);

            var dto = new ExecuteCommandDto
            {
                CommandId = commandId,
                MachineId = machineId,
                File = Encoding.UTF8.GetBytes("payload"),
                FileName = "file.txt"
            };

            var result = await _presenter.ExecuteCommand(dto);

            var status = result as StatusCodeResult;
            Assert.IsNotNull(status);
            Assert.AreEqual(502, status.StatusCode);
        }

        [TestMethod]
        public async Task ExecuteCommand()
        {
            var commandId = Guid.NewGuid();
            var machineId = Guid.NewGuid();

            var dbCmd = new Command
            {
                CommandId = commandId,
                Name = "Cmd",
                CommandType = RemoteControllerMaster.Enums.CommandType.HTTP,
                CommandText = "POST /run",
                AdditionalInformationText = "info"
            };
            var dbMachine = new RemoteControllerMaster.Database.Models.Machine
            {
                MachineId = machineId,
                MachineName = "M1",
                IpAddress = "localhost:5000"
            };

            _commandRepoMock
                .Setup(r => r.GetByCommandsIds(It.IsAny<Guid[]>()))
                .ReturnsAsync(new Command[] { dbCmd });
            _machineRepoMock
                .Setup(r => r.GetByMachinesIds(It.IsAny<Guid[]>()))
                .ReturnsAsync(new RemoteControllerMaster.Database.Models.Machine[] { dbMachine });

            var sampleResult = new RemoteControllerCore.Commands.CommandResult()
            {
                IsSuccess = true,
                Result = "OK"
            };
            var jsonResponse = JsonSerializer.Serialize(sampleResult);
            var handler = new HttpMessageHandlerStub(
                exceptionToThrow: null,
                statusCode: HttpStatusCode.OK,
                responseContent: jsonResponse
            );
            _httpClient = new HttpClient(handler);
            _presenter = new CommandPresenter(_commandRepoMock.Object, _httpClient, _machineRepoMock.Object);

            var dto = new ExecuteCommandDto
            {
                CommandId = commandId,
                MachineId = machineId,
                File = null,
                FileName = null
            };

            var result = await _presenter.ExecuteCommand(dto);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var ok = (OkObjectResult)result;
            Assert.AreEqual(jsonResponse, ok.Value);
        }
    }
}
