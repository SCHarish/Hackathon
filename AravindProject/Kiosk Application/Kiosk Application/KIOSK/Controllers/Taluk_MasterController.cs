using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using KIOSK.Models;

namespace KIOSK.Controllers
{
    public class Taluk_MasterController : ApiController
    {
        private Entities3 db = new Entities3();
        private Entities4 db1 = new Entities4();
        private Entities5 db2 = new Entities5();
        private Entities6 db3 = new Entities6();
        // GET: api/Taluk_Master
       
          [ResponseType(typeof(Taluk_Master))]
       [System.Web.Http.HttpGet]
       [ActionName("Getdetails")]
       public async Task<String> GetTable(string id)
       {
           String taluk, district, state,country;
            String district_code, state_code, country_code;
           Taluk_Master table = await db.Taluk_Master.FindAsync(id);
           taluk = table.TALUK_NAME;
           District_Master table1 = await db1.District_Master.FindAsync(table.DISTRICT_CODE);
           district = table1.DISTRICT_NAME;
            district_code = table1.DISTRICT_CODE;
           State_Master table2 = await db2.State_Master.FindAsync(table1.STATE_CODE);
           state = table2.STATE_NAME;
            state_code = table2.STATE_CODE;
            Country_Master table3 = await db3.Country_Master.FindAsync(table2.COUNTRY_CODE);
            country = table3.COUNTRY_NAME;
            country_code = table3.COUNTRY_CODE;
           if (table == null)
           {
               return "Not found";
           }

           return district+"$"+state+"$"+country;
       }

        // GET: api/Taluk_Master/5
        
    }
}