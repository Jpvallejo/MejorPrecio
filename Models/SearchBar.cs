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
        public List<PriceViewModel> SearchProductName(string productName)
        {
            var persistence = new ProductPersistence();
            var productList = new List<PriceViewModel>();

            var product = persistence.GetProductByName(productName);
            productList = persistence.GetTopFive(product);
            return productList;
        }

        //This function searches products by their barCode
        public List<PriceViewModel> SearchProductBarcode(string barcode)
        {
            var persistence = new ProductPersistence();
            List<PriceViewModel> productList = new List<PriceViewModel>();

            var product = persistence.GetProductByBarcode(barcode);
            productList = persistence.GetTopFive(product);
            return productList;
        }
    }
}