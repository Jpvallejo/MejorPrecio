using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace mejor_precio_3.Models
{
    public class User
    {
        public string Name { get; set; }
        public string Mail { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        Queue Historal;
        public int Id{get;set;}
        public bool Verified{get;set;}
        public string Role{get;set;}

    }
}