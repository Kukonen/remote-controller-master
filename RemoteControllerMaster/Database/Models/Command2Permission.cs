namespace RemoteControllerMaster.Database.Models
{
    public class Command2Permission
    {
        public Guid CommandId { get; set; }
        public Enums.Permission Permission { get; set; }
    }
}
