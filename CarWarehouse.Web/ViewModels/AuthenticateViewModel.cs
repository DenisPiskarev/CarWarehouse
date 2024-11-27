using System.ComponentModel.DataAnnotations;
using CarWarehouse.DAL.Models;
using System.Text.Json.Serialization;

namespace CarWarehouse.Web.ViewModels
{
    public class AuthenticateViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
