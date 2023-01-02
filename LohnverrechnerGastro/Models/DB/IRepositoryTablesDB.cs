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
        Task<decimal> GetSVSatzSZAsync(decimal einkommen);

        //Task<bool> UpdateLoggedAsync(int userId, bool newLogged);
        Task<decimal> GetEffTarifAsync(decimal einkommen);
        Task<List<Table>> GetAllTablesAsync(string tablename);
        Task<Dictionary<string, decimal>> GetDGAbgaben();
        Task<Dictionary<string, decimal>> GetDGAbgabenSZ();
        Task<Dictionary<string, decimal>> GetGrenzenSV();
        Task<Dictionary<string, decimal>> GetGrenzenSVSZ();
        Task<decimal> GetSZSteuergrenzen(decimal einkommen);





    }
}
