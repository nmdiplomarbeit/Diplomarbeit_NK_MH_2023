using Org.BouncyCastle.Asn1.X509;
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
        Task<decimal> GetEffTarifAsync(decimal einkommen, int anzkinder);
        Task<Dictionary<string, decimal>> GetDGAbgaben();
        Task<Dictionary<string, decimal>> GetDGAbgabenSZ();
        Task<Dictionary<string, decimal>> GetGrenzenSV();
        Task<Dictionary<string, decimal>> GetGrenzenSVSZ();
        Task<decimal> GetSZSteuergrenzen(decimal einkommen);
        Task<List<Table>> GetAllTablesAsync(string tablename);
        Task<decimal> GetBetrzugehArbeiter(int anzJahre);
        Task<decimal> GetBetrzugehAngestellter(int anzJahre);
        Task<decimal> GetProzBundesland(string bundesland);
        Task<bool> DeleteAsync(string tablename, int cnumber);
        Task<bool> UpdateAsync(string tablename, int cnumber, Table newTable);
        Task<Table> GetOneTableRow(string tablename, int cnumber);
        //Task<Table> GetOneEmptyTableRow(string tablename);
        Task<bool> InsertAsync(string tablename, Table newTable);







    }
}
