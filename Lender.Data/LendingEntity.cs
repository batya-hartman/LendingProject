using Lending.Services.Models;
using System;
using System.Collections.Generic;

namespace Lendings.Data
{
    public class LendingEntity
    {
        public int Id { get; set; }
        public Guid LenderId { get; set; }
        public List<Parameter> Parameters { get; set; }
        public string PrincipalSignature { get; set; }
        public bool Confirmed { get; set; }
        public virtual Lender Lender { get; set; }
    }
    public class Parameter
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public virtual LendingEntity LendingEntity { get; set; }
    }
}