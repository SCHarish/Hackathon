using Demo2.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Demo2.Context
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("CustomerDatabase")
        { }
        public DbSet<Customer> Customers { get; set; }

        	 protected override void OnModelCreating(DbModelBuilder modelBuilder)
	        {   
	            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();   
	        }
}
}