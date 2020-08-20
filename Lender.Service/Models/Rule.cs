using System;

namespace Lender.Service.Models
{
    public class Rule
    {
        public Guid LenderId { get; set; }
        public int Id { get; set; }
        public string Description { get; set; }
        public string ComparisonOperator { get; set; }
        public string Operand { get; set; }
        public string LogicalOperator { get; set; }
        public virtual Lender Lender { get; set; }
        public string Type { get; set; }
    }
}
