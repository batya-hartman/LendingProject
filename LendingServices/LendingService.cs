using Lending.Services.Models;
using Messages.Command;
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
        public LendingService(ILendingRepository ilendingrepository)
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

            var parameter = Expression.Property(parameters, "Item", Expression.Constant(rules[0].Description));
            UnaryExpression kindParam;

            if (double.TryParse(rules[0].Operand, out _))
            {
                kindParam = Expression.Convert(parameter, typeof(double));
                operand = Expression.Constant(double.Parse(rules[0].Operand), typeof(double));
            }
            else if (bool.TryParse(rules[0].Operand, out _))
            {
                kindParam = Expression.Convert(parameter, typeof(bool));
                operand = Expression.Constant(bool.Parse(rules[0].Operand), typeof(bool));
            }
            else
            {
                kindParam = Expression.Convert(parameter, typeof(string));
                operand = Expression.Constant(rules[0].Operand, typeof(string));
            }
            expression = OperotorsDictionary[rules[0].ComparisonOperator](kindParam, operand);

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
        public async Task<bool> CheckLendingPassible(Models.Lending lending)
        {
            try
            {
                var parameters = Expression.Parameter(typeof(Dictionary<string, object>), "parameters");
                var expression = await BuildLenderExpression(lending, parameters);
                Expression<Func<Dictionary<string, object>, bool>> le = Expression.Lambda<Func<Dictionary<string, object>, bool>>(expression, parameters);
                Func<Dictionary<string, object>, bool> compiled = le.Compile();
                var res = compiled(lending.Parameters);
                return res;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }

        }
       
    }
    
}
