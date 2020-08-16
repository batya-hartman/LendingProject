using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Lender.Api
{
    [Route("api/[controller]")]
    public class LenderController : Controller
    {
        // POST: LenderController/Create
        [HttpPost]
        public ActionResult Create(string collection)
        {

            return Ok();
        }

    }
    //public class trying
    //{
    //    public void start() {
    //        using Microsoft.Office.Interop.Excel = Microsoft.Office.Interop.Excel;
    //        Microsoft.Office.Interop.Excel.Application xlApp = new
    //        Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
    //        Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
    //        Microsoft.Office.Interop.Excel.Range range;
    //        }
    //} 
}
