﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.RequestDTO
{
    public class PremiumCalculationRequest
    {
        public int PolicyId { get; set; }
        public int CustomerAge { get; set; }
        public decimal CoverageAmount { get; set; }
        public string PolicyType { get; set; }
        public  string paymentFrequency { get; set; }
        public  int TermYears { get; set; }
    }
}
