using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace mejor_precio_3.Models
{
    public class User
    {
        public string name { get; set; }
        public string mail { get; set; }
         [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }
        public int age { get; set; }
        public string gender { get; set; }


        public HttpStatusCode Create (User user)
        {            

            return HttpStatusCode.OK;
            
        }
    }
}