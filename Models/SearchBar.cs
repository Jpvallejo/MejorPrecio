using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mejor_precio_3.Models
{
    public class SearchBar
    {
        //This function searches products by their name
        public List<Product> searchProduct (string productName)
        {
            List<Product> productList = new List<Product>();
            //search in the DB for all products that contain the string
            //received through parameter and add them to the list

            //MOCK SECTION
            for (int i = 0; i < 10; i++)
            {
                Product product = new Product();
                product.Name = productName;

                double price = i * 1.10;
                product.Price = (decimal)price;
                product.Barcode = 24871110;
                productList.Add(product);
            }
            // END MOCK


            return productList;
        }


        //This function searches products by their barCode
        public List<Product> searchProduct(int barCode)
        {
            List<Product> productList = new List<Product>();

            //search in the DB for all products with the barCode
            //received as a parameter

            //MOCK SECTION
            for (int i = 0; i < 10; i++)
            {
                Product product = new Product();
                product.Name = "Galletitas '9 De Oro'";

                double price = i * 1.10;
                product.Price = (decimal)price;
                product.Barcode = barCode;
                productList.Add(product);
            }
            // END MOCK

            return productList;
        }
    }
}
