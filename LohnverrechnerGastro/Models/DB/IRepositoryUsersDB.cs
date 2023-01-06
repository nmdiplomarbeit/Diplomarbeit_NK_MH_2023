using System.Collections.Generic;
using System.Threading.Tasks;

namespace LohnverrechnerGastro.Models.DB
{
    public interface IRepositoryUsersDB
    {
        Task ConnectAsync();
        Task DisconnectAsync();
        Task<bool> InsertAsync(User user, string salt);
        Task<User> GetUserAsync(int userId);
        //Task<bool> UpdateLoggedAsync(int userId, bool newLogged);
        Task<bool> AskEmailAsync(string email);
        Task<bool> LoginAsync(User userdaten);
        Task<bool> AskNameAsync(string name);
    }
}
