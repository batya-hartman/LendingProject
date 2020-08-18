using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lending.Services.Models
{
    public class Lending
    {
        public Guid LenderId { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
        public Dictionary<string, string> StringParameters { get; set; }
        public Dictionary<string, double> doubleParameters { get; set; }
        public Dictionary<string, bool> BoolParameters { get; set; }
        public string PrincipalSignature { get; set; }
    }
}
