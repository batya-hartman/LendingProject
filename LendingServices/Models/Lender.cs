using System;
using System.Collections.Generic;

namespace Lending.Services.Models
{
    public class Lender
    {
        public Guid LenderId { get; set; }
        public string Name { get; set; }
        public List<Rule> Rules { get; set; }
    }
}
