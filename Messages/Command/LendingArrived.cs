using System;
using System.Collections.Generic;
using System.Text;

namespace Messages.Command
{
    public  class LendingArrived
    {
        public Guid  LenderId { get; set; }
        public Dictionary<string,object > Parameters { get; set; }
        public string PrincipalSignature { get; set; }
    }
}
