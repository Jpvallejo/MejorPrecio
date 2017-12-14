using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace mejor_precio_3.Models
{
    public class ProductPersistence
    {
        string cString = @"Server=localhost\SQLEXPRESS;Database=Mejor_Precio_3;Trusted_Connection=True"; //A cambiar cuando nos den el cstring de Azure
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

                    var actualPrice = new Price();
                    actualPrice.location = reader["Location"].ToString();
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
                new SqlParameter("@adress", SqlDbType.NVarChar) {Value = price.location},
                new SqlParameter("@unitPrice", SqlDbType.Money) {Value = price.price},
                new SqlParameter("@idProduct", SqlDbType.Int) { Value = price.productId},
            };
            using (var conn = new SqlConnection(cString))
            {
                conn.Open();
                var command = new SqlCommand("INSERT INTO Prices (Location,Price,productId) VALUES (@adress, @unitPrice, @idProduct)", conn);
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