using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using MejorPrecio3.Models;
using System;

namespace MejorPrecio3.Persistence
{
    public class ProductPersistence
    {
        string cString = @"Server=localhost\SQLEXPRESS;Database=Mejor_Precio_3;Trusted_Connection=True;"; //A cambiar cuando nos den el cstring de Azure
        public Product GetProductByName(string name)
        {

            using (var conn = new SqlConnection(cString))
            {
                conn.Open();
                var command = new SqlCommand("SELECT * FROM Products WHERE Name = @searchName", conn);
                command.Parameters.AddWithValue("@searchName", name);

                var reader = command.ExecuteReader();

                var product = new Product();
                while (reader.Read())
                {
                    product.Name = reader["Name"].ToString();
                    product.Brand = reader["Brand"].ToString();
                    product.Barcode = reader["Barcode"].ToString();
                    product.Id = (Guid)reader["Id"];
                }
                return product;
            }
        }


        public Product GetProductByBarcode(string barcode)
        {

            using (var conn = new SqlConnection(cString))
            {
                conn.Open();
                var command = new SqlCommand("SELECT * FROM Products WHERE Barcode = @barcode", conn);
                command.Parameters.AddWithValue("@barcode", barcode);

                var reader = command.ExecuteReader();

                var product = new Product();
                while (reader.Read())
                {
                    product.Name = reader["Name"].ToString();
                    product.Brand = reader["Brand"].ToString();
                    product.Barcode = barcode;
                    product.Id = (Guid)reader["Id"];
                }
                return product;
            }
        }

        public Product GetProductById(Guid id)
        {

            using (var conn = new SqlConnection(cString))
            {
                conn.Open();
                var command = new SqlCommand("SELECT * FROM Products WHERE Id = @id", conn);
                command.Parameters.AddWithValue("@id", id);

                var reader = command.ExecuteReader();

                var product = new Product();
                while (reader.Read())
                {
                    product.Name = reader["Name"].ToString();
                    product.Brand = reader["Brand"].ToString();
                    product.Barcode = reader["Barcode"].ToString();
                    product.Id = id;
                }
                return product;
            }
        }



        public List<string> GetAllProductNames()
        {
            StringBuilder sb = new StringBuilder();
            var list = new List<string>();
            using (var conn = new SqlConnection(cString))
            {
                conn.Open();
                var command = new SqlCommand("SELECT Name FROM Products", conn);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(reader["Name"].ToString());
                }
                return list;
            }
        }
        public List<Price> GetTopFive(Product product)
        {
            var list = new List<Price>();
            using (var conn = new SqlConnection(cString))
            {
                conn.Open();
                var command = new SqlCommand("SELECT TOP 5 * FROM Prices WHERE productId = @searchId ORDER BY Price", conn);
                command.Parameters.AddWithValue("@searchId", product.Id);

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var actualPrice = new Price();
                    actualPrice.latitude = (double)reader["Latitude"];
                    actualPrice.longitude = (double)reader["Longitude"];
                    actualPrice.price = (decimal)reader["Price"];
                    actualPrice.product = product;
                    actualPrice.Id = (Guid)reader["Id"];


                    list.Add(actualPrice);
                }
                return list;
            }
        }
        public List<string> GetAllNames(string ProductName)
        {
            var list = new List<string>();
            using (var conn = new SqlConnection(cString))
            {
                conn.Open();
                var command = new SqlCommand("SELECT Name FROM Products WHERE Name Like '%' + @searchName + '%' ORDER BY Name", conn);
                command.Parameters.AddWithValue("@searchName", ProductName);

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(reader["Name"].ToString());
                }
                return list;

            }
        }

        public bool SavePrice(Price price)
        {
            SqlCommand command = null;
            var parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@latitude", price.latitude),
                    new SqlParameter("@longitude", price.longitude),
                    new SqlParameter("@unitPrice", price.price),
                    new SqlParameter("@idProduct", price.product.Id)
                };

            using (var conn = new SqlConnection(cString))
            {
                conn.Open();
                if (this.ExistPrice(price))
                {
                    command = new SqlCommand("UPDATE Prices SET Price = @unitPrice WHERE Latitude = @latitude AND Longitude = @longitude AND productId = @idProduct", conn);

                }
                else
                {
                    command = new SqlCommand("INSERT INTO Prices (Id,Latitude,Longitude,Price,productId) VALUES (NEWID(),@latitude,@longitude, @unitPrice, @idProduct)", conn);
                }
                command.Parameters.AddRange(parameters.ToArray());
                try
                {
                    command.ExecuteNonQuery();
                    return true;
                }
                catch
                {
                    return false;
                }

            }
        }

        public bool ExistPrice(Price price)
        {
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@latitude", price.latitude),
                new SqlParameter("@longitude", price.longitude),
                new SqlParameter("@idProduct", price.product.Id)
            };
            using (var conn = new SqlConnection(cString))
            {
                conn.Open();
                var command = new SqlCommand("SELECT * FROM Prices WHERE Latitude = @latitude AND Longitude = @longitude AND productId = @idProduct", conn);
                command.Parameters.AddRange(parameters.ToArray());

                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    return true;
                }

                return false;

            }
        }

        public void DeletePrice(Guid id)
        {
            using (var conn = new SqlConnection(cString))
            {
                conn.Open();
                var command = new SqlCommand("DELETE FROM Prices WHERE Id = @id", conn);
                command.Parameters.AddWithValue("@id",id);

                command.ExecuteNonQuery();
            }
        }

        public List<Price> GetAllPrices()
        {
            var list = new List<Price>();
            using (var conn = new SqlConnection(cString))
            {
                conn.Open();
                var command = new SqlCommand("SELECT * FROM Prices", conn);


                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var actualPrice = new Price();
                    actualPrice.product = this.GetProductById((Guid)reader["productId"]);
                    actualPrice.latitude = (double)reader["Latitude"];
                    actualPrice.longitude = (double)reader["Longitude"];
                    actualPrice.price = (decimal)reader["Price"];
                    actualPrice.Id = (Guid)reader["Id"];


                    list.Add(actualPrice);
                }
                return list;
            }
        }
    }
}