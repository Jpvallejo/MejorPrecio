using System.Collections.Generic;

namespace MejorPrecio3.MVC.Models
{
    public class PriceViewModel
    {
        public List<string> ProductsNames {get;set;}

        public string selectedProduct {get; set;}
        public decimal price{get; set;}

        public string location {get; set;}
        
    }
}