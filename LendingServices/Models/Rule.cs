using System;

namespace Lending.Services.Models
{
    public class Rule
    {
        public Guid LenderId { get; set; }
        public int Id { get; set; }
        public string Description { get; set; }
        public string ComparisonOperator { get; set; }
        public string Operand { get; set; }
        public virtual Lender Lender { get; set; }
        public string LogicalOperator { get; set; }
       // public Type Type {get; set; }
    }    
}