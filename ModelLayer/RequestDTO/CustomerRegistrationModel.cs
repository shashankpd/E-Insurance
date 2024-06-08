﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.RequestDTO
{
    public class CustomerRegistrationModel
    {
        public string Name { get; set; }  
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }  
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        //public int Age { get; set; }   
       // public string Address { get; set; }
       // public int? AgentId { get; set; }  
    }
}
