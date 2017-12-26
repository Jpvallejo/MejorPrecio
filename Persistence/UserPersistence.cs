using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Net;
using System.Text.RegularExpressions;
using mejor_precio_3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace mejor_precio_3.Persistence
{
    public class UserPersistence
    {
        public void Add(User user)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = @"Server=localhost\SQLEXPRESS;Database=Mejor_Precio_3;Trusted_Connection=True";
                conn.Open();
                SqlCommand command = new SqlCommand("INSERT INTO Users ([Name],[Mail],[Password],[Age],[Gender],[Verified] ,[Role]) VALUES (@name,@mail,@pass,@age,@gender,@verified,@role)", conn);
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
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = @"Server=localhost\SQLEXPRESS;Database=Mejor_Precio_3;Trusted_Connection=True";
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