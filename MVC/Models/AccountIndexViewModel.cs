using System.Collections.Generic;

namespace MejorPrecio3.MVC.Models
{
    public class AccountIndexViewModel
    {
        public List<HistoryViewModel> history { get; set; }

        public string name { get; set; }

        public string email { get; set; }

        public ModifyPasswordViewModel modifyPassword { get; set; }
    }
}