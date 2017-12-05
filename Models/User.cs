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
        Queue queue;

        public string Add(User user)
        {
            var pass = user.Password;
            Regex pat = new Regex(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$");

            if (!pat.IsMatch(user.Password))
                return "Error en la contrasena +8, Mayus,Minus,numero";
            Regex regex = new Regex(@"(\w+)@(\w+)\.(\w+)");
            if (!regex.IsMatch(user.Mail))
                return "Error en el mail";

            return "OK";



        }
    }
}