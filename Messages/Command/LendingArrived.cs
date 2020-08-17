using System;
using System.Collections.Generic;
using System.Text;

namespace Messages.Command
{
    public  class LendingArrived
    {
        public Guid  LenderId { get; set; }
        public Dictionary<string,MyObject > Parameters { get; set; }
    }
    public class MyObject:Object
    {

    }
}
