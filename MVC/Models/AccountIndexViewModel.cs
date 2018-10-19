using System.Collections.Generic;
using MejorPrecio3.Models;

namespace MejorPrecio3.MVC.Models
{
    public class AccountIndexViewModel
    {
        public List<History> history { get; set; }

        public string name { get; set; }

        public string email { get; set; }

        public ModifyPasswordViewModel modifyPassword { get; set; }
    }
}