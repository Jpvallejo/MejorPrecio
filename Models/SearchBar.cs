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
        public List<Product> SearchProductName (List<Product> originList, string productName)
        {
            List<Product> productList = new List<Product>();

           // for (int i = 0; i < originList.Count(); i++)
           foreach(var product in originList)
            {

                if (product.Name == productName)
                {
                    productList.Add (product);
                }
            }
            return productList;
        }

        //This function searches products by their barCode
        public List<Product> SearchProductBarcode(List<Product> originList, int barCode)
        {
            List<Product> productList = new List<Product>();
            //search in the DB for all products that contain the string
            //received through parameter and add them to the list

            //for (int i = 0; i < originList.Count(); i++)
            foreach(var product in  originList)
            {

                if (product.Barcode == barCode)
                {
                    productList.Add(product);
                }
            }
            return productList;
        }
    }
}
