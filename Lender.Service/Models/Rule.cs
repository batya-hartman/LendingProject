namespace Lender.Service.Models
{
    public class Rule
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Operator { get; set; }
        public string Operand { get; set; }
        public string KindOperator { get; set; }
    }
}
