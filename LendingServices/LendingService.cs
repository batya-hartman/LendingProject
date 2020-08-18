using Lending.Services.Models;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Lending.Services
{
    public class LendingService : ILendingService
    {
        private readonly ILendingRepository _lendingRepository;
        private readonly Dictionary<string, Func<Expression, Expression, BinaryExpression>>
                  OperotorsDictionary = new Dictionary<string, Func<Expression, Expression, BinaryExpression>>();
        public LendingService(ILendingRepository ilendingrepository, IMessageSession imessageSession)
        {
            _lendingRepository = ilendingrepository;
            if (OperotorsDictionary.Count == 0)
            {
                initDictionary();
            }
        }
        private void initDictionary()
        {

            OperotorsDictionary.Add(">", (a, b) => Expression.GreaterThan(a, b));
            OperotorsDictionary.Add("<", (a, b) => Expression.LessThan(a, b));
            OperotorsDictionary.Add(">=", (a, b) => Expression.GreaterThanOrEqual(a, b));
            OperotorsDictionary.Add("<=", (a, b) => Expression.LessThanOrEqual(a, b));
            OperotorsDictionary.Add("=", (a, b) => Expression.Equal(a, b));
            OperotorsDictionary.Add("<>", (a, b) => Expression.NotEqual(a, b));
            OperotorsDictionary.Add("is", (a, b) => Expression.Equal(a, b));
            OperotorsDictionary.Add("not", (a, b) => Expression.NotEqual(a, b));
            OperotorsDictionary.Add("and", (a, b) => Expression.AndAlso(a, b));
            OperotorsDictionary.Add("or", (a, b) => Expression.OrElse(a, b));
        }
        private async Task<BinaryExpression> BuildLenderExpression(Models.Lending lending, ParameterExpression parameters)
        {
            Lender lender = await _lendingRepository.GetLenderAsync(lending.LenderId);
            if (lender.Rules != null)
            {
                if (!String.IsNullOrEmpty(lending.PrincipalSignature))
                {
                    lender.Rules.RemoveAll(rule => rule.Description == lending.PrincipalSignature);
                }
                var holeExpression = GenarateExpressionFromRules(lender.Rules, parameters);
                return holeExpression;
            }
            return null;
        }
        private BinaryExpression GenarateExpressionFromRules(List<Rule> rules, ParameterExpression parameters)
        {
            BinaryExpression expression;
            ConstantExpression operand;
            //ParameterExpression parameters = Expression.Parameter(typeof(Dictionary<string, object>), "parameters");

            if (int.TryParse(rules[0].Operand, out _))
            {
                var parameter = Expression.Property(parameters, "Item", Expression.Constant(rules[0].Description));
                var intParam = Expression.Convert(parameter, typeof(int));
                operand = Expression.Constant(Int32.Parse(rules[0].Operand), typeof(int));
                expression = OperotorsDictionary[rules[0].ComparisonOperator](intParam, operand);
            }
            else
            {
                var parameter = Expression.Property(parameters, "Item", Expression.Constant(rules[0].Description));
                var stringParam = Expression.Convert(parameter, typeof(string));
                operand = Expression.Constant(rules[0].Operand, typeof(string));
                expression = OperotorsDictionary[rules[0].ComparisonOperator](stringParam, operand);
            }
            if (rules.Count == 1)
            {
                return expression;
            }
            else
            {
                rules.RemoveAt(0);
                return OperotorsDictionary[rules[0].LogicalOperator](expression, GenarateExpressionFromRules(rules, parameters));
            }
        }
        public async Task CheckLendingPassible(Models.Lending lending)
        {

            var parameters = Expression.Parameter(typeof(Dictionary<string, object>), "parameters");
            var expression = await BuildLenderExpression(lending, parameters);
            Expression<Func<Dictionary<string, object>, bool>> le = Expression.Lambda<Func<Dictionary<string, object>, bool>>(expression, parameters);
            Func<Dictionary<string, object>, bool> compiled = le.Compile();
            var temp = new Dictionary<string, object>();
            temp.Add("salary", 520000);
            temp.Add("Credit Rating", 52000);
            temp.Add("age", 67);
            temp.Add("ggg", 5000002);
            temp.Add("state", "employee");
            var res = compiled(temp);
        }
    }
}
