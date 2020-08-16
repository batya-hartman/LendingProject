using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LendingData
{
    public class Rule
    {
        public Guid LenderId { get; set; }
        public int Id { get; set; }
        public string Description { get; set; }
        public string Operator { get; set; }
        public string Operand { get; set; }
        public virtual Lender Lender { get; set; }
        public List<Rule> fakeRules { get; set; }
        public Rule()
        {
            fakeRules.Add(new Rule() { Description = "age", Operand = ">", Operator = "16" });
            execute(12);
        }
        public bool execute(int age)
        {
            Expression<Func<int, bool>> lambda = num => num < 5;
            // var opertor = ;
            // var operand = ;
            //fakeRules[0].
            return false;
        }
    }
    
}