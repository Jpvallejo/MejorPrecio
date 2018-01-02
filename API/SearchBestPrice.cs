using System;
using System.Collections.Generic;
using System.Linq;
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
            new UserPersistence().Add(user);
        }
        public bool Exist(string mail)
        {
            UserPersistence up = new UserPersistence();
            bool exist = up.Exist(mail);
            return exist;
        }


        public bool SavePrice(Price price)
        {
            if (persistence.SavePrice(price))
            {
                return true;

            }
            return false;
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

        public IEnumerable<string> GetSimilarNames(string name)
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
    }
}