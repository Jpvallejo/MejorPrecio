using System.ComponentModel.DataAnnotations;

namespace MejorPrecio3.RESTApi.Models
{
    public class ModifyPasswordViewModel
    {
        [Required]
        public string mail { get; set; }

        [Required]

        public string password {get; set;}

        [Required]

        public string confirmPassword {get; set;}
    }
}