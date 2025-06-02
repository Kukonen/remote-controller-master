using Microsoft.AspNetCore.SignalR;
using RemoteControllerCore.Telemetry;
using RemoteControllerMaster.Database.Repositories.Interfaces;
using System.Security.Claims;


namespace RemoteControllerMaster.Hubs
{
    public class MachinesHub : Hub
    {
        private readonly IStatisticsRepository _statisticsRepository;

        public MachinesHub(IStatisticsRepository statisticsRepository)
        {
            _statisticsRepository = statisticsRepository;
        }

        // Метод вызывается клиентом OPC UA для отправки данных
        public async Task SendData(string machineId, TelemetryPayload payload)
        {
            // Рассылаем данные всем подписчикам группы конкретной машины
            await Clients.Group(machineId).SendAsync("ReceiveData", machineId, payload);
            await _statisticsRepository.InsertAsync(new Database.Models.Statistic()
            {
                StatisticId = Guid.NewGuid(),
                MachineId = Guid.Parse(machineId),
                Date = DateTime.UtcNow,
                Variable = payload.Variable,
                Value = payload.Value
            });
        }

        // Подписка клиента на конкретную машину
        public async Task SubscribeToMachine(string machineId)
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            await Groups.AddToGroupAsync(Context.ConnectionId, machineId);
        }

        // Отсписка от машины
        public async Task UnsubscribeFromMachine(string machineId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, machineId);
        }
    }
}
