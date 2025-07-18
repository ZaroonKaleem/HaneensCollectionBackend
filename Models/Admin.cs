// Models/Admin.cs
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace HaneensCollection.Models
{
    public class Admin : IdentityUser
    {
        [Required]
        public string FullName { get; set; }

        // You can add more admin-specific properties here
    }
}   