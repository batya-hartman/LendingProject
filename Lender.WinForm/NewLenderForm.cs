﻿using Lender.Service;
using System;
using System.Windows.Forms;
using Lender;

namespace Lender.WinForm
{
    public partial class NewLenderForm : Form
    {
        public NewLenderForm()
        {
            InitializeComponent();
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            openFileDialog2.Filter = "excel files only | *.xlsx";
            openFileDialog2.ShowDialog();
            Service.Models.Lender lender = new Service.Models.Lender()
            {
                Name = textBox1.Text,
                PathToExcelFile = openFileDialog2.FileName
            };
       //    Program.AddNSB();
            

           //Lender.Handler.LenderHandler lenderHandler = new Handler.();
           // lenderHandler.AddLenderAsync(lender);
        }
    }
}