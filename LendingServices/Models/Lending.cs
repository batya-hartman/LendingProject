using System;
using System.Collections.Generic;

namespace Lending.Services.Models
{
    public class Lending
    {
        public Guid LenderId { get; set; }
        public Dictionary<string,object> Parameters { get; set; }
    }
}
