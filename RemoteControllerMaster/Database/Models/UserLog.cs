namespace RemoteControllerMaster.Database.Models
{
    public class UserLog
    {
        public Guid UserLogId { get; set; }
        public Guid? UserId { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
