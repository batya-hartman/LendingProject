using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lender.Service.Models
{
    public class Lender
    {
        public Guid LenderId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string PathToExcelFile { get; set; }
        public List<Rule> RulesList { get; set; }
    }
}