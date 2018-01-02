using System;
using System.Data.SqlClient;
using MejorPrecio3.Models;

namespace MejorPrecio3.Persistence
{
    public class UserPersistence
    {
        string cString = System.IO.File.ReadAllText(System.Environment.GetEnvironmentVariable("Connectionstring")); //A cambiar cuando nos den el cstring de Azure

        public void Add(User user)
        {
            using (SqlConnection conn = new SqlConnection(cString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("INSERT INTO Users ([Id],[Name],[Mail],[Password],[Age],[Gender],[Verified] ,[Role]) VALUES (NEWID(),@name,@mail,@pass,@age,@gender,@verified,@role)", conn);
                command.Parameters.AddWithValue("@name", user.Name);
                command.Parameters.AddWithValue("@mail", user.Mail);
                command.Parameters.AddWithValue("@pass", user.Password);
                command.Parameters.AddWithValue("@age", user.Age);
                command.Parameters.AddWithValue("@gender", user.Gender);
                command.Parameters.AddWithValue("@verified", user.Verified);
                command.Parameters.AddWithValue("@role", user.Role);
                command.ExecuteNonQuery();
                conn.Close();
            }
        }
        public bool Exist(string mail)
        {
            using (SqlConnection conn = new SqlConnection(cString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("SELECT COUNT(*) as count FROM Users WHERE Mail='" + mail+"'", conn);
                int dato1;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    reader.Read();
                    dato1 = Convert.ToInt32(reader["count"]);
                }
                conn.Close();
                if (dato1 > 0)
                    return true;
                return false;
            }
        }
    }
}