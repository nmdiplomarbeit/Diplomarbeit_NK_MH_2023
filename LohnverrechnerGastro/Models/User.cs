using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LohnverrechnerGastro.Models
{
    public class User
    {
        private int userId;



        public int UserId
        {
            get { return this.userId; }
            set
            {
                if (value >= 0)
                {
                    this.userId = value;
                }
            }
        }

        public string Name { get; set; }

        public string Email { get; set; }  

        public string Password { get; set; }

        public bool IsLogged { get; set; }

        public User() : this(0, "", "") { }

        public User(int userId, string name, string password)
        {
            this.UserId = userId;
            this.Name = name;
            this.Password = password;
            this.IsLogged = false;
        }

    }
}
