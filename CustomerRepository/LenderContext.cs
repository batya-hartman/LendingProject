using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using Lender.Service.Models;

namespace Lender.Data
{
    public class LenderContext : DbContext
        {
            public DbSet<Service.Models.Lender> Lenders { get; set; }
            public DbSet<Rule> Rules { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source = ILBHARTMANLT; Initial Catalog = LenderNSB; Integrated Security = True");
            base.OnConfiguring(optionsBuilder);
           } 
        }
        public LenderContext(DbContextOptions<LenderContext> options)
       : base(options)
            { }
            public LenderContext()
            { }
        }
    }


