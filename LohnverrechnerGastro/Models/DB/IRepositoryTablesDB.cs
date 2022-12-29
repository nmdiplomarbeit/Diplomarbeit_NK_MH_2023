using System.Threading.Tasks;

namespace LohnverrechnerGastro.Models.DB
{
    public interface IRepositoryTablesDB
    {
        Task ConnectAsync();
        Task DisconnectAsync();
        //Task<bool> InsertAsync(User user);
        Task<decimal> GetSVSatzAsync(decimal einkommen);
        //Task<bool> UpdateLoggedAsync(int userId, bool newLogged);
        Task<decimal> GetEffTarifAsync(decimal einkommen);
        Task<bool> AskEmailAsync(string email);
        Task<bool> LoginAsync(string name, string password);
        Task<bool> AskNameAsync(string name);
    }
}
