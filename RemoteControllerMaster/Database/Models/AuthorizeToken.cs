namespace RemoteControllerMaster.Database.Models
{
    public class AuthorizeToken
    {
        public Guid AuthorizeTokenId { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiryDate { get; set; }
        public Guid UserId { get; set; }
    }
}
