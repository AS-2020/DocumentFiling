using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_Dokumentenablage.Models
{
    class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    class UserHandler
    {
        private List<User> users;

        public UserHandler()
        {
            users = new List<User>();
        }
        private static UserHandler instance;
        public static UserHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UserHandler();
                }
                return instance;
            }
        }
        private const string VERBINDUNG = @"Data Source=localhost\sqlexpress;Initial Catalog=DocumentFiling;User ID=sa;Password=P@ssword";

        public void AddUser(User user)
        {
            users.Add(user);
        }

        public List<User> GetUsers()
        {
            return users;
        }
        public void Load()
        {
            SqlConnection con = new SqlConnection(VERBINDUNG);

            string sql = "Select * from documentFilingPassword";

            con.Open();

            SqlCommand com = new SqlCommand(sql, con);
            SqlDataReader reader = com.ExecuteReader();

            while (reader.Read())
            {
                User user = new User();

                user.UserName = reader.GetValue(1).ToString();
                user.Password = reader.GetValue(2).ToString();
                Instance.AddUser(user);
            }

            reader.Close();
            com.Dispose();
            con.Close();
        }
    }

}
