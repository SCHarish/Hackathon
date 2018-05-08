using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace KIOSK.Models
{
    public class form
    {
        [Required(ErrorMessage = "* Please enter your name")]
        [MaxLength(30,ErrorMessage ="Please enter less than 30 characters")]
        [MinLength(3,ErrorMessage ="Please enter atleast 3 characters")]
        [RegularExpression(@"^[a-zA-Z\s\.]{1,}[\.]{0,1}[A-Za-z\s]{0,}$", ErrorMessage = "Use letters only please")]
        public string name { get; set; }//Required
        [RegularExpression("^[0-9]*$",
        ErrorMessage = "* Please enter a valid age")]
        [MaxLength(3, ErrorMessage = "Please enter less than 3 digits")]
        [Required(ErrorMessage = "* Please enter your age")]
        public string age { get; set; }
        [Required(ErrorMessage = "* Please select your sex")]
        public string sex { get; set; }//Required
        [Required(ErrorMessage = "Please select your date of birth")]
        public DateTime dob { get; set; }      
        [RegularExpression(".+\\@.+\\..+",
        ErrorMessage = "* Please enter a valid email address")]
      [MaxLength(30, ErrorMessage = "Please enter less than 30 characters")]
        public string email { get; set; }
        [Required(ErrorMessage = "* Please enter your street")]
        [MaxLength(50, ErrorMessage = "Please enter less than 50 characters")]
        [RegularExpression(@"^[0-9a-zA-Z\s\#\,\.\-/\\]{1,}[\.]{0,1}[A-Za-z\s]{0,}$", ErrorMessage = "Please enter address in correct format")]
        public string street { get; set; }//Required
        [Required(ErrorMessage = "* Please enter your city")]
        [MaxLength(25, ErrorMessage = "Please enter less than 25 characters")]
        [RegularExpression(@"^[a-zA-Z\s\.\,]{1,}[\.]{0,1}[A-Za-z\s]{0,}$", ErrorMessage = "Use letters only please")]
        public string city { get; set; }//Required
        [Required(ErrorMessage = "* Please enter your district")]
        public string district { get; set; }//Required
        [Required(ErrorMessage = "* Please enter your state")]
        public string state { get; set; }//Required
      //  [Required(ErrorMessage = "* Please enter your pin code")]
        [MaxLength(10, ErrorMessage = "Please enter less than 10 digits")]
        [MinLength(6, ErrorMessage = "Please enter atleast 6 digits")]
        [RegularExpression("^[0-9]*$",
        ErrorMessage = "* Please enter a valid pin code")]
        public string pin { get; set; }//Required
        [RegularExpression("^[0-9+-]*$",
        ErrorMessage = "* Please enter a valid mobile number")]
        [MaxLength(30, ErrorMessage = "Please enter less than 30 digits")]
        [MinLength(10, ErrorMessage = "Please enter atleast 10 digits")]
        public string mobile { get; set; }
        [RegularExpression("^[0-9+-]*$",
        ErrorMessage = "* Please enter a valid landline number")]
            [MaxLength(30, ErrorMessage = "Please enter less than 30 digits")]
        [MinLength(6, ErrorMessage = "Please enter atleast 6 digits")]
        public string landline { get; set; }
       //[Required(ErrorMessage = "* Please enter whether you already visited or not")]
        public string alreadyVisted { get; set; }
        [Required(ErrorMessage = "* Please enter your country")]
        public string country { get; set; }//Required
        public string otp { get; set; }
        [Required(ErrorMessage ="*Select your Taluk")]
        public string taluk { get; set; }//Required
        [Required(ErrorMessage = "* Please enter next of kin")]
        [MaxLength(26, ErrorMessage = "Please enter less than 26 characters")]
        [RegularExpression(@"^[a-zA-Z\s\.]{1,}[\.]{0,1}[A-Za-z\s]{0,}$", ErrorMessage = "Use letters only please")]
        public string nextofkin { get; set; }//Required
        [Required(ErrorMessage = "* Please enter your locality")]
        [MaxLength(50, ErrorMessage = "Please enter less than 50 characters")]
        [RegularExpression(@"^[a-zA-Z\s\.\,]{1,}[\.]{0,1}[A-Za-z\s]{0,}$", ErrorMessage = "Use letters only please")]
        public string locality { get; set; }//Required
        public string relation { get; set; }
        }
    }

