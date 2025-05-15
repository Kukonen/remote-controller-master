namespace RemoteControllerMaster.Dtos.User
{
    public class User2MachineUpdateRequestDto
    {
        public Guid UserId { get; set; }
        public Guid[] MachinesIds { get; set; }
    }
}
