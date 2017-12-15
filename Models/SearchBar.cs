using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mejor_precio_3.Models
{
    public class SearchBar
    {
        //This function searches products on a productList by their name and returns
        //all products with that name on another list
        public List<Price> SearchProductName(string productName)
        {
            var persistence = new ProductPersistence();
            List<Price> productList = new List<Price>();

            var productId = persistence.GetProductByName(productName).Id;
            productList = persistence.GetTopFive(productId);
            return productList;
        }

        //This function searches products by their barCode
        public List<Price> SearchProductBarcode(string barcode)
        {/*
            List<Price> productList = new List<Price>();
            //search in the DB for all products that contain the string
            //received through parameter and add them to the list

            //for (int i = 0; i < originList.Count(); i++)
            foreach (var article in originList)
            {

                if (article.product.Barcode == barCode)
                {
                    productList.Add(article);
                }
                if (productList.Count() >= 5)
                {
                    break;
                }
            }
            return productList;
            */ 
            var persistence = new ProductPersistence();
            List<Price> productList = new List<Price>();

            var productId = persistence.GetProductByBarcode(barcode).Id;
            productList = persistence.GetTopFive(productId);
            return productList;
        }
    }
}