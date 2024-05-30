using TalentGroupsTest.Data.Repository.Interface;
using TalentGroupsTest.Data.Services.Interface;
using TalentGroupsTest.Models;
using System.Linq.Expressions;

namespace TalentGroupsTest.Data.Services
{
    public class UserService : IUser
    {
        private readonly IMongoRepository<User> _userRepository;

        public UserService(IMongoRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var userQuery = _userRepository.GetQueryable();
            var users = await _userRepository.FindAllAsync(userQuery);
            return users.Select(user => CryptoService.DecryptUser(user));
        }
        public async Task<bool> AddUserAsync(User user)
        {
            var encryptedUser = CryptoService.EncryptUser(user);
            var result = await _userRepository.InsertOneAsync(encryptedUser);
            return result;
        }
        public async Task<bool> DeleteUserAsync(User user)
        {
            Expression<Func<User, bool>> filterExpression = x => x.Id == user.Id;
            return await _userRepository.DeleteOneAsync(filterExpression);
        }
        public async Task<bool> UpdateUserAsync(User user)
        {
            Expression<Func<User, bool>> filterExpression = x => x.Id == user.Id;
            var encryptedUser = CryptoService.EncryptUser(user);
            return await _userRepository.ReplaceOneAsync(filterExpression, encryptedUser);
        }

        public async Task<IEnumerable<User>> SearchUserAsync(string query)
        {
            var userQuery = _userRepository.GetQueryable();
            var users = await _userRepository.FindAllAsync(userQuery);
            users = users.Select(user => CryptoService.DecryptUser(user));

            query = query?.ToLower() ?? string.Empty;

            var filteredUsers = users.Where(user =>
                (user.FirstName != null && user.FirstName.ToLower().Contains(query)) ||
                (user.LastName != null && user.LastName.ToLower().Contains(query)) ||
                (user.Address != null && user.Address.ToLower().Contains(query))
            );

            return filteredUsers;
        }
    }
}
