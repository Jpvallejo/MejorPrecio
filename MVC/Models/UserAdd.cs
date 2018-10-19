using System;
using System.ComponentModel.DataAnnotations;

namespace MejorPrecio3.MVC.Models
{
    public class UserAdd
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Mail { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public String Password { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public char Gender { get; set; }

        public bool captcha {get; set;}

        
        }
    }
