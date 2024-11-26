using System.ComponentModel.DataAnnotations;

namespace CarWarehouse.Web.ViewModels
{
    public class AuthenticateViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
