using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using MejorPrecio3.Models;
using System;
using System.Data;

namespace MejorPrecio3.Persistence
{
    public class ProductPersistence
    {
        string cString = System.Environment.GetEnvironmentVariable("Connectionstring");
        public Product GetProductByName(string name)
        {

            using (var conn = new SqlConnection(cString))
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT * FROM Products WHERE Name = @searchName";
                    command.Parameters.AddWithValue("@searchName", name);

                    using (var reader = command.ExecuteReader())
                    {

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
            }
        }

        public Product GetProductByBarcode(string barcode)
        {

            using (var conn = new SqlConnection(cString))
            {
                conn.Open();
                using (var command =conn.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT * FROM Products WHERE Barcode = @barcode";
                    command.Parameters.AddWithValue("@barcode", barcode);

                    using (var reader = command.ExecuteReader())
                    {

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
            }
        }

        public Product GetProductById(Guid id)
        {

            using (var conn = new SqlConnection(cString))
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT * FROM Products WHERE Id = @id";
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {

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
            }
        }



        public List<Product> GetAllProducts()
        {
            var list = new List<Product>();
            using (var conn = new SqlConnection(cString))
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT * FROM Products";
                    using (var reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            var product = new Product()
                            {
                                Id = (Guid)reader["Id"],
                                Name = reader["Name"].ToString(),
                                Barcode = reader["Barcode"].ToString(),
                                Brand = reader["Brand"].ToString()
                            };
                            list.Add(product);
                        }
                    }
                    return list;
                }
            }
        }
        public List<Price> GetTopFive(Product product)
        {
            var list = new List<Price>();
            using (var conn = new SqlConnection(cString))
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT TOP 5 * FROM Prices WHERE productId = @searchId ORDER BY Price";
                    command.Parameters.AddWithValue("@searchId", product.Id);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var actualPrice = new Price();
                            actualPrice.latitude = (double)reader["Latitude"];
                            actualPrice.longitude = (double)reader["Longitude"];
                            actualPrice.price = (decimal)reader["Price"];
                            actualPrice.product = product;
                            actualPrice.Id = (Guid)reader["Id"];
                            actualPrice.date = (DateTimeOffset)reader["Date"];


                            list.Add(actualPrice);
                        }
                    }
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
                using (var command =  conn.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT Name FROM Products WHERE Name Like '%' + @searchName + '%' ORDER BY Name";
                    command.Parameters.AddWithValue("@searchName", ProductName);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(reader["Name"].ToString());
                        }
                    }
                    return list;

                }
            }
        }

        public bool SavePrice(Price price)
        {

            if (price.Id == Guid.Empty)
            {
                price.Id = Guid.NewGuid();
            }
            var parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@latitude", price.latitude),
                    new SqlParameter("@longitude", price.longitude),
                    new SqlParameter("@unitPrice", price.price),
                    new SqlParameter("@idProduct", price.product.Id),
                    new SqlParameter("@id", price.Id),
                    new SqlParameter("@date",price.date)
                };

            using (var conn = new SqlConnection(cString))
            {
                conn.Open();
                using (var command =  conn.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT INTO Prices (Id,Latitude,Longitude,Price,productId, Date) VALUES (@id,@latitude,@longitude, @unitPrice, @idProduct,@date)";
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
                using (var command =conn.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT 1 FROM Prices WHERE Latitude = @latitude AND Longitude = @longitude AND productId = @idProduct";
                    command.Parameters.AddRange(parameters.ToArray());

                    var reader = command.ExecuteScalar();

                    if (reader != null)
                    {
                        return true;
                    }

                    return false;

                }
            }
        }


        public bool UpdatePrice(Price price)
        {
            var parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@latitude", price.latitude),
                    new SqlParameter("@longitude", price.longitude),
                    new SqlParameter("@unitPrice", price.price),
                    new SqlParameter("@idProduct", price.product.Id),
                    new SqlParameter("@date",price.date)
                };
            using (var conn = new SqlConnection(cString))
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "UPDATE Prices SET Price = @unitPrice, Date = @date WHERE Latitude = @latitude AND Longitude = @longitude AND productId = @idProduct";
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
        }

        public void DeletePrice(Guid id)
        {
            using (var conn = new SqlConnection(cString))
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "DELETE FROM Prices WHERE Id = @id";
                    command.Parameters.AddWithValue("@id", id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Price> GetAllPrices()
        {
            var list = new List<Price>();
            using (var conn = new SqlConnection(cString))
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {

                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT * FROM Prices ORDER BY Date";
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var actualPrice = new Price();
                        actualPrice.product = this.GetProductById((Guid)reader["productId"]);
                        actualPrice.latitude = (double)reader["Latitude"];
                        actualPrice.longitude = (double)reader["Longitude"];
                        actualPrice.price = (decimal)reader["Price"];
                        actualPrice.Id = (Guid)reader["Id"];
                        actualPrice.date = (DateTimeOffset)reader["Date"];


                        list.Add(actualPrice);
                    }
                    return list;
                }
            }
        }




        public bool SaveProduct(Product product)
        {

            if (product.Id == Guid.Empty)
            {
                product.Id = Guid.NewGuid();
            }


            var parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@name", product.Name),
                    new SqlParameter("@barcode", product.Barcode),
                    new SqlParameter("@brand", product.Brand),
                    new SqlParameter("@id", product.Id)
                };

            if (this.ExistProduct(product.Name))
            {
                return false;
            }
            using (var conn = new SqlConnection(cString))
            {
                conn.Open();
                using (var command =  conn.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT INTO Products (Id,Name,Barcode,Brand) VALUES (@id,@name,@barcode, @brand)";
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
        }


        public bool ExistProduct(string productName)
        {
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@name", productName),
                
            };
            using (var conn = new SqlConnection(cString))
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT * FROM Products WHERE Name = @name";
                    command.Parameters.AddRange(parameters.ToArray());

                    var reader = command.ExecuteScalar();

                    if (reader != null)
                    {
                        return true;
                    }

                    return false;
                }
            }
        }

        public void DeleteProduct(Guid id)
        {
            using (var conn = new SqlConnection(cString))
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "DELETE FROM Products WHERE Id = @id";
                    command.Parameters.AddWithValue("@id", id);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}