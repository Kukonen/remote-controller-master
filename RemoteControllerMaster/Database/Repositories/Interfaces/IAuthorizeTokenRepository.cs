using RemoteControllerMaster.Database.Models;

namespace RemoteControllerMaster.Database.Repositories.Interfaces
{
    public interface IAuthorizeTokenRepository
    {
        Task<AuthorizeToken?> GetByRefreshTokenAsync(string refreshToken);
        Task AddAsync(AuthorizeToken token);
        Task UpdateAsync(AuthorizeToken token);
        Task DeleteAsync(AuthorizeToken token);
    }
}
