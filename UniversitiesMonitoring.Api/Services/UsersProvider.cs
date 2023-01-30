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
        /// Получает пользователя
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <returns>Пользователя. Null, если пользователь не найден</returns>
        public async Task<User?> GetUserAsync(ulong userId) => await _dataProvider.Users.FindAsync(userId);

        /// <summary>
        /// Изменяет пользователя по ID и методу
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <param name="modifyAction">Метод, который изменяет пользователя</param>
        /// <returns>Если удачно, то true</returns>
        public async Task<bool> ModifyUserAsync(ulong userId, Action<User> modifyAction)
        {
            var user = await _dataProvider.Users.FindAsync(userId);
            
            if (user == null) return false;

            modifyAction(user);
            await _dataProvider.SaveChangesAsync();
            
            return true;
        }
        
        /// <summary>
        /// Создает нового пользователя
        /// </summary>
        /// <param name="username">Имя пользоваетя</param>
        /// <param name="password">Пароль пользователя</param>
        /// <returns></returns>
        public async Task<User> CreateUserAsync(string username, string password)
        {
            var user = new User()
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
            using var sha256 = SHA256.Create();
            var hashValue = sha256.ComputeHash(Encoding.UTF8.GetBytes(s));
            return hashValue;
        }
    }
}
