using Lending.Services.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            List<Rule> copyRules = new List<Rule>();
            copyRules.AddRange(lender.Rules);
            if (!String.IsNullOrEmpty(lending.PrincipalSignature))
            {
               copyRules.RemoveAll(rule => lending.PrincipalSignature.Contains(rule.Description));
            }
            if (copyRules.Count < 1)
            {
                return null;
            }
            
            var wholeExpression = GenarateExpressionFromRules(copyRules, parameters);
            return wholeExpression;
        }
        private BinaryExpression GenarateExpressionFromRules(List<Rule> rules, ParameterExpression parameters)
        {
            IndexExpression parameter = Expression.Property(parameters, "Item", Expression.Constant(rules[0].Description));
            Type type = Type.GetType(rules[0].Type.Replace(" ", ""));
            UnaryExpression TypeConvertedParam = Expression.Convert(parameter, type);
            TypeConverter converter = TypeDescriptor.GetConverter(type);
            ConstantExpression operand = Expression.Constant(converter.ConvertFromString(rules[0].Operand), type);
            BinaryExpression expression = OperotorsDictionary[rules[0].ComparisonOperator](TypeConvertedParam, operand);

            if (rules.Count == 1)
            {
                return expression; 
            }
            else
            {
                Rule rule = rules[0];
                rules.RemoveAt(0);
                //Take delgate from the OperotorsDictionary - AndAlso / OrElse (according the LogicalOperator - and/or of the rule).
                return OperotorsDictionary[rule.LogicalOperator](expression, GenarateExpressionFromRules(rules, parameters));
            }
        }
        public async Task<bool> CheckLendingPassibleAsync(Models.Lending lending)
        {
            var parameters = Expression.Parameter(typeof(Dictionary<string, object>), "parameters");
            var expression = await BuildLenderExpression(lending, parameters);
            Expression<Func<Dictionary<string, object>, bool>> le = Expression.Lambda<Func<Dictionary<string, object>, bool>>(expression, parameters);
            Func<Dictionary<string, object>, bool> compiled = le.Compile();
            var res = compiled(lending.Parameters);
            bool success = await _lendingRepository.AddLendingToDBAsync(lending, res);
            return res && success;
        }
    }
}