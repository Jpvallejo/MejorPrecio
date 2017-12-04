namespace mejor_precio_3.Models
{
    public class Product
    {
        public string Name {get; set;}

        public string Barcode {get; set;}

        public decimal Price {get; set;}

        public string Location {get; set;}

        public bool CreateProduct(Product prod){
            return true;
        }  
        public void Delete(){
            
        }
    }
    
}