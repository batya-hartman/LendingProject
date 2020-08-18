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
                            ComparisonOperator = dataTable.Rows[i][1].ToString(),
                            Operand = dataTable.Rows[i][2].ToString(),
                            LogicalOperator = dataTable.Rows[i][3].ToString()
                        };
                        lender.RulesList.Add(rule);
                    }
                }
            }
            return _LenderRepository.AddLenderAsync(lender);
        }
      
    }
}

