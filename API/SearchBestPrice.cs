using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MejorPrecio3.Models;
using MejorPrecio3.Persistence;

namespace MejorPrecio3.API
{
    public class SearchBestPrice
    {
        private static ProductPersistence productPersistence = new ProductPersistence();
        private static UserPersistence userPersistence = new UserPersistence();

        //This function searches products on a productList by their name and returns
        //all products with that name on another list
        public List<Price> SearchProductName(string productName)
        {
            var productList = new List<Price>();

            var product = productPersistence.GetProductByName(productName);
            productList = productPersistence.GetTopFive(product);
            return productList;
        }

        public bool IsUserVerified(string mail, string password)
        {
            return userPersistence.CheckVerified(mail);
        }

        //This function searches products by their barCode
        public List<Price> SearchProductBarcode(string barcode)
        {
            List<Price> productList = new List<Price>();

            var product = productPersistence.GetProductByBarcode(barcode);
            productList = productPersistence.GetTopFive(product);
            return productList;
        }

        public User GetUserByEmail(string mail)
        {
            return userPersistence.GetUserWithMail(mail);
        }

        public bool CheckValidToken(Guid token)
        {
            return userPersistence.CheckValidToken(token);
        }

        public bool ProductIsValid(string selectedProduct)
        {
            return productPersistence.ExistProduct(selectedProduct);
        }

        public User GetUserByToken(Guid token)
        {
            return userPersistence.GetUserByToken(token);
        }

        public void CreateUser(User user)
        {
            var pass = user.Password;
            try
            {

                if (String.IsNullOrEmpty(user.Name))
                    throw new Exception("Error en el usuario. Debe tener Nombre");

                if (!isPasswordValid(user.Password))
                    throw new Exception("La contraseña debe tener un minimo de 8 caracteres, con una Mayuscula, una minuscula y un número");

                Regex regex = new Regex(@"(\w+)@(\w+)\.(\w+)");
                if (!regex.IsMatch(user.Mail))
                    throw new Exception("Error en el mail");

                if (user.Age <= 0 || user.Age > 100)
                    throw new Exception("Error en la edad. Debes ser mayor a 0, y menor a 100");

                byte[] data = System.Text.Encoding.ASCII.GetBytes(user.Password);
                data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
                user.Password = System.Text.Encoding.ASCII.GetString(data);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            userPersistence.Add(user);
        }

        private bool isPasswordValid(string password)
        {
            Regex pat = new Regex(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$");
            if (!pat.IsMatch(password))
                return false;
            return true;
        }

        public void VerifyUser(Guid token)
        {
            userPersistence.VerifyUser(token);
        }

        public Guid GetUserToken(string mail)
        {
            return userPersistence.GetToken(mail);
        }

        public bool Login(string password, string mail)
        {
            try
            {
                byte[] data = System.Text.Encoding.ASCII.GetBytes(password);
                data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
                password = System.Text.Encoding.ASCII.GetString(data);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            bool success = userPersistence.Login(password, mail);
            return success;
        }

        public bool Exist(string mail)
        {
            bool exist = userPersistence.Exist(mail);
            return exist;
        }

        public bool SavePrice(Price price)
        {
            if (productPersistence.ExistPrice(price))
            {
                return productPersistence.UpdatePrice(price);
            }
            else
            {
                return productPersistence.SavePrice(price);
            }
        }

        public string GetEmailByToken(Guid token)
        {
            return userPersistence.GetEmailbyToken(token);
        }

        public void ModifyToken(string mail)
        {
            userPersistence.ModifyToken(mail);
        }

        public List<Price> GetAllPrices()
        {
            return productPersistence.GetAllPrices();
        }

        public List<Product> GetAllProducts()
        {
            return productPersistence.GetAllProducts();
        }

        public Product GetProductByName(string name)
        {
            return productPersistence.GetProductByName(name);
        }

        public void ModifyPassword(string mail, string password)
        {
            if (!isPasswordValid(password))
            {
                throw new Exception("La contraseña deve tener un minimo de 8 caracteres, con una Mayuscula, una minuscula y un número");
            }
            byte[] data = System.Text.Encoding.ASCII.GetBytes(password);
            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            var newPassword = System.Text.Encoding.ASCII.GetString(data);
            userPersistence.ModifyPassword(mail, newPassword);
        }

        public List<string> GetSimilarNames(string name)
        {
            return productPersistence.GetAllNames(name);
        }

        public void DeletePrice(Guid id)
        {
            productPersistence.DeletePrice(id);
        }

        public bool SaveProduct(Product product)
        {
            if (productPersistence.SaveProduct(product))
            {
                return true;
            }
            return false;
        }

        public void DeleteProduct(Guid id)
        {
            productPersistence.DeleteProduct(id);
        }

        public void UpdateSearchHistory(Guid userId, string newProduct)
        {
            userPersistence.UpdateHistory(userId, newProduct);
        }

        public IEnumerable<History> GetSearchHistory(Guid userId)
        {
            var result = userPersistence.GetHistory(userId).Cast<History>();
            return result;
        }
    }
}