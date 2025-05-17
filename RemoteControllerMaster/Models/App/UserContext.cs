namespace RemoteControllerMaster.Models.App
{
    public class UserContext
    {
        public Guid UserId { get; set; }
        public Enums.Permission[] Permissions { get; set; }
    }
}
