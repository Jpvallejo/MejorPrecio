using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MejorPrecio3.Models;
using MejorPrecio3.Persistence;

namespace MejorPrecio3.API
{
    public class SearchBestPrice
    {
        private static ProductPersistence persistence = new ProductPersistence();
        
        //This function searches products on a productList by their name and returns
        //all products with that name on another list
        public List<Price> SearchProductName(string productName)
        {
            var persistence = new ProductPersistence();
            var productList = new List<Price>();

            var product = persistence.GetProductByName(productName);
            productList = persistence.GetTopFive(product);
            return productList;
        }

        //This function searches products by their barCode
        public List<Price> SearchProductBarcode(string barcode)
        {
            List<Price> productList = new List<Price>();

            var product = persistence.GetProductByBarcode(barcode);
            productList = persistence.GetTopFive(product);
            return productList;
        }

        public void CreateUser(User user)
        {
            var pass = user.Password;
            try
            {
                Regex pat = new Regex(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$");
                if (!pat.IsMatch(user.Password))
                    throw new Exception("Error en la contrasena +8, Mayus,Minus,numero");

                Regex regex = new Regex(@"(\w+)@(\w+)\.(\w+)");
                if (!regex.IsMatch(user.Mail))
                    throw new Exception("Error en el mail");

                byte[] data = System.Text.Encoding.ASCII.GetBytes(user.Password);
                data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
                user.Password = System.Text.Encoding.ASCII.GetString(data);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            new UserPersistence().Add(user);
        }

        public bool Login (User user)
        {
            try
            {
                byte[] data = System.Text.Encoding.ASCII.GetBytes(user.Password);
                data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
                user.Password = System.Text.Encoding.ASCII.GetString(data);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            
            bool success = new UserPersistence().Login(user);
            return success;
        }

        public bool Exist(string mail)
        {
            UserPersistence up = new UserPersistence();
            bool exist = up.Exist(mail);
            return exist;
        }

        public bool SavePrice(Price price)
        {
            if (persistence.ExistPrice(price))
            {
                return persistence.UpdatePrice(price);
            }
            else
            {
                return persistence.SavePrice(price);
            }
        }

        public List<Price> GetAllPrices()
        {
            return persistence.GetAllPrices();
        }

        public List<Product> GetAllProducts()
        {
            return persistence.GetAllProducts();
        }

        public Product GetProductByName(string name)
        {
            return persistence.GetProductByName(name);
        }

        public List<string> GetSimilarNames(string name)
        {
            return persistence.GetAllNames(name);
        }

        public void DeletePrice(Guid id)
        {
            persistence.DeletePrice(id);
        }

        public bool SaveProduct(Product product)
        {
            if (persistence.SaveProduct(product))
            {
                return true;
            }
            return false;
        }

        public void DeleteProduct(Guid id)
        {
            persistence.DeleteProduct(id);
        }

        public void UpdateSearchHistory(User user)
        {
            new UserPersistence().UpdateHistory(user);
        }

        public IEnumerable<string> GetSearchHistory(Guid userId)
        {
            var result = new UserPersistence().GetHistory(userId).Cast<string>();
            return result;
        }
    }
}