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

        public async Task<bool> InsertAsync(User user)
        {

            if (this._conn?.State == ConnectionState.Open)
            {
                DbCommand cmdInsert = this._conn.CreateCommand();
                cmdInsert.CommandText = "insert into users values(null, @name, @email, sha2(@password, 512))";

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
                paramPWD.Value = user.Password;

                cmdInsert.Parameters.Add(paramN);
                cmdInsert.Parameters.Add(paramPWD);
                cmdInsert.Parameters.Add(paramEmail);

                return await cmdInsert.ExecuteNonQueryAsync() == 1;
            }

            return false;
        }

        public async Task<User> GetKundeAsync(int userId)
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
                            Password = Convert.ToString(reader["password"])
                        };
                        return user;
                    }
                }
            }
            return null;

        }

        public async Task<bool> LoginAsync(string name, string password)

        {
            if (this._conn?.State == ConnectionState.Open)
            {
                DbCommand cmdInsert = this._conn.CreateCommand();

                cmdInsert.CommandText = "select name from users where name = @name and password = sha2(@password, 512)";
                DbParameter paramN = cmdInsert.CreateParameter();
                paramN.ParameterName = "name";
                paramN.DbType = DbType.String;
                paramN.Value = name;

                DbParameter paramPWD = cmdInsert.CreateParameter();
                paramPWD.ParameterName = "password";
                paramPWD.DbType = DbType.String;
                paramPWD.Value = password;

                cmdInsert.Parameters.Add(paramN);
                cmdInsert.Parameters.Add(paramPWD);

                using (DbDataReader reader = await cmdInsert.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
