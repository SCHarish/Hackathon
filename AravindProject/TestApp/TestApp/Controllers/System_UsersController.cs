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
    public class System_UsersController : ApiController
    {
        private ARAVIND_DBEntities db = new ARAVIND_DBEntities();

        // GET: api/System_Users
        public IQueryable<System_Users> GetSystem_Users()
        {
            return db.System_Users;
        }

        // GET: api/System_Users/5
        [ResponseType(typeof(System_Users))]
        public async Task<IHttpActionResult> GetSystem_Users(string id)
        {
            System_Users system_Users = await db.System_Users.FindAsync(id);
            if (system_Users == null)
            {
                return NotFound();
            }

            return Ok(system_Users);
        }

        // PUT: api/System_Users/5
        [ResponseType(typeof(String))]
        [HttpGet]
        [ActionName("PutSystem_Users")]
        public async Task<IHttpActionResult> PutSystem_Users([FromUri]System_Users system_Users)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

           
            db.Entry(system_Users).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!System_UsersExists(system_Users.Username))
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

        // POST: api/System_Users
        [ResponseType(typeof(System_Users))]
        public async Task<IHttpActionResult> PostSystem_Users(System_Users system_Users)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.System_Users.Add(system_Users);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (System_UsersExists(system_Users.Username))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = system_Users.Username }, system_Users);
        }

        // DELETE: api/System_Users/5

        [System.Web.Http.HttpGet]
        [ResponseType(typeof(System_Users))]
        [ActionName("Removeuser")]
        public async Task<String> DeleteSystem_Users(string allusernames)
        {
            if (allusernames == null)
            {
                return "0";
            }
            else
            {
                string[] usernames = allusernames.Split('$');
                for (int i = 0; i < usernames.Length; i++)
                {
                    if (usernames[i] != null)
                    {
                        try
                        {
                            string temp = usernames[i];

                            System_Users user = await db.System_Users.FindAsync(temp);
                            db.System_Users.Remove(user);
                            await db.SaveChangesAsync();
                        }
                        catch (Exception e)
                        {
                            return "1";
                        }
                    }
                }
                return "1";
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool System_UsersExists(string id)
        {
            return db.System_Users.Count(e => e.Username == id) > 0;
        }
    }
}