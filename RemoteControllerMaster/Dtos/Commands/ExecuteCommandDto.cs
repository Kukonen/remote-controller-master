namespace RemoteControllerMaster.Dtos.Commands
{
    public class ExecuteCommandDto
    {
        public Guid CommandId { get; set; }
        public Guid MachineId { get; set; }
        public byte[] File { get; set; }
        public string FileName { get; set; }
    }
}
