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

        public int CustomerId { get; set; }  // Ensure this property exists
        public int AgentId { get; set; }  // Ensure this property exists
        public int AdminId { get; set; }  // Ensure this property exists
        public int EmployeeId { get; set; }  // Ensure this property exists



    }
}
