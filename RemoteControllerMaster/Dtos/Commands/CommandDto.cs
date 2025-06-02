namespace RemoteControllerMaster.Dtos.Commands
{
    public class CommandDto
    {
        public Guid CommandId { get; set; }
        public string Name { get; set; }
        public Enums.CommandType CommandType { get; set; }
        public string CommandText { get; set; }
        public string AdditionalInformationText { get; set; }
    }
}
