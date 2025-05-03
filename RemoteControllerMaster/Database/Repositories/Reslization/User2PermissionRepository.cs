using Microsoft.EntityFrameworkCore;
using RemoteControllerMaster.Database.Models;
using RemoteControllerMaster.Database.Repositories.Interfaces;
using RemoteControllerMaster.Enums;


namespace RemoteControllerMaster.Database.Repositories.Reslization
{
    public class User2PermissionRepository : IUser2PermissionRepository
    {
        private readonly ApplicationDbContext _context;

        public User2PermissionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User2Permission[]> GetPermissionsAsync(Guid userId)
        {
            return await _context.User2Permissions
                .Where(up => up.UserId == userId)
                .ToArrayAsync();
        }

        public async Task AddPermissionsAsync(Guid userId, IEnumerable<Enums.Permission> permissions)
        {
            foreach (var permission in permissions)
            {
                _context.User2Permissions.Add(new User2Permission
                {
                    UserId = userId,
                    Permission = permission
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemovePermissionsAsync(Guid userId)
        {
            var userPermissions = _context.User2Permissions.Where(up => up.UserId == userId);
            _context.User2Permissions.RemoveRange(userPermissions);
            await _context.SaveChangesAsync();
        }
    }
}
