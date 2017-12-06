using System;

namespace mejor_precio_3.Models
{
    [Serializable]
    public class Product
    {
        public string Name {get; set;}

        public int Barcode {get; set;}

        public decimal Price {get; set;}

        public string Location {get; set;}
    }
    
}