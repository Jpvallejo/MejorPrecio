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
                    command.CommandText = "INSERT INTO Users ([Id],[Token],[Name],[Mail],[Password],[Age],[Gender],[Verified] ,[Role]) VALUES (NEWID(),NEWID(),@name,@mail,@pass,@age,@gender,@verified,@role)";
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

        public bool CheckVerified(string mail)
        {
            var isVerified = false;
            using (SqlConnection conn = new SqlConnection(cString))
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT Verified FROM Users WHERE Mail= @mail";
                    command.Parameters.AddWithValue("@mail", mail);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();
                        isVerified = (bool)reader["Verified"];
                    }
                }
            }
            return isVerified;
        }

        public User GetUserByToken(Guid token)
        {
            var user = new User();
            using (SqlConnection conn = new SqlConnection(cString))
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT * FROM Users WHERE Token= @token";
                    command.Parameters.AddWithValue("@token", token);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();
                        user.Role = reader["Role"].ToString();
                        user.Name = (string)reader["Name"];
                        user.Age = (int)reader["Age"];
                        user.Id = (Guid)reader["Id"];
                        user.Verified = (bool)reader["Verified"];
                        user.Password = (string)reader["Password"];
                        user.Hisory = GetHistory(user.Id);
                        user.Mail = (string)reader["Mail"];
                    }
                }
            }
            return user;
        }

        public bool CheckValidToken(Guid token)
        {
            int dato1;
            using (SqlConnection conn = new SqlConnection(cString))
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT COUNT(*) as count FROM Users WHERE Token= @token";
                    command.Parameters.AddWithValue("@token", token);
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

        public User GetUserWithMail(string mail)
        {
            var user = new User();
            using (SqlConnection conn = new SqlConnection(cString))
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT * FROM Users WHERE Mail= @mail";
                    command.Parameters.AddWithValue("@mail", mail);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();
                        user.Role = reader["Role"].ToString();
                        user.Name = (string)reader["Name"];
                        user.Age = (int)reader["Age"];
                        user.Id = (Guid)reader["Id"];
                        user.Verified = (bool)reader["Verified"];
                        user.Password = (string)reader["Password"];
                        user.Hisory = GetHistory(user.Id);
                        user.Mail = mail;
                    }
                }
            }
            return user;
        }

        public bool Login(String password, String mail)
        {
            using (SqlConnection conn = new SqlConnection(cString))
            {
                int dato1;
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT COUNT(*) as count FROM Users WHERE Mail= @mail and Password= @password";
                    command.Parameters.AddWithValue("@mail", mail);
                    command.Parameters.AddWithValue("@password", password);
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

        public void ModifyToken(string mail)
        {
            using (SqlConnection conn = new SqlConnection(cString))
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "UPDATE Users SET Token=NEWID() WHERE Mail= @mail";
                    command.Parameters.AddWithValue("@mail", mail);
                    command.ExecuteNonQuery();

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

        public void VerifyUser(Guid token)
        {
            using (SqlConnection conn = new SqlConnection(cString))
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "UPDATE Users SET Verified='true', Token=NEWID() WHERE Token= @token";
                    command.Parameters.AddWithValue("@token", token);
                    command.ExecuteNonQuery();

                }
            }
        }

        public string GetEmailbyToken(Guid token)
        {
            var mail = "";
            using (SqlConnection conn = new SqlConnection(cString))
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT Mail FROM Users WHERE Token= @token";
                    command.Parameters.AddWithValue("@token", token);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();
                        mail = (string)reader["Mail"];
                    }
                }
            }
            return mail;
        }

        public void ModifyPassword(string mail, string newPassword)
        {
            using (SqlConnection conn = new SqlConnection(cString))
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "UPDATE Users SET Password=@password WHERE Mail= @mail";
                    command.Parameters.AddWithValue("@mail", mail);
                    command.Parameters.AddWithValue("@password", newPassword);
                    command.ExecuteNonQuery();

                }
            }
        }

        public Guid GetToken(string mail)
        {
            var token = Guid.Empty;
            using (SqlConnection conn = new SqlConnection(cString))
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT Token FROM Users WHERE Mail= @mail";
                    command.Parameters.AddWithValue("@mail", mail);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();
                        token = (Guid)reader["Token"];
                    }
                }
            }
            return token;
        }

        public void UpdateHistory(Guid userId,string newProduct)
        {
            var date = DateTimeOffset.Now;
            using (SqlConnection conn = new SqlConnection(cString))
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT INTO History (IdProduct,IdUser,Date) VALUES(@IdProd, @IdUser,@date)";
                    command.Parameters.AddWithValue("@IdProd", new ProductPersistence().GetProductByName(newProduct).Id);
                    command.Parameters.AddWithValue("@IdUser", userId);
                    command.Parameters.AddWithValue("@date", date);
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
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT TOP 5 * FROM History WHERE IdUser=@Id ORDER BY Date DESC";
                    command.Parameters.AddWithValue("@Id", IdUser);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while(reader.Read()){
                            var history = new History(){
                                Date = (DateTimeOffset)reader["Date"],
                                Product = new ProductPersistence().GetProductById((Guid)reader["IdProduct"])
                            };
                            his.Enqueue(history);
                        }
                    }
                }
                return his;
            }
        }
    }
}