using MySql.Data.MySqlClient;
using System.Data.Common;
using System.Data;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using MySqlX.XDevAPI.Relational;

namespace LohnverrechnerGastro.Models.DB
{
    public class RepositoryTablesDB : IRepositoryTablesDB
    {
        private string _connectionString = "Server=localhost;database=pvdiplomarbeit;user=root;password=";
        private DbConnection _conn;

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
                DbParameter paramBrut = cmd.CreateParameter();
                paramBrut.ParameterName = "brutto";
                paramBrut.DbType = DbType.String;
                paramBrut.Value = einkommen;
                cmd.Parameters.Add(paramBrut);
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

        public async Task<decimal> GetSVSatzSZAsync(decimal einkommen)
        {
            decimal svSatz = 0m;
            if (this._conn?.State == ConnectionState.Open)
            {
                DbCommand cmd = this._conn.CreateCommand();
                cmd.CommandText = "select sv_satz from sv_sz where bruttovon <= @brutto and bruttobis >= @brutto";
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
                DbParameter paramLstbem = cmd.CreateParameter();
                paramLstbem.ParameterName = "lstbem";
                paramLstbem.DbType = DbType.String;
                paramLstbem.Value = lstbem;
                cmd.Parameters.Add(paramLstbem);
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

        public async Task<Dictionary<string, decimal>> GetDGAbgaben() {    
            Dictionary<string, decimal> dgabgaben = new Dictionary<string, decimal>();
            if (this._conn?.State == ConnectionState.Open)
            {
                DbCommand cmd = this._conn.CreateCommand();
                cmd.CommandText = "select sv_dg, bmv, db, dz, kommst from dg_abgaben where cnumber = 1";        // cnumber = Columnnumber -> ist zum auslesen der Zeile
                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        dgabgaben.Add("sv_dg", Convert.ToDecimal(reader["sv_dg"]));
                        dgabgaben.Add("bmv", Convert.ToDecimal(reader["bmv"]));
                        dgabgaben.Add("db", Convert.ToDecimal(reader["db"]));
                        dgabgaben.Add("dz", Convert.ToDecimal(reader["dz"]));
                        dgabgaben.Add("kommst", Convert.ToDecimal(reader["kommst"]));
                        return dgabgaben;
                    }
                }
            }
            return null;
        }

        public async Task<Dictionary<string, decimal>> GetDGAbgabenSZ()
        {
            Dictionary<string, decimal> dgabgabensv = new Dictionary<string, decimal>();
            if (this._conn?.State == ConnectionState.Open)
            {
                DbCommand cmd = this._conn.CreateCommand();
                cmd.CommandText = "select sv_dg, bmv, db, dz, kommst from dg_abgaben_sz where cnumber = 1";
                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        dgabgabensv.Add("sv_dg", Convert.ToDecimal(reader["sv_dg"]));
                        dgabgabensv.Add("bmv", Convert.ToDecimal(reader["bmv"]));
                        dgabgabensv.Add("db", Convert.ToDecimal(reader["db"]));
                        dgabgabensv.Add("dz", Convert.ToDecimal(reader["dz"]));
                        dgabgabensv.Add("kommst", Convert.ToDecimal(reader["kommst"]));
                        return dgabgabensv;
                    }
                }
            }
            return null;
        }

        public async Task<Dictionary<string, decimal>> GetGrenzenSV()
        {
            Dictionary<string, decimal> grenzensv = new Dictionary<string, decimal>();
            if (this._conn?.State == ConnectionState.Open)
            {
                DbCommand cmd = this._conn.CreateCommand();
                cmd.CommandText = "select hbgl, gfg from grenzen_sv where cnumber = 1";
                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        grenzensv.Add("hbgl", Convert.ToDecimal(reader["hbgl"]));
                        grenzensv.Add("gfg", Convert.ToDecimal(reader["gfg"]));
                        return grenzensv;
                    }
                }
            }
            return null;
        }

        public async Task<Dictionary<string, decimal>> GetGrenzenSVSZ()
        {
            Dictionary<string, decimal> grenzensvsz = new Dictionary<string, decimal>();
            if (this._conn?.State == ConnectionState.Open)
            {
                DbCommand cmd = this._conn.CreateCommand();
                cmd.CommandText = "select hbgl_sz, gfg from grenzen_sv_sz where cnumber = 1";
                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        grenzensvsz.Add("hbgl_sz", Convert.ToDecimal(reader["hbgl_sz"]));
                        grenzensvsz.Add("gfg", Convert.ToDecimal(reader["gfg"]));
                        return grenzensvsz;
                    }
                }
            }
            return null;
        }

        public async Task<decimal> GetSZSteuergrenzen(decimal einkommen)
        {
            decimal prozent = 0m;
            if (this._conn?.State == ConnectionState.Open)
            {
                DbCommand cmd = this._conn.CreateCommand();
                cmd.CommandText = "select prozent from sz_steuergrenzen where von <= @einkommen and bis >= @einkommen";
                DbParameter paramEink = cmd.CreateParameter();
                paramEink.ParameterName = "einkommen";
                paramEink.DbType = DbType.String;
                paramEink.Value = einkommen;
                cmd.Parameters.Add(paramEink);
                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        prozent = Convert.ToDecimal(reader["prozent"]);
                        return prozent;
                    }
                }
            }
            return 0;
        }








        public async Task<List<Table>> GetAllTablesAsync(string tablename)
        {

            List<Table> sv = new List<Table>();

            if (this._conn?.State == ConnectionState.Open)
            {

                DbCommand cmdTables = this._conn.CreateCommand();
                cmdTables.CommandText = "select * from @tablename";
                DbParameter paramtable = cmdTables.CreateParameter();
                paramtable.ParameterName = "tablename";
                paramtable.DbType = DbType.String;
                paramtable.Value = tablename;
                cmdTables.Parameters.Add(paramtable);

                using (DbDataReader reader = await cmdTables.ExecuteReaderAsync())
                {

                    while (await reader.ReadAsync())
                    {
                        sv.Add(new Table()
                        {
                            Column1 = Convert.ToDecimal(reader["bruttovon"]),
                            Column2 = Convert.ToDecimal(reader["bruttobis"]),
                            Column3 = Convert.ToDecimal(reader["sv_satz"]),
                        });
                    }
                }
            }
            return sv;
        }
    }
}
