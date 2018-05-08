using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KIOSK.Models
{
    public class OTP
    {
        [Required(ErrorMessage = "Please enter your pin")]
        [DataType(DataType.PostalCode)]
        [RegularExpression("^[0-9]*$",
        ErrorMessage = "Please enter a valid 4 digit OTP number")]
        public string pin { get; set; }
        public string pattern { get; set; }
        public string name { get; set; }
        public int age { get; set; }
        public string gender { get; set; }
        public DateTime dob { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string district { get; set; }
        public string state { get; set; }
        public string pincode { get; set; }
        public string mobileno { get; set; }
        public string landlineno { get; set; }
        public string visit { get;set; }
        public string country { get; set; }
        public DateTime dt { get; set; }
        public string MRNO { get; set; }
    }
}