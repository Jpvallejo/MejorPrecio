using System.Collections.Generic;

namespace MejorPrecio3.Models
{
    public class ProductViewModel
    {
        public List<string> ProductsNames {get;set;}

        public string selectedProduct {get; set;}
        public decimal price{get; set;}

        public string location {get; set;}
        
    }
}