using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Demo2.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DoB { get; set; }
        public string Gender { get; set; }
        public int Flag { get; set; }
    }
}