using System;
using System.ComponentModel.DataAnnotations;

namespace mejor_precio_3.Models
{
    public class Product
    {

        [Key]
        public int Id {get; set;}
        public string Name { get; set; }

        public string Barcode { get; set; }

        public string Brand {get; set;}

        
    }

}