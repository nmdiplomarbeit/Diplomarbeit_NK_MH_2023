using System.Collections.Generic;
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
        Task<List<Table>> GetAllTablesAsync(string tablename);
    }
}
