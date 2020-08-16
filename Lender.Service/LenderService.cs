using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Lender.Service.Models;

namespace Lender.Service
{
    public class LenderService : ILenderService
    {
        private static readonly Dictionary<string, Func<Expression, Expression, BinaryExpression>> OperotorsDictionary = new Dictionary<string, Func<Expression, Expression, BinaryExpression>>();
        private readonly ILenderRepository _LenderRepository;
        public LenderService(ILenderRepository LenderRepository)
        {
            _LenderRepository = LenderRepository;
            initDictionary();
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
        public async Task<BinaryExpression> BuildLenderExpression(Guid lenderId)
        {
            var lender = await _LenderRepository.GetLenderAsync(lenderId);
            var holeExpression = GenarateExpressionFromRules(lender.RulesList);
            return holeExpression;
            //Trying trying = new Trying(50, 8000, 1200, "employee");
            //bool pass = TryExpression(holeExpression, trying);
            //return Task.CompletedTask;
        }

        public Task<bool> AddLender(Models.Lender lender)
        {
            using (var stream = File.Open(lender.PathToExcelFile, FileMode.Open, FileAccess.Read))
            {
                lender.RulesList = new List<Rule>();
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet();
                    var dataTable = result.Tables[0];
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        Rule rule = new Rule()
                        {
                            Description = dataTable.Rows[i][0].ToString(),
                            Operator = dataTable.Rows[i][1].ToString(),
                            Operand = dataTable.Rows[i][2].ToString(),
                            KindOperator = dataTable.Rows[i][3].ToString()
                        };
                        lender.RulesList.Add(rule);
                    }
                }
            }
            return _LenderRepository.AddLenderAsync(lender);
        }
        private BinaryExpression GenarateExpressionFromRules(List<Rule> rules, int index = 0, BinaryExpression expression = null)
        {
            if (rules.Count == index)
            {
                return expression;
            }
            else
            {
                ParameterExpression paramName;
                ConstantExpression operand;
                if (int.TryParse(rules[index].Operand, out _))
                {
                    paramName = Expression.Parameter(typeof(int), Regex.Replace(rules[index].Description, @"\s+", ""));
                    operand = Expression.Constant(Int32.Parse(rules[index].Operand), typeof(int));
                    expression = OperotorsDictionary[rules[index].Operator](paramName, operand);
                }
                else
                {
                    paramName = Expression.Parameter(typeof(string), Regex.Replace(rules[index].Description, @"\s+", ""));
                    operand = Expression.Constant(rules[index].Operand, typeof(string));
                    expression = OperotorsDictionary[rules[index].Operator](paramName, operand);
                }
                return OperotorsDictionary[rules[index].KindOperator](expression, GenarateExpressionFromRules(rules, ++index, expression));
            }
        }
        //    private bool TryExpression(BinaryExpression expression, Trying obj)
        //    {
        //        try
        //        {
        //            var factorial = BinaryExpression.Lambda<Func<Trying>>(expression, new ParameterExpression[] { });
        //            Expression<Func<int, bool>> lambda1 =
        //        Expression.Lambda<Func<int, bool>>(
        //            expression,
        //            new ParameterExpression[] { });
        //            var x = lambda1.Compile()(5);
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e.Message);
        //        }
        //        return true;
        //    }
        //}
        //public class Trying
        //{
        //    public Trying(int age, int salary, int creditRating, string state)
        //    {
        //        Age = age;
        //        Salary = salary;
        //        CreditRating = creditRating;
        //        State = state;
        //    }

        //    public int Age { get; set; }
        //    public int Salary { get; set; }
        //    public int CreditRating { get; set; }
        //    public string State { get; set; }
        //}
    }
}

