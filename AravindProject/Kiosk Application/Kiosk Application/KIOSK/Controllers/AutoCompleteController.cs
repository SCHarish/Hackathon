using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KIOSK.Models;
namespace KIOSK.Controllers
{
    public class AutoCompleteController : Controller
    {
      
        // GET: AutoComplete
        /*public ActionResult ProductName(string q)
        {
            DatabaseEntities db = new DatabaseEntities();
            var productNames = (from p in db.Tables where p.Name.Contains(q) select p.Name).Distinct().Take(2);
            string content = string.Join<string>("\n", productNames);
            return Content(content);
        }
        public JsonResult AutocompleteSuggestions(string searchstring)
        {
            DatabaseEntities db = new DatabaseEntities();

            var suggestions = from s in db.Tables
                          select s.Name;
        var namelist = suggestions.Where(n => n.ToLower().StartsWith(searchstring.ToLower()));

    return Json(namelist, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult RemoteData(string query)
        {
            DatabaseEntities db = new DatabaseEntities();
            List<string> listData = null;
            List<string> authorNames = new List<string>();
            //checking the query parameter sent from view. If it is null we will return null else we will return list based on query.
            if (!string.IsNullOrEmpty(query))
            {
                foreach(var record in db.Tables)
                {
                    authorNames.Add(record.Name);
                }
                //Created an array of players. We can fetch this from database as well.
              // string[] arrayData = new string[] { "Fabregas", "Messi", "Ronaldo", "Ronaldinho", "Goetze", "Cazorla", "Henry", "Luiz", "Reus", "Neur", "Podolski" };

                //Using Linq to query the result from an array matching letter entered in textbox.
                listData = authorNames.Where(q => q.ToLower().Contains(query.ToLower())).ToList();
            }

            //Returning the matched list as json data.
            return Json(new { Data = listData });
        }*/

    }
}