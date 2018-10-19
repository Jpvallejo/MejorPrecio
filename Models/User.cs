using System;
using System.Collections;

namespace MejorPrecio3.Models
{
    public class User
    {
        public string Name { get; set; }
        public string Mail { get; set; }

        public string Password { get; set; }
        public int Age { get; set; }
        public char Gender { get; set; }
        public Guid Id { get; set; }
        public bool Verified { get; set; }
        public string Role { get; set; }
        public Queue Hisory { get; set; }
        
    }
}