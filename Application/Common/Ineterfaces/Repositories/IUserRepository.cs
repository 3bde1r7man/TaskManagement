using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Common.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> FindByEmailAsync(string email);
        Task<User?> FindByIdAsync(int id);
        Task<IdentityResult> CreateUserAsync(User user, string password);
        Task<IdentityResult> UpdateUserAsync(User user);
        Task<bool> IsInRoleAsync(User user, string role);
        Task<IdentityResult> AddToRoleAsync(User user, string role);
        Task<IEnumerable<string>> GetRolesAsync(User user);
        Task<SignInResult> PasswordSignInAsync(User user, string password, bool isPersistent, bool lockoutOnFailure);
        Task SignOutAsync();
        Task<bool> CheckPasswordAsync(User user, string password);
        Task SaveChangesAsync();
        Task<bool> RoleExistsAsync(string roleName);
        Task<IdentityResult> CreateRoleAsync(string roleName);
        Task<IEnumerable<User>> GetUsersInRoleAsync(string roleName);
    }
}