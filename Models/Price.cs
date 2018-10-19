using System;

namespace MejorPrecio3.Models
{
    public class Price {
        public Guid Id {get; set;}

        public Product product {get; set;}

        public decimal price { get; set; }


        public double latitude {get; set;}

        public double longitude {get; set;}


        
    }
}