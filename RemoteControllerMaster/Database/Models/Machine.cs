namespace RemoteControllerMaster.Database.Models
{
    public class Machine
    {
        public Guid MachineId { get; set; }
        public string MachineName { get; set; }
        public string IpAddress { get; set; }
    }
}
