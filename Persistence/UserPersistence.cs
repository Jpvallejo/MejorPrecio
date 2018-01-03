using System;
using System.Collections;
using System.Data.SqlClient;
using System.Text;
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
                SqlCommand command = new SqlCommand("SELECT COUNT(*) as count FROM Users WHERE Mail='" + mail + "'", conn);
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
                SqlCommand command = new SqlCommand("Update Users SET SearchHistory=@his WHERE Id=@Id", conn);
                command.Parameters.AddWithValue("@Id", us.Id);
                command.Parameters.AddWithValue("@His", his);
                command.ExecuteNonQuery();
                conn.Close();
            }
        }
        public Queue GetHistory(Guid IdUser)
        {
            using (SqlConnection Conn = new SqlConnection(cString))
            {
                Conn.Open();
                SqlCommand command = new SqlCommand("SELECT SearchHistory FROM Users WHERE Id=@ID", Conn);
                string[] dato1;
                command.Parameters.AddWithValue("@Id", IdUser);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    reader.Read();
                    dato1 = reader["SearchHistory"].ToString().Split(',');
                }
                Queue his = new Queue();
                for (int i = 0; i < dato1.Length; i++)
                {
                    his.Enqueue(dato1[i]);
                }

                return his;
            }
        }
    }
}