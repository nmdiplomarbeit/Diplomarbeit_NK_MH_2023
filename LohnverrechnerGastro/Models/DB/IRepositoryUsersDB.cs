using System.Collections.Generic;
using System.Threading.Tasks;

namespace LohnverrechnerGastro.Models.DB
{
    public interface IRepositoryUsersDB
    {
        Task ConnectAsync();
        Task DisconnectAsync();
        Task<bool> InsertAsync(User user);
        Task<User> GetUserAsync(int userId);
        //Task<bool> UpdateLoggedAsync(int userId, bool newLogged);
        Task<bool> AskEmailAsync(string email);
        Task<bool> LoginAsync(string name, string password);
        Task<bool> AskNameAsync(string name);
    }
}
