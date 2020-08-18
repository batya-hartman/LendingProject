using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lending.Services.Models
{
    public class Lending
    {
        [Required]
        public Guid LenderId { get; set; }
        [Required]
        public Dictionary<string,object> Parameters { get; set; }
        public string PrincipalSignature { get; set; }
    }
}
