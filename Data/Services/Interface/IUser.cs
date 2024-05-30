using TalentGroupsTest.Models;

namespace TalentGroupsTest.Data.Services.Interface
{
    public interface IUser
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<bool> AddUserAsync(User user);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(User user);
        Task<IEnumerable<User>> SearchUserAsync(string query);
    }
}
