using ExcelDataReader;
using Lender.Service.Models;
using Messages.Command;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Lender.Service
{
    public class LenderService : ILenderService
    {
        private readonly ILenderRepository _LenderRepository;
    //    private readonly IMessageSession _messageSession;

        public LenderService(ILenderRepository LenderRepository
            //, IMessageSession imessageSession
            )
        {
            _LenderRepository = LenderRepository;
          //  _messageSession = imessageSession;
        }

        public async Task<bool> AddLenderAsync(Models.Lender lender)
        {
            lender.RulesList= ReadFromExcel(lender.PathToExcelFile);
            return await _LenderRepository.AddLenderAsync(lender);
        }
        public async Task<bool> EditLenderAsync(Models.Lender lender)
        {
            lender.RulesList = ReadFromExcel(lender.PathToExcelFile);
            return await _LenderRepository.EditLenderRulesAsync(lender);
        }
        private List<Rule> ReadFromExcel(string pathToExcelFile)
        { 
            var rulesList = new List<Rule>();
            using (var stream = File.Open(pathToExcelFile, FileMode.Open, FileAccess.Read))
            {
               
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
                            ComparisonOperator = dataTable.Rows[i][1].ToString(),
                            Operand = dataTable.Rows[i][2].ToString(),
                            LogicalOperator = dataTable.Rows[i][3].ToString()
                        };
                        rule.Type = AddTypeToRule(rule.Operand);
                        rulesList.Add(rule);
                    }
                }
            }
            return rulesList;
        }

        private string AddTypeToRule(string operand)
        {
            if (double.TryParse(operand, out _))
                return "System.Double";
            if (bool.TryParse(operand, out _))
                return "System.boolean";
            if (DateTime.TryParse(operand, out _))
                return "System.DateTime";
            return "System.String";
        }
    }
}

