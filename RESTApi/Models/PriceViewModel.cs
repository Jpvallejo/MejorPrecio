namespace MejorPrecio3.Models
{
    public class PriceViewModel
    {
        public string productName {get; set;}

        public string productBarcode {get; set;}

        public string productBrand {get; set;}

        public double latitude {get; set;}

        public double longitude {get; set;}

        public string adress {get; set;}
        
        public decimal price {get; set;}
    }
}