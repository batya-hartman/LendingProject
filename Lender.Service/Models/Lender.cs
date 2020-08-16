using System;
using System.Collections.Generic;

namespace Lender.Service.Models
{
    public class Lender
    {
        public Guid LenderId { get; set; }
        public string Name { get; set; }
        public string PathToExcelFile { get; set; }
        public List<Rule> RulesList { get; set; }
    }
}