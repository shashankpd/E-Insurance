using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Entity
{
    public class PolicyPurchase
    {
        public int CustomerId { get; set; }
        public int PolicyId { get; set; }
        public DateTime PurchaseDate { get; set; } 
        public int AgentId { get; set; } 
        public double AnnualIncome { get; set; } 
        public DateTime DateOfBirth { get; set; } 
        public string FirstName { get; set; } 
        public string LastName { get; set; } 
        public string Gender { get; set; } 
        public string MobileNumber { get; set; } 

        public string Address { get; set; }

    }
}
