using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Entity
{
    public class PolicyCreation
    {
        public string PolicyName { get; set; }
        public string Description { get; set; }
        public string PolicyType { get; set; }
        public int TermLength { get; set; }
        public double CoverageAmount { get; set; }
        public double Premium { get; set; }
        public int EntryAge { get; set; }

        
       // public string Status { get; set; }


    }
}
