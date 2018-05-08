using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FeedbackAPP.Models
{
    public class User
    {
       
       
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
        public string reason1 { get; set; }
        public string reason2 { get; set; }
        public string reason3 { get; set; }
        public string reason4 { get; set; }
        public string reason5 { get; set; }
        public string status { get; set; }
      
        [Display(Name = "User name")]
        public string UserName { get; set; }

       
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember on this computer")]
        public bool RememberMe { get; set; }
       
    }
    
}