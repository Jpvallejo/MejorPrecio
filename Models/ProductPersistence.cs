using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace mejor_precio_3.Models
{
    public class ProductPersistence
    {
        string cString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=mejor_precio_3;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"; //A cambiar cuando nos den el cstring de Azure
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
                    int.TryParse(reader["Id"].ToString(), out int resul);
                    product.Id = resul;
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
                    int.TryParse(reader["Id"].ToString(), out int resul);
                    product.Id = resul;
                }
                return product;
            }
        }



        public List<string> GetAllProductNames()
        { StringBuilder sb = new StringBuilder();
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
        public List<PriceViewModel> GetTopFive(Product product)
        {
            var geocoder = new Geocoder();
            var list = new List<PriceViewModel>();
            using (var conn = new SqlConnection(cString))
            {
                conn.Open();
                var command = new SqlCommand("SELECT TOP 5 * FROM Prices WHERE productId = @searchId ORDER BY Price", conn);
                command.Parameters.AddWithValue("@searchId", product.Id);

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int.TryParse(reader["Id"].ToString(), out int id);
                    decimal.TryParse(reader["Price"].ToString(), out decimal price);
                    double.TryParse(reader["Latitude"].ToString(), out double lat);
                    double.TryParse(reader["Longitude"].ToString(), out double longitude);

                    var actualPrice = new PriceViewModel();
                    actualPrice.latitude = lat;
                    actualPrice.longitude = longitude;
                    actualPrice.price = price;
                    actualPrice.productName = product.Name;
                    actualPrice.productBrand = product.Brand;
                    actualPrice.productBarcode = product.Barcode;
                    actualPrice.adress = geocoder.GetAdress(lat, longitude);


                    list.Add(actualPrice);
                }
                return list;
            }
        }
   public List<string> GetProductAutoComplete(string ProductName)
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
                    new SqlParameter("@idProduct", price.productId)
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
                    command = new SqlCommand("INSERT INTO Prices (Latitude,Longitude,Price,productId) VALUES (@latitude,@longitude, @unitPrice, @idProduct)", conn);
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
                new SqlParameter("@idProduct", price.productId)
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

        public void DeletePrice(Price price)
        {
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@latitude", price.latitude),
                new SqlParameter("@longitude", price.longitude),
                new SqlParameter("@idProduct", price.productId)
            };
            using (var conn = new SqlConnection(cString))
            {
                conn.Open();
                var command = new SqlCommand("DELETE * FROM Prices WHERE Latitude = @latitude AND Longitude = @longitude AND productId = @idProduct", conn);
                command.Parameters.AddRange(parameters.ToArray());

                command.ExecuteNonQuery();
            }
        }
    }
}