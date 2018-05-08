using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FeedbackAPP.Models
{
    public class User1
    {
        [Required(ErrorMessage = "Please Enter your Medical Record Number")]
        public string MRID { set; get; }
        public string pattern { get; set; }
        public string name { get; set; }
        public string ans1 { set; get; }
        public string pin { get; set; }
        public string ans2 { set; get; }
        public string ans3 { set; get; }
        public string ans4 { set; get; }
        public string ans5 { set; get; }
        public string remarks { get; set; }
    }
}