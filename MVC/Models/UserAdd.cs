using System;
using System.ComponentModel.DataAnnotations;

namespace MejorPrecio3.MVC.Models
{
    public class UserAdd
    {
        public string Name { get; set; }
        public string Mail { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public String Password { get; set; }
        public int Age { get; set; }
        public char Gender { get; set; }

        
        }
    }
