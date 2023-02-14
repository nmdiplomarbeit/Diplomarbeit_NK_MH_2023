using MySql.Data.MySqlClient;
using System.Data.Common;
using System.Data;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using MySqlX.XDevAPI.Relational;
using Org.BouncyCastle.Asn1.X509;

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
                paramBrut.DbType = DbType.Decimal;
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
                paramID.DbType = DbType.Decimal;
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
                paramLstbem.DbType = DbType.Decimal;
                paramLstbem.Value = lstbem;
                cmd.Parameters.Add(paramLstbem);
                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        abzugInsg = (lstbem * (Convert.ToDecimal(reader["grenzsteuersatz"]) / 100)) - Convert.ToDecimal(reader["abgaben"]);
                        return abzugInsg;
                    }
                }
            }
            return 0;
        }

        public async Task<Dictionary<string, decimal>> GetDGAbgaben()
        {
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
                paramEink.DbType = DbType.Decimal;
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

        public async Task<decimal> GetBetrzugehArbeiter(int anzJahre)
        {
            decimal prozSatz = 0m;
            if (this._conn?.State == ConnectionState.Open)
            {
                DbCommand cmd = this._conn.CreateCommand();
                cmd.CommandText = "select prozentsatz from betrzugeh_arbeiter where von <= @anzJahre and bis >= @anzJahre";
                DbParameter paramJ = cmd.CreateParameter();
                paramJ.ParameterName = "anzJahre";
                paramJ.DbType = DbType.Int32;
                paramJ.Value = anzJahre;
                cmd.Parameters.Add(paramJ);
                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        prozSatz = Convert.ToDecimal(reader["prozentsatz"]);
                        return prozSatz;
                    }
                }
            }
            return 0;
        }

        public async Task<decimal> GetBetrzugehAngestellter(int anzJahre)
        {
            decimal prozSatz = 0m;
            if (this._conn?.State == ConnectionState.Open)
            {
                DbCommand cmd = this._conn.CreateCommand();
                cmd.CommandText = "select prozentsatz from betrzugeh_angestellter where von <= @anzJahre and bis >= @anzJahre";
                DbParameter paramJ = cmd.CreateParameter();
                paramJ.ParameterName = "anzJahre";
                paramJ.DbType = DbType.Int32;
                paramJ.Value = anzJahre;
                cmd.Parameters.Add(paramJ);
                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        prozSatz = Convert.ToDecimal(reader["prozentsatz"]);
                        return prozSatz;
                    }
                }
            }
            return 0;
        }

        public async Task<decimal> GetProzBundesland(string bundesland)
        {
            decimal prozSatz = 0m;
            if (this._conn?.State == ConnectionState.Open)
            {
                DbCommand cmd = this._conn.CreateCommand();
                cmd.CommandText = "select prozentsatz from bundesland_dz where bundesland = @bundesland";
                DbParameter paramB = cmd.CreateParameter();
                paramB.ParameterName = "bundesland";
                paramB.DbType = DbType.String;
                paramB.Value = bundesland;
                cmd.Parameters.Add(paramB);
                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        prozSatz = Convert.ToDecimal(reader["prozentsatz"]);
                        return prozSatz;
                    }
                }
            }
            return 0;
        }


        public async Task<Table> GetOneTableRow(string tablename, int cnumber)
        {
            Table table;
            if (this._conn?.State == ConnectionState.Open)
            {
                DbCommand cmd = this._conn.CreateCommand();
                cmd.CommandText = "select * from " + tablename + " where cnumber = @cnumber";
                DbParameter paramCnumber = cmd.CreateParameter();
                paramCnumber.ParameterName = "cnumber";
                paramCnumber.DbType = DbType.Int32;
                paramCnumber.Value = cnumber;
                cmd.Parameters.Add(paramCnumber);
                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        if (tablename == "sv" || tablename == "sv_sz")
                        {
                            table = new Table()                       // new Table() macht immer nur einen neuen Eintrag in der Tabelle, dadruch relativ unübersichtlich
                            {
                                TableName = tablename,
                                Cnumber = Convert.ToInt32(reader["cnumber"]),
                                Column1 = Convert.ToDecimal(reader["bruttovon"]),
                                Column2 = Convert.ToDecimal(reader["bruttobis"]),
                                Column3 = Convert.ToDecimal(reader["sv_satz"]),

                            };
                            return table;
                        }

                        if (tablename == "dg_abgaben" || tablename == "dg_abgaben_sz")
                        {
                            table = new Table()
                            {
                                TableName = tablename,
                                Cnumber = Convert.ToInt32(reader["cnumber"]),
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

                            };
                            return table;
                        }
                        if (tablename == "grenzen_sv")
                        {
                            table = new Table()
                            {
                                TableName = tablename,
                                Cnumber = Convert.ToInt32(reader["cnumber"]),
                                Column1 = Convert.ToDecimal(reader["hbgl"]),
                                Column2 = Convert.ToDecimal(reader["gfg"]),
                                Column1Name = "hbgl",
                                Column2Name = "gfg",
                            };
                            return table;
                        }
                        if (tablename == "grenzen_sv_sz")
                        {
                            table = new Table()
                            {
                                TableName = tablename,
                                Cnumber = Convert.ToInt32(reader["cnumber"]),
                                Column1 = Convert.ToDecimal(reader["hbgl_sz"]),
                                Column2 = Convert.ToDecimal(reader["gfg"]),
                                Column1Name = "hbgl_sz",
                                Column2Name = "gfg",
                            };
                            return table;
                        }
                        if (tablename == "effektiv_tarif_monat")
                        {
                            table = new Table()
                            {
                                TableName = tablename,
                                Cnumber = Convert.ToInt32(reader["cnumber"]),
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
                            }; 
                            return table;
                        }
                        if (tablename == "sz_steuergrenzen")
                        {
                            table = new Table()
                            {
                                TableName = tablename,
                                Cnumber = Convert.ToInt32(reader["cnumber"]),
                                Column1 = Convert.ToDecimal(reader["von"]),
                                Column2 = Convert.ToDecimal(reader["bis"]),
                                Column3 = Convert.ToDecimal(reader["prozent"]),
                                Column1Name = "von",
                                Column2Name = "bis",
                                Column3Name = "prozent",
                            };
                            return table;
                        }
                        if (tablename == "freibetraege_lst")
                        {
                            table = new Table()
                            {
                                TableName = tablename,
                                Cnumber = Convert.ToInt32(reader["cnumber"]),
                                Column1 = Convert.ToDecimal(reader["§68/2"]),
                                Column2 = Convert.ToDecimal(reader["§68/1"]),
                                Column1Name = "§68/2",
                                Column2Name = "§68/1",
                            };
                            return table;
                        }

                        if (tablename == "betrzugeh_angestellter" || tablename == "betrzugeh_arbeiter")
                        {
                            table = new Table()
                            {
                                TableName = tablename,
                                Cnumber = Convert.ToInt32(reader["cnumber"]),
                                Column1 = Convert.ToInt32(reader["von"]),
                                Column2 = Convert.ToInt32(reader["bis"]),
                                Column3 = Convert.ToInt32(reader["prozentsatz"]),
                                Column1Name = "von",
                                Column2Name = "bis",
                                Column3Name = "prozentsatz",

                            };
                            return table;
                        }
                        if (tablename == "bundesland_dz")
                        {
                            table = new Table()
                            {
                                TableName = tablename,
                                Cnumber = Convert.ToInt32(reader["cnumber"]),
                                Column1s = Convert.ToString(reader["bundesland"]),
                                Column2 = Convert.ToInt32(reader["prozent"]),
                                Column1Name = "bundesland",
                                Column2Name = "prozent",

                            };
                            return table;
                        }


                    }
                }
            }
            return null;

        }

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
                            table.Add(new Table()                       // new Table() macht immer nur einen neuen Eintrag in der Tabelle, dadruch relativ unübersichtlich
                            {
                                TableName = tablename,
                                Cnumber = Convert.ToInt32(reader["cnumber"]),
                                Column1 = Convert.ToDecimal(reader["bruttovon"]),
                                Column2 = Convert.ToDecimal(reader["bruttobis"]),
                                Column3 = Convert.ToDecimal(reader["sv_satz"]),
                                
                            });
                        }

                        if (tablename == "dg_abgaben" || tablename == "dg_abgaben_sz")
                        {
                            table.Add(new Table()
                            {
                                TableName = tablename,
                                Cnumber = Convert.ToInt32(reader["cnumber"]),
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
                                TableName = tablename,
                                Cnumber = Convert.ToInt32(reader["cnumber"]),
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
                                TableName = tablename,
                                Cnumber = Convert.ToInt32(reader["cnumber"]),
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
                                TableName = tablename,
                                Cnumber = Convert.ToInt32(reader["cnumber"]),
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
                                TableName = tablename,
                                Cnumber = Convert.ToInt32(reader["cnumber"]),
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
                                TableName = tablename,
                                Cnumber = Convert.ToInt32(reader["cnumber"]),
                                Column1 = Convert.ToDecimal(reader["§68/2"]),
                                Column2 = Convert.ToDecimal(reader["§68/1"]),
                                Column1Name = "§68/2",
                                Column2Name = "§68/1",
                            });
                        }

                        if (tablename == "betrzugeh_angestellter" || tablename == "betrzugeh_arbeiter")
                        {
                            table.Add(new Table()
                            {
                                TableName = tablename,
                                Cnumber = Convert.ToInt32(reader["cnumber"]),
                                Column1 = Convert.ToInt32(reader["von"]),
                                Column2 = Convert.ToInt32(reader["bis"]),
                                Column3 = Convert.ToInt32(reader["prozentsatz"]),
                                Column1Name = "von",
                                Column2Name = "bis",
                                Column3Name = "prozentsatz",

                            });
                        }
                        if (tablename == "bundesland_dz")
                        {
                            table.Add(new Table()
                            {
                                TableName = tablename,
                                Cnumber = Convert.ToInt32(reader["cnumber"]),
                                Column1s = Convert.ToString(reader["bundesland"]),
                                Column2 = Convert.ToDecimal(reader["prozent"]),
                                Column1Name = "bundesland",
                                Column2Name = "prozent",

                            });
                        }
                    }
                }
            }
            return table;
        }

        public async Task<bool> DeleteAsync(string tablename, int cnumber)
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

        public async Task<bool> UpdateAsync(string tablename, int cnumber, Table newTable)
        {
            if (this._conn?.State == ConnectionState.Open)
            {
                string param1Name = "";
                string param2Name = "";
                string param3Name = "";
                string param4Name = "";
                string param5Name = "";

                DbCommand cmd = this._conn.CreateCommand();
                if (tablename == "sv" || tablename == "sv_sz")
                {
                    cmd.CommandText = "update " + tablename + " set bruttovon = @bruttovon, bruttobis = @bruttobis, " +
                    "sv_satz = @sv_satz where cnumber = @cnumber;";
                    param1Name = "bruttovon"; param2Name = "bruttobis"; param3Name = "sv_satz";
                }
                if (tablename == "dg_abgaben" || tablename == "dg_abgaben_sz")
                {
                    cmd.CommandText = "update " + tablename + " set sv_dg = @sv_dg, bmv = @bmv, " +
                    "db = @db, dz = @dz, kommst = @kommst where cnumber = @cnumber;";
                    param1Name = "sv_dg"; param2Name = "bmv"; param3Name = "db"; param4Name = "dz"; param5Name = "kommst";

                }
                if (tablename == "grenzen_sv")
                {
                    cmd.CommandText = "update " + tablename + " set hbgl = @hbgl, gfg = @gfg, " +
                    "where cnumber = @cnumber;";
                    param1Name = "hbgl"; param2Name = "gfg";
                }
                if (tablename == "grenzen_sv_sz")
                {
                    cmd.CommandText = "update " + tablename + " set hbgl_sz = @hbgl_sz, gfg = @gfg, " +
                    "where cnumber = @cnumber;";
                    param1Name = "hbgl_sz"; param2Name = "gfg";
                }
                if (tablename == "sz_steuergrenzen")
                {
                    cmd.CommandText = "update " + tablename + " set bis = @bis, prozent = @prozent, " +
                    "von = @von, where cnumber = @cnumber;";
                    param1Name = "bis"; param2Name = "prozent"; param3Name = "von";
                }
                if (tablename == "sv" || tablename == "sv_sz")
                {
                    cmd.CommandText = "update " + tablename + " set bruttovon = @bruttovon, bruttobis = @bruttobis, " +
                    "sv_satz = @sv_satz, where cnumber = @cnumber;";
                }


                DbParameter param1 = cmd.CreateParameter();
                param1.ParameterName = param1Name;
                param1.DbType = DbType.Decimal;
                param1.Value = newTable.Column1;

                DbParameter param2 = cmd.CreateParameter();
                param2.ParameterName = param2Name;
                param2.DbType = DbType.Decimal;
                param2.Value = newTable.Column2;

                DbParameter param3 = cmd.CreateParameter();
                param3.ParameterName = param3Name;
                param3.DbType = DbType.Decimal;
                param3.Value = newTable.Column3;

                DbParameter param4 = cmd.CreateParameter();
                param4.ParameterName = param4Name;
                param4.DbType = DbType.Decimal;
                param4.Value = newTable.Column4;

                DbParameter param5 = cmd.CreateParameter();
                param5.ParameterName = param5Name;
                param5.DbType = DbType.Decimal;
                param5.Value = newTable.Column5;

                //DbParameter paramBD = cmd.CreateParameter();
                //paramBD.ParameterName = "birthdate";
                //paramBD.DbType = DbType.Date;
                //paramBD.Value = newKundenData.Birthdate;

                //DbParameter paramGender = cmd.CreateParameter();
                //paramGender.ParameterName = "geschlecht";
                //paramGender.DbType = DbType.Int32;
                //paramGender.Value = newKundenData.Geschlecht;

                DbParameter paramCnumber = cmd.CreateParameter();
                paramCnumber.ParameterName = "cnumber";
                paramCnumber.DbType = DbType.Int32;
                paramCnumber.Value = cnumber;


                cmd.Parameters.Add(param1);
                cmd.Parameters.Add(param2);
                cmd.Parameters.Add(param3);
                cmd.Parameters.Add(param4);
                cmd.Parameters.Add(param5);
                cmd.Parameters.Add(paramCnumber);
                //cmd.Parameters.Add(paramBD);
                //cmd.Parameters.Add(paramGender);
                //cmd.Parameters.Add(paramID);

                return await cmd.ExecuteNonQueryAsync() == 1;
            }
            return false;
        }

        public async Task<bool> InsertAsync(string tablename, Table newTable)
        {

            if (this._conn?.State == ConnectionState.Open)
            {
                DbCommand cmdInsert = this._conn.CreateCommand();
                cmdInsert.CommandText = "insert into " + tablename + " values(null, @bruttovon, @bruttobis, @sv_satz)";

                DbParameter param1 = cmdInsert.CreateParameter();
                param1.ParameterName = "bruttovon";
                param1.DbType = DbType.Decimal;
                param1.Value = newTable.Column1;

                DbParameter param2 = cmdInsert.CreateParameter();
                param2.ParameterName = "bruttobis";
                param2.DbType = DbType.Decimal;
                param2.Value = newTable.Column2;

                DbParameter param3 = cmdInsert.CreateParameter();
                param3.ParameterName = "sv_satz";
                param3.DbType = DbType.Decimal;
                param3.Value = newTable.Column3;

                cmdInsert.Parameters.Add(param1);
                cmdInsert.Parameters.Add(param2);
                cmdInsert.Parameters.Add(param3);

                return await cmdInsert.ExecuteNonQueryAsync() == 1;
            }
            return false;
        }
    }
}
