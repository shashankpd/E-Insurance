using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Entity
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DateOfBirth { get; set; } // If needed
        public string? Address { get; set; } // Add this property
<<<<<<< HEAD
=======

        public int CustomerId { get; set; }  // Ensure this property exists

>>>>>>> c8ef75a48d6d0f2109f56342f5abb787d9323a7f
    }
}
