using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.RequestDTO
{
    public class InsuranceAgentRegistrationModel
    {
        public string Name { get; set; }  // Corrected to match the table schema
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }  // Added to match the table schema
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
