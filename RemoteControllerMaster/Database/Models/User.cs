namespace RemoteControllerMaster.Database.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
    }
}
