using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mejor_precio_3.Models{
    public class Price {

        [Key]
        public int Id {get; set;}

        [ForeignKey(name: "productId")]
        public int productId {get; set;}

        public decimal price { get; set; }


        public double latitude {get; set;}

        public double longitude {get; set;}


        public bool SaveProduct(ProductViewModel model)
        {
            var geocoder = new Geocoder();
            var persistence = new ProductPersistence();
            var price = new Price{ price = model.price};
            var latLong = geocoder.GetLatLong(model.location);
            price.productId = persistence.GetProductByName(model.selectedProduct).Id;
            price.latitude = latLong.Item1;
            price.longitude = latLong.Item2;
                if (persistence.SavePrice(price))
                {   
                    return true;

                }
                return false;
        }
    }
}