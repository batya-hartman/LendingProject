using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lending.Api
{
    public class LendingDTO
    {
        public Guid LenderId { get; set; }
        public Dictionary<string, string> StringParameters { get; set; }
        public Dictionary<string, double> doubleParameters { get; set; }
        public Dictionary<string, bool> BoolParameters { get; set; }
        public string PrincipalSignature { get; set; }
    }
}
