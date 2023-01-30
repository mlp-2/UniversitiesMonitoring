using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using UniversityMonitoring.Data.Models;
using UniversityMonitoring.Data.Repositories;

namespace UniversitiesMonitoring.Api.Services
{
    internal class UsersProvider : IUsersProvider
    {
        private readonly IDataProvider _dataProvider;
        
        public UsersProvider(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }
        
        /// <summary>
        /// Сам такой
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<User?> GetUserAsync(ulong userId)
        {
            return await _dataProvider.Users.FindAsync(userId);
        }
        
        /// <summary>
        /// Получает пользователя по айди, а потом полученный инстанс прогоняет через модифэй экшен (юзер), мы можем поменять айди, юзернейм, вот этот метод должэен это делать
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="modifyAction"></param>
        /// <returns>возвращает тру, если удачно всё сделал, если удачно изменился объект.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> ModifyUserAsync(ulong userId, Action<User> modifyAction)
        {
            var user = await _dataProvider.Users.FindAsync(userId);
            
            if (user == null)
                return false;

            modifyAction(user);
            await _dataProvider.SaveChangesAsync();
            
            return true;
        }
        
        
        
        /// <summary>
        /// Просто тупо создёет нового пользователя, прям по-тупому
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<User> CreateUserAsync(string username, string password)
        {
            User user = new User()
            {
                Username = username,
                PasswordSha256hash = ComputeSha256(password)
            };

            await _dataProvider.Users.AddAsync(user);
            await _dataProvider.SaveChangesAsync();

            return user;
        }
        
        private static byte[] ComputeSha256(string s)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashValue = sha256.ComputeHash(Encoding.UTF8.GetBytes(s));
                return hashValue;
            }
        }
    }
}
