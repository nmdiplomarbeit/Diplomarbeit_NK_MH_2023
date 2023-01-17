using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace LohnverrechnerGastro.Models.DB
{
    public class RepositoryUsersDB : IRepositoryUsersDB
    {
        private string _connectionString = "Server=localhost;database=pvdiplomarbeit;user=root;password=";
        private DbConnection _conn;


        public static bool IsLogged { get; set; }
        public static bool IsAdmin { get; set; }

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

        public async Task<bool> InsertAsync(User user, string salt)
        {

            if (this._conn?.State == ConnectionState.Open)
            {
                DbCommand cmdInsert = this._conn.CreateCommand();
                cmdInsert.CommandText = "insert into users values(null, @name, @email, sha2(@password, 512), @salt)";

                DbParameter paramN = cmdInsert.CreateParameter();
                paramN.ParameterName = "name";
                paramN.DbType = DbType.String;
                paramN.Value = user.Name;

                DbParameter paramEmail = cmdInsert.CreateParameter();
                paramEmail.ParameterName = "email";
                paramEmail.DbType = DbType.String;
                paramEmail.Value = user.Email;

                DbParameter paramPWD = cmdInsert.CreateParameter();
                paramPWD.ParameterName = "password";
                paramPWD.DbType = DbType.String;
                paramPWD.Value = user.Password + salt;

                DbParameter paramSALT = cmdInsert.CreateParameter();
                paramSALT.ParameterName = "salt";
                paramSALT.DbType = DbType.String;
                paramSALT.Value = salt;

                //DbParameter paramLogged = cmdInsert.CreateParameter();
                //paramLogged.ParameterName = "isLogged";
                //paramLogged.DbType = DbType.String;
                //paramLogged.Value = user.IsLogged;

                cmdInsert.Parameters.Add(paramN);
                cmdInsert.Parameters.Add(paramEmail);
                cmdInsert.Parameters.Add(paramPWD);
                cmdInsert.Parameters.Add(paramSALT);

                //cmdInsert.Parameters.Add(paramLogged);

                return await cmdInsert.ExecuteNonQueryAsync() == 1;
            }

            return false;
        }


        public async Task<User> GetUserAsync(int userId)
        {
            User user;
            if (this._conn?.State == ConnectionState.Open)
            {
                DbCommand cmd = this._conn.CreateCommand();
                cmd.CommandText = "select * from users where userId = @userId";     // Name der ID in DB und in Code gleich,
                DbParameter paramID = cmd.CreateParameter();                        //      -> weniger Fehler
                paramID.ParameterName = "userId";
                paramID.DbType = DbType.String;
                paramID.Value = userId;
                cmd.Parameters.Add(paramID);
                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        user = new User
                        {
                            UserId = userId,
                            Name = Convert.ToString(reader["name"]),
                            Email = Convert.ToString(reader["email"]),
                            Password = Convert.ToString(reader["password"]),
                            //IsLogged = Convert.ToBoolean(reader["isLogged"])
                        };
                        return user;
                    }
                }
            }
            return null;

        }

        //public async Task<bool> UpdateLoggedAsync(int userId, bool newLogged)
        //{
        //    if (this._conn?.State == ConnectionState.Open)
        //    {
        //        DbCommand cmd = this._conn.CreateCommand();
        //        cmd.CommandText = "update users set isLogged = @isLogged where userId = @userId";

        //        DbParameter paramID = cmd.CreateParameter();
        //        paramID.ParameterName = "userId";
        //        paramID.DbType = DbType.Int32;
        //        paramID.Value = paramID;

        //        DbParameter paramLogged = cmd.CreateParameter();
        //        paramLogged.ParameterName = "isLogged";
        //        paramLogged.DbType = DbType.String;
        //        paramLogged.Value = newLogged;

        //        cmd.Parameters.Add(paramID);
        //        cmd.Parameters.Add(paramLogged);

        //        return await cmd.ExecuteNonQueryAsync() == 1;
        //    }

        //    return false;
        //}

        public async Task<bool> AskEmailAsync(string email)
        {
            if (this._conn?.State == ConnectionState.Open)
            {
                DbCommand cmd = this._conn.CreateCommand();
                cmd.CommandText = "select * from users where email = @email";

                DbParameter paramEM = cmd.CreateParameter();
                paramEM.ParameterName = "email";
                paramEM.DbType = DbType.String;
                paramEM.Value = email;

                cmd.Parameters.Add(paramEM);


                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public async Task<bool> AskNameAsync(string name)
        {
            DbCommand cmd = this._conn.CreateCommand();
            cmd.CommandText = "select * from users where name = @name";

            DbParameter paramN = cmd.CreateParameter();
            paramN.ParameterName = "name";
            paramN.DbType = DbType.String;
            paramN.Value = name;

            cmd.Parameters.Add(paramN);


            using (DbDataReader reader = await cmd.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> LoginAsync(User userdaten)

        {
            if (this._conn?.State == ConnectionState.Open)
            {
                string salt = "";

                DbCommand cmdSalt = this._conn.CreateCommand();
                cmdSalt.CommandText = "select salt from users where name = @name";
                DbParameter paramNSalt = cmdSalt.CreateParameter();
                paramNSalt.ParameterName = "name";
                paramNSalt.DbType = DbType.String;
                paramNSalt.Value = userdaten.Name;
                cmdSalt.Parameters.Add(paramNSalt);
                using (DbDataReader reader = await cmdSalt.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        salt = Convert.ToString(reader["salt"]);
                    }
                }

                DbCommand cmd = this._conn.CreateCommand();

                cmd.CommandText = "select name from users where name = @name and password = sha2(@password, 512)";
                DbParameter paramN = cmd.CreateParameter();
                paramN.ParameterName = "name";
                paramN.DbType = DbType.String;
                paramN.Value = userdaten.Name;

                DbParameter paramPWD = cmd.CreateParameter();
                paramPWD.ParameterName = "password";
                paramPWD.DbType = DbType.String;
                paramPWD.Value = userdaten.Password + salt;

                cmd.Parameters.Add(paramN);
                cmd.Parameters.Add(paramPWD);

                if (userdaten.Name == "AdminAdmin123" && userdaten.Password == "AdminAdmin123")
                {
                    IsAdmin = true;
                }

                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        IsLogged = true;
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
