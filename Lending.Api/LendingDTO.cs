using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lending.Api
{
    public class LendingDTO
    {
        [Required]
        public Guid LenderId { get; set; }
        public Dictionary<string, string> StringParameters { get; set; }
        public Dictionary<string, double> DoubleParameters { get; set; }
        public Dictionary<string, bool> BoolParameters { get; set; }
        public string PrincipalSignature { get; set; }
    }
}
