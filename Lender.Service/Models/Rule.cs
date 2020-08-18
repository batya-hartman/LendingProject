using System;

namespace Lender.Service.Models
{
    public class Rule
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string ComparisonOperator { get; set; }
        public string Operand { get; set; }
        public string LogicalOperator { get; set; }
        public Type Type { get; set; }
    }
}
