namespace RemoteControllerMaster.Dtos.User
{
    public class User2PermissionUpdateRequestDto
    {
        public Guid UserId { get; set; }
        public Enums.Permission[] Permissions { get; set; }
    }
}
