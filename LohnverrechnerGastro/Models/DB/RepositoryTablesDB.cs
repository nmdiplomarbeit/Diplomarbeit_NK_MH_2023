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








        //public async Task<List<Table>> GetAllTablesAsync(string tablename)
        //{

        //    List<Table> sv = new List<Table>();

        //    if (this._conn?.State == ConnectionState.Open)
        //    {

        //        DbCommand cmdTables = this._conn.CreateCommand();
        //        cmdTables.CommandText = "select * from @tablename";
        //        DbParameter paramtable = cmdTables.CreateParameter();
        //        paramtable.ParameterName = "tablename";
        //        paramtable.DbType = DbType.String;
        //        paramtable.Value = tablename;
        //        cmdTables.Parameters.Add(paramtable);
        //        cmdTables.ExecuteNonQuery();

        //        using (DbDataReader reader = await cmdTables.ExecuteReaderAsync())
        //        {

        //            while (await reader.ReadAsync())
        //            {
        //                sv.Add(new Table()
        //                {
        //                    Column1 = Convert.ToDecimal(reader["bruttovon"]),
        //                    Column2 = Convert.ToDecimal(reader["bruttobis"]),
        //                    Column3 = Convert.ToDecimal(reader["sv_satz"]),
        //                });
        //            }
        //        }
        //    }
        //    return sv;
        //}

        public async Task<List<Table>> GetAllTablesAsync(string tablename)
        {

            List<Table> table = new List<Table>();

            if (this._conn?.State == ConnectionState.Open)
            {

                DbCommand cmdTables = this._conn.CreateCommand();
                cmdTables.CommandText = "select * from " + tablename;       // zuerst wieder mit @tablename probieren -> funktioniert nicht

                using (DbDataReader reader = await cmdTables.ExecuteReaderAsync())
                {

                    while (await reader.ReadAsync())
                    {
                        
                        if (tablename == "sv" || tablename == "sv_sz")
                        {
                            table.Add(new Table()
                            {
                                Column1 = Convert.ToDecimal(reader["bruttovon"]),
                                Column2 = Convert.ToDecimal(reader["bruttobis"]),
                                Column3 = Convert.ToDecimal(reader["sv_satz"]),
                                Column1Name = "bruttovon",
                                Column2Name = "bruttobis",
                                Column3Name = "sv_satz",
                            });
                        }
                        if (tablename == "dg_abgaben" || tablename == "dg_abgaben_sz")
                        {
                            table.Add(new Table()
                            {
                                Column1 = Convert.ToDecimal(reader["sv_dg"]),
                                Column2 = Convert.ToDecimal(reader["bmv"]),
                                Column3 = Convert.ToDecimal(reader["db"]),
                                Column4 = Convert.ToDecimal(reader["dz"]),
                                Column5 = Convert.ToDecimal(reader["kommst"]),
                                Column1Name = "sv_dg",
                                Column2Name = "bmv",
                                Column3Name = "db",
                                Column4Name = "dz",
                                Column5Name = "kommst",

                            });
                        }
                        if (tablename == "grenzen_sv")
                        {
                            table.Add(new Table()
                            {
                                Column1 = Convert.ToDecimal(reader["hbgl"]),
                                Column2 = Convert.ToDecimal(reader["gfg"]),
                                Column1Name = "hbgl",
                                Column2Name = "gfg",
                            });
                        }
                        if (tablename == "grenzen_sv_sz")
                        {
                            table.Add(new Table()
                            {
                                Column1 = Convert.ToDecimal(reader["hbgl_sz"]),
                                Column2 = Convert.ToDecimal(reader["gfg"]),
                                Column1Name = "hbgl_sz",
                                Column2Name = "gfg",
                            });
                        }
                        if (tablename == "effektiv_tarif_monat")
                        {
                            table.Add(new Table()
                            {
                                Column1 = Convert.ToDecimal(reader["lst_bemgrundlage_von"]),
                                Column2 = Convert.ToDecimal(reader["lst_bemgrundlage_bis"]),
                                Column3 = Convert.ToDecimal(reader["grenzsteuersatz"]),
                                Column4 = Convert.ToDecimal(reader["anz_kinder"]),
                                Column5 = Convert.ToDecimal(reader["abgaben"]),
                                Column1Name = "lst_bemgrundlage_von",
                                Column2Name = "lst_bemgrundlage_bis",
                                Column3Name = "grenzsteuersatz",
                                Column4Name = "anz_kinder",
                                Column5Name = "abzug",
                            });
                        }
                        if (tablename == "sz_steuergrenzen")
                        {
                            table.Add(new Table()
                            {
                                Column1 = Convert.ToDecimal(reader["von"]),
                                Column2 = Convert.ToDecimal(reader["bis"]),
                                Column3 = Convert.ToDecimal(reader["prozent"]),
                                Column1Name = "von",
                                Column2Name = "bis",
                                Column3Name = "prozent",
                            });
                        }
                        if (tablename == "freibetraege_lst")
                        {
                            table.Add(new Table()
                            {
                                Column1 = Convert.ToDecimal(reader["§68/2"]),
                                Column2 = Convert.ToDecimal(reader["§68/1"]),
                                Column1Name = "§68/2",
                                Column2Name = "§68/1",
                            });
                        }
                    }
                }
            }
            return table;
        }

        public async Task<bool> DeleteAsync(int cnumber, string tablename)
        {
            if (this._conn?.State == ConnectionState.Open)
            {
                DbCommand cmdDelete = this._conn.CreateCommand();
                cmdDelete.CommandText = "delete from " + tablename + " where cnumber = @cnumber";
                DbParameter paramcn = cmdDelete.CreateParameter();
                paramcn.ParameterName = "cnumber";
                paramcn.DbType = DbType.Int32;
                paramcn.Value = cnumber;

                cmdDelete.Parameters.Add(paramcn);

                return await cmdDelete.ExecuteNonQueryAsync() == 1;
            }

            return false;
        }
    }
}
