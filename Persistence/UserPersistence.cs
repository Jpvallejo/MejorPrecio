using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using MejorPrecio3.Models;

namespace MejorPrecio3.Persistence
{
    public class UserPersistence
    {
        string cString = System.Environment.GetEnvironmentVariable("Connectionstring"); //A cambiar cuando nos den el cstring de Azure

        public void Add(User user)
        {
            using (SqlConnection conn = new SqlConnection(cString))
            {
                conn.Open();
                using (SqlCommand command = conn.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT INTO Users ([Id],[Name],[Mail],[Password],[Age],[Gender],[Verified] ,[Role]) VALUES (NEWID(),@name,@mail,@pass,@age,@gender,@verified,@role)";
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
        }

        public bool Login(User user)
        {
            using(SqlConnection conn = new SqlConnection(cString))
            {
                int dato1;
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT COUNT(*) as count FROM Users WHERE Mail= @mail and Password= @password";
                    command.Parameters.AddWithValue("@mail", user.Mail);
                    command.Parameters.AddWithValue("@password", user.Password);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();
                     dato1 = Convert.ToInt32(reader["count"]);   
                    }
                }
                conn.Close();
                if (dato1 == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool Exist(string mail)
        {
            int dato1;
            using (SqlConnection conn = new SqlConnection(cString))
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT COUNT(*) as count FROM Users WHERE Mail= @mail";
                    command.Parameters.AddWithValue("@mail", mail);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();
                        dato1 = Convert.ToInt32(reader["count"]);
                    }
                }
            }
            if (dato1 > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void UpdateHistory(User us)
        {
            Queue his = us.Hisory;
            string[] dato1 = new string[5];
            for (int i = 0; i < his.Count; i++)
            {
                dato1[i] = his.Dequeue().ToString();
            }
            StringBuilder stringBuilder = null;
            for (int i = 0; i <= his.Count - 1; i++)
            {
                stringBuilder.Append(dato1[i]);
                stringBuilder.Append(",");
            }
            stringBuilder.Append(dato1[his.Count - 1]);
            using (SqlConnection conn = new SqlConnection(cString))
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "Update Users SET SearchHistory=@his WHERE Id=@Id";
                    command.Parameters.AddWithValue("@Id", us.Id);
                    command.Parameters.AddWithValue("@His", his);
                    command.ExecuteNonQuery();
                }
            }
        }
        public Queue GetHistory(Guid IdUser)
        {
            using (SqlConnection Conn = new SqlConnection(cString))
            {
                Conn.Open();
                Queue his = new Queue();
                using (var command = Conn.CreateCommand())
                {
                    string[] dato1;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT SearchHistory FROM Users WHERE Id=@ID";
                    command.Parameters.AddWithValue("@Id", IdUser);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();
                        dato1 = reader["SearchHistory"].ToString().Split(',');
                    }
                    for (int i = 0; i < dato1.Length; i++)
                    {
                        his.Enqueue(dato1[i]);
                    }
                }
                return his;
            }
        }
    }
}