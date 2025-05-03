namespace RemoteControllerMaster.Database.Models
{
    public class User2Permission
    {
        public Guid UserId { get; set; }
        public Enums.Permission Permission { get; set; }
    }
}
