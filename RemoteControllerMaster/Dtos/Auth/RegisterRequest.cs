namespace RemoteControllerMaster.Dtos.Auth
{
    public class RegisterRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public List<Enums.Permission> Permissions { get; set; }
    }
}
