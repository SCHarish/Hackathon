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
using TestApp.Models;

namespace TestApp.Controllers
{
    public class ZonesController : ApiController
    {
        private Entities3 db = new Entities3();

        // GET: api/Zones
        [ResponseType(typeof(Zone))]
        [System.Web.Http.HttpGet]
        [ActionName("getallzones")]
        public IQueryable<Zone> GetZones()
        {
            return db.Zones;
        }

        // GET: api/Zones/5
       
        public async Task<IHttpActionResult> GetZone(int id)
        {
            Zone zone = await db.Zones.FindAsync(id);
            if (zone == null)
            {
                return NotFound();
            }

            return Ok(zone);
        }

        // PUT: api/Zones/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutZone(int id, Zone zone)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != zone.Zone_ID)
            {
                return BadRequest();
            }

            db.Entry(zone).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ZoneExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Zones
        [ResponseType(typeof(Zone))]
        [System.Web.Http.HttpGet]
        [ActionName("AddZone")]
        public async Task<String> PostZone(string zonename)
        {
            Zone z = new Zone();
            z.Zone_Name = zonename;
            z.Zone_ID = 0;
            db.Zones.Add(z);
            await db.SaveChangesAsync();
            return "1"; 
        }

        // DELETE: api/Zones/5
        [ResponseType(typeof(Zone))]
        [System.Web.Http.HttpGet]
        [ActionName("Deletezone")]
        public async Task<String> DeleteZone(int id)
        {
            Zone zone = await db.Zones.FindAsync(id);
            if (zone == null)
            {
                return "0";
            }

            db.Zones.Remove(zone);
            await db.SaveChangesAsync();

            return "1";
        }
        [System.Web.Http.HttpGet]
        [ResponseType(typeof(FeedConfig))]
        [ActionName("Updatezones")]
        public async Task<String> Updatezones(int id, String feedback, String questionno)
        {
            int pos = 1;
            int count = 0;
            int temp;
            string[] questions =
                new string[id];
            for (int i = 0; i < feedback.Length - 1; i++)
            {
                if (feedback[i] != '$')
                {
                    questions[count] = questions[count] + feedback[i];
                }
                else
                {
                    count++;
                }
            }
            string[] qno = questionno.Split('$');
            for (int i = 0; i < id; i++)
            {
                temp = Int32.Parse(qno[i]);
                Zone feedConfig = await db.Zones.FindAsync(temp);
                feedConfig.Zone_Name = questions[i];
                db.Entry(feedConfig).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                    pos++;
                }
                catch (DbUpdateConcurrencyException)
                {
                    return "error";
                }
            }
                return "1";
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ZoneExists(int id)
        {
            return db.Zones.Count(e => e.Zone_ID == id) > 0;
        }
    }
}