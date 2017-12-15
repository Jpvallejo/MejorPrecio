using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

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
        {
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
        public List<Price> GetTopFive(int productId)
        {
            var list = new List<Price>();
            using (var conn = new SqlConnection(cString))
            {
                conn.Open();
                var command = new SqlCommand("SELECT TOP 5 * FROM Prices WHERE productId = @searchId ORDER BY Price", conn);
                command.Parameters.AddWithValue("@searchId", productId);

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int.TryParse(reader["Id"].ToString(), out int id);
                    decimal.TryParse(reader["Price"].ToString(), out decimal price);
                    double.TryParse(reader["Latitude"].ToString(),out double lat);
                    double.TryParse(reader["Longitude"].ToString(),out double longitude);

                    var actualPrice = new Price();
                    actualPrice.latitude = lat;
                    actualPrice.longitude = longitude;
                    actualPrice.price = price;
                    actualPrice.Id = id;
                    actualPrice.productId = productId;

                    list.Add(actualPrice);
                }
                return list;
            }
        }


        public bool SavePrice(Price price)
        {

            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@latitude", price.latitude),
                new SqlParameter("@longitude", price.longitude),
                new SqlParameter("@unitPrice", price.price),
                new SqlParameter("@idProduct", price.productId)
            };
            using (var conn = new SqlConnection(cString))
            {
                conn.Open();
                var command = new SqlCommand("INSERT INTO Prices (Latitude,Longitude,Price,productId) VALUES (@latitude,@longitude, @unitPrice, @idProduct)", conn);
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
}