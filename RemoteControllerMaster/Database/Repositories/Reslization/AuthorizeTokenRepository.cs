using Microsoft.EntityFrameworkCore;
using RemoteControllerMaster.Database.Models;
using RemoteControllerMaster.Database.Repositories.Interfaces;


namespace RemoteControllerMaster.Database.Repositories.Reslization
{
    public class AuthorizeTokenRepository : IAuthorizeTokenRepository
    {
        private readonly ApplicationDbContext _context;

        public AuthorizeTokenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AuthorizeToken?> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _context.AuthorizeTokens
                .FirstOrDefaultAsync(t => t.RefreshToken == refreshToken);
        }

        public async Task AddAsync(AuthorizeToken token)
        {
            _context.AuthorizeTokens.Add(token);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(AuthorizeToken token)
        {
            _context.AuthorizeTokens.Update(token);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(AuthorizeToken token)
        {
            _context.AuthorizeTokens.Remove(token);
            await _context.SaveChangesAsync();
        }
    }
}
