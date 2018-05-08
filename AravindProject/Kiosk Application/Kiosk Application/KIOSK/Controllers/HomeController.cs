using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KIOSK.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Configuration;
namespace KIOSK.Controllers
{
    public class HomeController : Controller
    {
        string url = ConfigurationManager.AppSettings["Getmydetails"];
      
       

        public ActionResult Index1()
        {
            return View();
        }
      
        public ActionResult Sample()
        {
            return View();
        }
        
        public ActionResult splash()
        {
            return View();
        }
        public ActionResult Invalid()
        {
            return View();
        }
      
        [HttpPost]
        public ActionResult RsvpForm()
        {

            return View("Index1");
        }
        public ActionResult qrscan()
        {
            return View();
        }
        
        public ActionResult Warning()
        {
            return View();
        }
        
        public ActionResult Sample1()
        {
            return View();
        }
        public ActionResult Newhomepage()
        {
            return View();
        }
    }
}