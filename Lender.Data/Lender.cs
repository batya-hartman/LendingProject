using System;
using System.Collections.Generic;

namespace LendingData
{
    public class Lender
    {
        public Guid LenderId { get; set; }
        public string Name { get; set; }
        public List<Rule> Rules { get; set; }
    }
}
