using System.Collections.Generic;
using System.Threading.Tasks;

namespace LohnverrechnerGastro.Models.DB
{
    public interface IRepositoryUsersDB
    {
        Task ConnectAsync();
        Task DisconnectAsync();
        Task<bool> DeleteAsync(int userId);
        Task<bool> InsertAsync(User user);
        Task<User> GetKundeAsync(int userId);
        Task<bool> LoginAsync(string name, string password);
    }
}
