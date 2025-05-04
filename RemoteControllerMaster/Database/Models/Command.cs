namespace RemoteControllerMaster.Database.Models
{
    public class Command
    {
        public Guid CommandId {  get; set; }
        public Enums.CommandType CommandType { get; set; }
        public string CommandText { get; set; }
        public string AdditionalInformationText { get; set; }
    }
}
