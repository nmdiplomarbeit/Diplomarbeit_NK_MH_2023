using MySql.Data.MySqlClient;
using System.Data.Common;
using System.Data;
using System.Threading.Tasks;
using System;

namespace LohnverrechnerGastro.Models.DB
{
    public class RepositoryTablesDB : IRepositoryTablesDB
    {
        private string _connectionString = "Server=localhost;database=pvdiplomarbeit;user=root;password=";
        private DbConnection _conn;

        public Task<bool> AskEmailAsync(string email)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> AskNameAsync(string name)
        {
            throw new System.NotImplementedException();
        }

        public async Task ConnectAsync()
        {
            if (this._conn == null)
            {
                this._conn = new MySqlConnection(this._connectionString);
            }
            if (this._conn.State != ConnectionState.Open)
            {
                await this._conn.OpenAsync();
            }
        }

        public async Task DisconnectAsync()
        {
            if ((this._conn != null) && (this._conn.State == ConnectionState.Open))
            {
                await this._conn.CloseAsync();
            }
        }

        public async Task<decimal> GetSVSatzAsync(decimal einkommen)
        {
            decimal svSatz = 0m;
            if (this._conn?.State == ConnectionState.Open)
            {
                DbCommand cmd = this._conn.CreateCommand();
                cmd.CommandText = "select sv_satz from sv where bruttovon <= @brutto and bruttobis >= @brutto";     
                DbParameter paramID = cmd.CreateParameter();                        
                paramID.ParameterName = "brutto";
                paramID.DbType = DbType.String;
                paramID.Value = einkommen;
                cmd.Parameters.Add(paramID);
                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        svSatz = Convert.ToDecimal(reader["sv_satz"]);
                        return svSatz;
                    }
                }
            }
            return 0;

        }

        public async Task<decimal> GetEffTarifAsync(decimal lstbem)
        {
            decimal abzugInsg = 0m;
            if (this._conn?.State == ConnectionState.Open)
            {
                DbCommand cmd = this._conn.CreateCommand();
                cmd.CommandText = "select grenzsteuersatz, abgaben from effektiv_tarif_monat where lst_bemgrundlage_von <= @lstbem and lst_bemgrundlage_bis >= @lstbem";
                DbParameter paramID = cmd.CreateParameter();
                paramID.ParameterName = "lstbem";
                paramID.DbType = DbType.String;
                paramID.Value = lstbem;
                cmd.Parameters.Add(paramID);
                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        abzugInsg = (lstbem * (Convert.ToDecimal(reader["grenzsteuersatz"])/100)) - Convert.ToDecimal(reader["abgaben"]);
                        return abzugInsg;
                    }
                }
            }
            return 0;
        }

        public Task<bool> LoginAsync(string name, string password)
        {
            throw new System.NotImplementedException();
        }
    }
}
