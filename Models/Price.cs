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

        public string location { get; set; }
    }
}