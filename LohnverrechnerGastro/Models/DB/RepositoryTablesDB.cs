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

        public async Task<decimal> GetEffTarifAsync(decimal lstbem, int anzkinder)
        {
            decimal abzugInsg = 0m;
            string kind = "nullkind";
            if (this._conn?.State == ConnectionState.Open)
            {
                DbCommand cmd = this._conn.CreateCommand();
                if(anzkinder == 0)
                {
                    kind = "nullkind";
                    cmd.CommandText = "select grenzsteuersatz, " + kind + " from effektiv_tarif_monat where lst_bemgrundlage_von <= @lstbem and lst_bemgrundlage_bis >= @lstbem";
                }
                if (anzkinder == 1)
                {
                    kind = "einkind";
                    cmd.CommandText = "select grenzsteuersatz, " + kind + " from effektiv_tarif_monat where lst_bemgrundlage_von <= @lstbem and lst_bemgrundlage_bis >= @lstbem";
                }
                if (anzkinder == 2)
                {
                    kind = "zweikind";
                    cmd.CommandText = "select grenzsteuersatz, " + kind + " from effektiv_tarif_monat where lst_bemgrundlage_von <= @lstbem and lst_bemgrundlage_bis >= @lstbem";
                }
                if (anzkinder == 3)
                {
                    kind = "dreikind";
                    cmd.CommandText = "select grenzsteuersatz, " + kind + " from effektiv_tarif_monat where lst_bemgrundlage_von <= @lstbem and lst_bemgrundlage_bis >= @lstbem";
                }
                if (anzkinder == 4)
                {
                    kind = "vierkind";
                    cmd.CommandText = "select grenzsteuersatz, " + kind + " from effektiv_tarif_monat where lst_bemgrundlage_von <= @lstbem and lst_bemgrundlage_bis >= @lstbem";
                }
                if (anzkinder == 5)
                {
                    kind = "fuenfkind";
                    cmd.CommandText = "select grenzsteuersatz, " + kind + " from effektiv_tarif_monat where lst_bemgrundlage_von <= @lstbem and lst_bemgrundlage_bis >= @lstbem";
                }
                DbParameter paramLstbem = cmd.CreateParameter();
                paramLstbem.ParameterName = "lstbem";
                paramLstbem.DbType = DbType.Decimal;
                paramLstbem.Value = lstbem;
                cmd.Parameters.Add(paramLstbem);
                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        abzugInsg = (lstbem * (Convert.ToDecimal(reader["grenzsteuersatz"]) / 100)) - Convert.ToDecimal(reader[kind]);
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

        public async Task<decimal> GetLohngruppen(string lohngruppe)
        {
            decimal kv = 0m;
            if (this._conn?.State == ConnectionState.Open)
            {
                DbCommand cmd = this._conn.CreateCommand();
                cmd.CommandText = "select kv from lohngruppen where lohngruppe = @lohngruppe";
                DbParameter paramB = cmd.CreateParameter();
                paramB.ParameterName = "lohngruppe";
                paramB.DbType = DbType.String;
                paramB.Value = lohngruppe;
                cmd.Parameters.Add(paramB);
                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        kv = Convert.ToDecimal(reader["kv"]);
                        return kv;
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
                                Column1Name = "bruttovon",
                                Column2Name = "bruttobis",
                                Column3Name = "sv_satz"
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
                                Column4 = Convert.ToDecimal(reader["nullkind"]),
                                Column5 = Convert.ToDecimal(reader["einkind"]),
                                Column6 = Convert.ToDecimal(reader["zweikind"]),
                                Column7 = Convert.ToDecimal(reader["dreikind"]),
                                Column8 = Convert.ToDecimal(reader["vierkind"]),
                                Column9 = Convert.ToDecimal(reader["fuenfkind"]),
                                Column1Name = "lst_bemgrundlage_von",
                                Column2Name = "lst_bemgrundlage_bis",
                                Column3Name = "grenzsteuersatz",
                                Column4Name = "nullkind",
                                Column5Name = "einkind",
                                Column6Name = "zweikind",
                                Column7Name = "dreikind",
                                Column8Name = "vierkind",
                                Column9Name = "fuenfkind",
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
                                Column1 = Convert.ToDecimal(reader["von"]),
                                Column2 = Convert.ToDecimal(reader["bis"]),
                                Column3 = Convert.ToDecimal(reader["prozentsatz"]),
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
                                Column2 = Convert.ToDecimal(reader["prozent"]),
                                Column2Name = "prozent",

                            };
                            return table;
                        }


                    }
                }
            }
            return null;

        }

        //public async Task<Table> GetOneEmptyTableRow(string tablename)
        //{
        //    Table table;
        //    int cnumber = 0;
        //    if (this._conn?.State == ConnectionState.Open)
        //    {
        //        DbCommand cmdhigh = this._conn.CreateCommand();
        //        cmdhigh.CommandText = "select max(cnumber) from " + tablename;
        //        using (DbDataReader r = await cmdhigh.ExecuteReaderAsync())
        //        {
        //            while (await r.ReadAsync())
        //            {
        //                cnumber = Convert.ToInt32(r["max(cnumber)"]) + 1;
        //            }
        //        }



        //        DbCommand cmd = this._conn.CreateCommand();
        //        cmd.CommandText = "select * from " + tablename + " where cnumber = @cnumber";
        //        DbParameter paramCnumber = cmd.CreateParameter();
        //        paramCnumber.ParameterName = "cnumber";
        //        paramCnumber.DbType = DbType.Int32;
        //        paramCnumber.Value = cnumber;
        //        cmd.Parameters.Add(paramCnumber);
        //        using (DbDataReader reader = await cmd.ExecuteReaderAsync())
        //        {
        //            while (await reader.ReadAsync())
        //            {
        //                if (tablename == "sv" || tablename == "sv_sz")
        //                {
        //                    table = new Table()                       // new Table() macht immer nur einen neuen Eintrag in der Tabelle, dadruch relativ unübersichtlich
        //                    {
        //                        TableName = tablename,
        //                        Cnumber = Convert.ToInt32(reader["cnumber"]),
        //                        Column1 = Convert.ToDecimal(reader["bruttovon"]),
        //                        Column2 = Convert.ToDecimal(reader["bruttobis"]),
        //                        Column3 = Convert.ToDecimal(reader["sv_satz"]),
        //                        Column1Name = "bruttovon",
        //                        Column2Name = "bruttobis",
        //                        Column3Name = "sv_satz"
        //                    };
        //                    return table;
        //                }

        //                if (tablename == "dg_abgaben" || tablename == "dg_abgaben_sz")
        //                {
        //                    table = new Table()
        //                    {
        //                        TableName = tablename,
        //                        Cnumber = Convert.ToInt32(reader["cnumber"]),
        //                        Column1 = Convert.ToDecimal(reader["sv_dg"]),
        //                        Column2 = Convert.ToDecimal(reader["bmv"]),
        //                        Column3 = Convert.ToDecimal(reader["db"]),
        //                        Column4 = Convert.ToDecimal(reader["dz"]),
        //                        Column5 = Convert.ToDecimal(reader["kommst"]),
        //                        Column1Name = "sv_dg",
        //                        Column2Name = "bmv",
        //                        Column3Name = "db",
        //                        Column4Name = "dz",
        //                        Column5Name = "kommst",

        //                    };
        //                    return table;
        //                }
        //                if (tablename == "grenzen_sv")
        //                {
        //                    table = new Table()
        //                    {
        //                        TableName = tablename,
        //                        Cnumber = Convert.ToInt32(reader["cnumber"]),
        //                        Column1 = Convert.ToDecimal(reader["hbgl"]),
        //                        Column2 = Convert.ToDecimal(reader["gfg"]),
        //                        Column1Name = "hbgl",
        //                        Column2Name = "gfg",
        //                    };
        //                    return table;
        //                }
        //                if (tablename == "grenzen_sv_sz")
        //                {
        //                    table = new Table()
        //                    {
        //                        TableName = tablename,
        //                        Cnumber = Convert.ToInt32(reader["cnumber"]),
        //                        Column1 = Convert.ToDecimal(reader["hbgl_sz"]),
        //                        Column2 = Convert.ToDecimal(reader["gfg"]),
        //                        Column1Name = "hbgl_sz",
        //                        Column2Name = "gfg",
        //                    };
        //                    return table;
        //                }
        //                if (tablename == "effektiv_tarif_monat")
        //                {
        //                    table = new Table()
        //                    {
        //                        TableName = tablename,
        //                        Cnumber = Convert.ToInt32(reader["cnumber"]),
        //                        Column1 = Convert.ToDecimal(reader["lst_bemgrundlage_von"]),
        //                        Column2 = Convert.ToDecimal(reader["lst_bemgrundlage_bis"]),
        //                        Column3 = Convert.ToDecimal(reader["grenzsteuersatz"]),
        //                        Column4 = Convert.ToDecimal(reader["anz_kinder"]),
        //                        Column5 = Convert.ToDecimal(reader["abgaben"]),
        //                        Column1Name = "lst_bemgrundlage_von",
        //                        Column2Name = "lst_bemgrundlage_bis",
        //                        Column3Name = "grenzsteuersatz",
        //                        Column4Name = "anz_kinder",
        //                        Column5Name = "abzug",
        //                    };
        //                    return table;
        //                }
        //                if (tablename == "sz_steuergrenzen")
        //                {
        //                    table = new Table()
        //                    {
        //                        TableName = tablename,
        //                        Cnumber = Convert.ToInt32(reader["cnumber"]),
        //                        Column1 = Convert.ToDecimal(reader["von"]),
        //                        Column2 = Convert.ToDecimal(reader["bis"]),
        //                        Column3 = Convert.ToDecimal(reader["prozent"]),
        //                        Column1Name = "von",
        //                        Column2Name = "bis",
        //                        Column3Name = "prozent",
        //                    };
        //                    return table;
        //                }
        //                if (tablename == "freibetraege_lst")
        //                {
        //                    table = new Table()
        //                    {
        //                        TableName = tablename,
        //                        Cnumber = Convert.ToInt32(reader["cnumber"]),
        //                        Column1 = Convert.ToDecimal(reader["§68/2"]),
        //                        Column2 = Convert.ToDecimal(reader["§68/1"]),
        //                        Column1Name = "§68/2",
        //                        Column2Name = "§68/1",
        //                    };
        //                    return table;
        //                }

        //                if (tablename == "betrzugeh_angestellter" || tablename == "betrzugeh_arbeiter")
        //                {
        //                    table = new Table()
        //                    {
        //                        TableName = tablename,
        //                        Cnumber = Convert.ToInt32(reader["cnumber"]),
        //                        Column1 = Convert.ToDecimal(reader["von"]),
        //                        Column2 = Convert.ToDecimal(reader["bis"]),
        //                        Column3 = Convert.ToDecimal(reader["prozentsatz"]),
        //                        Column1Name = "von",
        //                        Column2Name = "bis",
        //                        Column3Name = "prozentsatz",

        //                    };
        //                    return table;
        //                }
        //                if (tablename == "bundesland_dz")
        //                {
        //                    table = new Table()
        //                    {
        //                        TableName = tablename,
        //                        Cnumber = Convert.ToInt32(reader["cnumber"]),
        //                        Column2 = Convert.ToDecimal(reader["prozent"]),
        //                        Column2Name = "prozent",

        //                    };
        //                    return table;
        //                }


        //            }
        //        }
        //    }
        //    return null;

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
                            table.Add(new Table()                       // new Table() macht immer nur einen neuen Eintrag in der Tabelle, dadruch relativ unübersichtlich
                            {
                                TableName = tablename,
                                Cnumber = Convert.ToInt32(reader["cnumber"]),
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
                                Column4 = Convert.ToDecimal(reader["nullkind"]),
                                Column5 = Convert.ToDecimal(reader["einkind"]),
                                Column6 = Convert.ToDecimal(reader["zweikind"]),
                                Column7 = Convert.ToDecimal(reader["dreikind"]),
                                Column8 = Convert.ToDecimal(reader["vierkind"]),
                                Column9 = Convert.ToDecimal(reader["fuenfkind"]),
                                Column1Name = "lst_bemgrundlage_von",
                                Column2Name = "lst_bemgrundlage_bis",
                                Column3Name = "grenzsteuersatz",
                                Column4Name = "nullkind",
                                Column5Name = "einkind",
                                Column6Name = "zweikind",
                                Column7Name = "dreikind",
                                Column8Name = "vierkind",
                                Column9Name = "fuenfkind",
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
                                Column1 = Convert.ToDecimal(reader["von"]),
                                Column2 = Convert.ToDecimal(reader["bis"]),
                                Column3 = Convert.ToDecimal(reader["prozentsatz"]),
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
                string param6Name = "";
                string param7Name = "";
                string param8Name = "";
                string param9Name = "";

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
                    cmd.CommandText = "update " + tablename + " set hbgl = @hbgl, gfg = @gfg where cnumber = @cnumber;";
                    param1Name = "hbgl"; param2Name = "gfg";
                }
                if (tablename == "grenzen_sv_sz")
                {
                    cmd.CommandText = "update " + tablename + " set hbgl_sz = @hbgl_sz, gfg = @gfg " +
                    "where cnumber = @cnumber;";
                    param1Name = "hbgl_sz"; param2Name = "gfg";
                }
                if (tablename == "sz_steuergrenzen")
                {
                    cmd.CommandText = "update " + tablename + " set bis = @bis, prozent = @prozent, " +
                    "von = @von where cnumber = @cnumber;";
                    param1Name = "bis"; param2Name = "prozent"; param3Name = "von";
                }
                if (tablename == "freibetraege_lst")
                {
                    cmd.CommandText = "update " + tablename + " set §68/2 = @§68/2, §68/1  = @§68/1 where cnumber = @cnumber;";
                    param1Name = "§68/2"; param2Name = "§68/1";
                }
                if (tablename == "effektiv_tarif_monat")
                {
                    cmd.CommandText = "update " + tablename + " set lst_bemgrundlage_von = @lst_bemgrundlage_von, lst_bemgrundlage_bis = @lst_bemgrundlage_bis, " +
                    "grenzsteuersatz = @grenzsteuersatz, nullkind = @nullkind, einkind = @einkind, zweikind = @zweikind, dreikind = @dreikind, vierkind = @vierkind, fuenfkind = @fuenfkind where cnumber = @cnumber;";
                    param1Name = "lst_bemgrundlage_von"; param2Name = "lst_bemgrundlage_bis"; param3Name = "grenzsteuersatz"; param4Name = "nullkind"; param5Name = "einkind"; param6Name = "zweikind"; param7Name = "dreikind"; param8Name = "vierkind"; param9Name = "fuenfkind";
                }
                if (tablename == "betrzugeh_arbeiter" || tablename == "betrzugeh_angestellter")
                {
                    cmd.CommandText = "update " + tablename + " set von = @von, bis = @bis, " +
                    "prozentsatz = @prozentsatz where cnumber = @cnumber;";
                    param1Name = "von"; param2Name = "bis"; param3Name = "prozentsatz";
                }
                if(tablename == "bundesland_dz")
                {
                    cmd.CommandText = "update " + tablename + " set prozent = @prozent where cnumber = @cnumber;";
                    param1Name = "prozent";
                }

                DbParameter param1 = cmd.CreateParameter();
                param1.ParameterName = param1Name;
                if (tablename != "bundesland_dz")
                {
                    param1.DbType = DbType.Decimal;
                }
                else
                {
                    param1.DbType = DbType.String;
                }
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

                DbParameter param6 = cmd.CreateParameter();
                param6.ParameterName = param6Name;
                param6.DbType = DbType.Decimal;
                param6.Value = newTable.Column6;

                DbParameter param7 = cmd.CreateParameter();
                param7.ParameterName = param7Name;
                param7.DbType = DbType.Decimal;
                param7.Value = newTable.Column7;

                DbParameter param8 = cmd.CreateParameter();
                param8.ParameterName = param8Name;
                param8.DbType = DbType.Decimal;
                param8.Value = newTable.Column8;

                DbParameter param9 = cmd.CreateParameter();
                param9.ParameterName = param9Name;
                param9.DbType = DbType.Decimal;
                param9.Value = newTable.Column9;

                DbParameter paramCnumber = cmd.CreateParameter();
                paramCnumber.ParameterName = "cnumber";
                paramCnumber.DbType = DbType.Int32;
                paramCnumber.Value = cnumber;

                cmd.Parameters.Add(param1);
                cmd.Parameters.Add(param2);
                cmd.Parameters.Add(param3);
                cmd.Parameters.Add(param4);
                cmd.Parameters.Add(param5); 
                cmd.Parameters.Add(param6);
                cmd.Parameters.Add(param7);
                cmd.Parameters.Add(param8);
                cmd.Parameters.Add(param9);
                cmd.Parameters.Add(paramCnumber);

                return await cmd.ExecuteNonQueryAsync() == 1;
            }
            return false;
        }

        public async Task<bool> InsertAsync(string tablename, Table newTable)
        {

            //if (this._conn?.State == ConnectionState.Open)
            //{
            //    DbCommand cmdInsert = this._conn.CreateCommand();
            //    cmdInsert.CommandText = "insert into " + tablename + " values(null, @bruttovon, @bruttobis, @sv_satz)";

            //    DbParameter param1 = cmdInsert.CreateParameter();
            //    param1.ParameterName = "bruttovon";
            //    param1.DbType = DbType.Decimal;
            //    param1.Value = newTable.Column1;

            //    DbParameter param2 = cmdInsert.CreateParameter();
            //    param2.ParameterName = "bruttobis";
            //    param2.DbType = DbType.Decimal;
            //    param2.Value = newTable.Column2;

            //    DbParameter param3 = cmdInsert.CreateParameter();
            //    param3.ParameterName = "sv_satz";
            //    param3.DbType = DbType.Decimal;
            //    param3.Value = newTable.Column3;

            //    cmdInsert.Parameters.Add(param1);
            //    cmdInsert.Parameters.Add(param2);
            //    cmdInsert.Parameters.Add(param3);

            //    return await cmdInsert.ExecuteNonQueryAsync() == 1;
            //}
            //return false;

            if (this._conn?.State == ConnectionState.Open)
            {
                string param1Name = "";
                string param2Name = "";
                string param3Name = "";
                string param4Name = "";
                string param5Name = "";
                string param6Name = "";
                string param7Name = "";
                string param8Name = "";
                string param9Name = "";

                DbCommand cmd = this._conn.CreateCommand();
                if (tablename == "sv" || tablename == "sv_sz")
                {
                    cmd.CommandText = "insert into " + tablename + " values(null, @bruttovon, @bruttobis, @sv_satz)";
                    param1Name = "bruttovon"; param2Name = "bruttobis"; param3Name = "sv_satz";
                }
                if (tablename == "dg_abgaben" || tablename == "dg_abgaben_sz")
                {
                    cmd.CommandText = "insert into " + tablename + " values(null, @sv_dg, @bmv, @db, @dz, @kommst)";
                    param1Name = "sv_dg"; param2Name = "bmv"; param3Name = "db"; param4Name = "dz"; param5Name = "kommst";

                }
                if (tablename == "grenzen_sv")
                {
                    cmd.CommandText = "insert into " + tablename + " values(null, @hbgl, @gfg)";
                    param1Name = "hbgl"; param2Name = "gfg";
                }
                if (tablename == "grenzen_sv_sz")
                {
                    cmd.CommandText = "insert into " + tablename + " values(null, @hbgl_sz, @gfg)";
                    param1Name = "hbgl_sz"; param2Name = "gfg";
                }
                if (tablename == "sz_steuergrenzen")
                {
                    cmd.CommandText = "insert into " + tablename + " values(null, @bis, @prozent, @von)";
                    param1Name = "bis"; param2Name = "prozent"; param3Name = "von";
                }
                if (tablename == "freibetraege_lst")
                {
                    cmd.CommandText = "insert into " + tablename + " values(null, @§68/2, @§68/1)";
                    param1Name = "§68/2"; param2Name = "§68/1";
                }
                if (tablename == "effektiv_tarif_monat")
                {
                    cmd.CommandText = "insert into " + tablename + " values(null, @lst_bemgrundlage_von, @lst_bemgrundlage_bis, @grenzsteuersatz, @nullkind, @einkind, @zweikind, @dreikind, @vierkind, @fuenfkind)";
                    param1Name = "lst_bemgrundlage_von"; param2Name = "lst_bemgrundlage_bis"; param3Name = "grenzsteuersatz"; param4Name = "nullkind"; param5Name = "einkind"; param6Name = "zweikind"; param7Name = "dreikind"; param8Name = "vierkind"; param9Name = "fuenfkind";
                }
                if (tablename == "betrzugeh_arbeiter" || tablename == "betrzugeh_angestellter")
                {
                    cmd.CommandText = "insert into " + tablename + " values(null, @von, @bis, @prozentsatz)";

                    param1Name = "von"; param2Name = "bis"; param3Name = "prozentsatz";
                }
                if (tablename == "bundesland_dz")
                {
                    cmd.CommandText = "insert into " + tablename + " values(null, @bundesland, @prozent)";

                    param1Name = "bundesland"; param2Name = "prozent";
                }

                DbParameter param1 = cmd.CreateParameter();
                param1.ParameterName = param1Name;
                param1.DbType = DbType.Decimal;
                if (tablename != "bundesland_dz")
                {
                    param1.Value = newTable.Column1;
                }
                else
                {
                    param1.Value = newTable.Column2;
                }

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

                DbParameter param6 = cmd.CreateParameter();
                param6.ParameterName = param6Name;
                param6.DbType = DbType.Decimal;
                param6.Value = newTable.Column6;

                DbParameter param7 = cmd.CreateParameter();
                param7.ParameterName = param7Name;
                param7.DbType = DbType.Decimal;
                param7.Value = newTable.Column7;

                DbParameter param8 = cmd.CreateParameter();
                param8.ParameterName = param8Name;
                param8.DbType = DbType.Decimal;
                param8.Value = newTable.Column8;

                DbParameter param9 = cmd.CreateParameter();
                param9.ParameterName = param9Name;
                param9.DbType = DbType.Decimal;
                param9.Value = newTable.Column9;

                cmd.Parameters.Add(param1);
                cmd.Parameters.Add(param2);
                cmd.Parameters.Add(param3);
                cmd.Parameters.Add(param4);
                cmd.Parameters.Add(param5);
                cmd.Parameters.Add(param6);
                cmd.Parameters.Add(param7);
                cmd.Parameters.Add(param8);
                cmd.Parameters.Add(param9);

                return await cmd.ExecuteNonQueryAsync() == 1;
            }
            return false;
        }
    }
}
