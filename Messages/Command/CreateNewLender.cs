using System;
using System.Collections.Generic;

namespace Messages.Command
{
    public class CreateNewLender
    {
        public Guid LenderId { get; set; }
        public string Name { get; set; }
        public string PathToExcelFile { get; set; }
    }
}
