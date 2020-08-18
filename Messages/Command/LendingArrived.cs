using System;
using System.Collections.Generic;

namespace Messages.Command
{
    public class LendingArrived
    {
        public Guid LenderId { get; set; }
        public Dictionary<string, string> StringParameters { get; set; }
        public Dictionary<string, double> doubleParameters { get; set; }
        public Dictionary<string, bool> BoolParameters { get; set; }
        public string PrincipalSignature { get; set; }
    }
}
