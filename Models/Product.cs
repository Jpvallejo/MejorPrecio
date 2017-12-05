namespace mejor_precio_3.Models
{
    public class Product
    {
        public string Name {get; set;}

        public int Barcode {get; set;}

        public decimal Price {get; set;}

        public string Location {get; set;}

        public bool SaveProduct(){
            return true;
        }  
        public void Delete(){
            
        }
    }
    
}