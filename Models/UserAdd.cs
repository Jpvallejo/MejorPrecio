using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace mejor_precio_3.Models
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
        Queue queue;

        public void Validate(UserAdd user)
        {
            try
            {
                var pass = user.Password;
                Regex pat = new Regex(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$");

                if (!pat.IsMatch(user.Password))
                    throw new Exception("Error en la contrasena +8, Mayus,Minus,numero");
                Regex regex = new Regex(@"(\w+)@(\w+)\.(\w+)");
                if (!regex.IsMatch(user.Mail))
                    throw new Exception("Error en el mail");
            }
            catch(Exception e)
            {
               throw new Exception(e.Message);
            }
        }
    }
}