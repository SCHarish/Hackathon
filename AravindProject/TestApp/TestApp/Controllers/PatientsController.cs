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
using System.Web.Http.Cors;
using System.Configuration;
using System.IO;
using System.Data.SqlClient;

namespace TestApp.Controllers
{
    public class PatientsController : ApiController
    {
        String backupfilesizelimit = ConfigurationManager.AppSettings["backupsize"];
        String backuplocation = ConfigurationManager.AppSettings["locationofthelogfile"];
        String storedprocedure = ConfigurationManager.AppSettings["storedprocedure"];
        string strcon = ConfigurationManager.ConnectionStrings["myconnection"].ConnectionString;
        Random rnd = new Random();
        private ARAVIND_DBEntities2 db = new ARAVIND_DBEntities2();
        private Entities1 db1 = new Entities1();
        // GET: api/Patients
        public IQueryable<Patient> GetPatients()
        {
            return db.Patients;
        }

        // GET: api/Patients/5
        
        [ResponseType(typeof(Patient))]
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> GetPatient(int id)
        {
            PatientsController p = new PatientsController();
            Patient patient = await db.Patients.FindAsync(id);
           
            if (patient == null)
            {
                return NotFound();
            }
          
            return Ok(patient);
        }
        
        // PUT: api/Patients/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPatient(int id, Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != patient.PIN)
            {
                return BadRequest();
            }

            db.Entry(patient).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
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
        [ResponseType(typeof(Patient))]
        [System.Web.Http.HttpGet]
        [ActionName("checkMRID")]
        public async Task<string> checkMRID(int pin)
        {
            String MRNO;
            Patient p = await db.Patients.FindAsync(pin);
            if (p.MRNO == null)
            {
                return "YOUR MRID IS NOT YET CREATED";
            }
            else
            {
                MRNO = p.MRNO;
                db.Patients.Remove(p);
                await db.SaveChangesAsync();
                return MRNO;
            }
        }
        [ResponseType(typeof(Patient))]
        [System.Web.Http.HttpGet]
        [ActionName("GetMrid")]
        public async Task<string> GetMR(int pin, string mrno)
        {
            Patient patient = await db.Patients.FindAsync(pin);
            if (!ModelState.IsValid)
            {
                return "Please enter all your details correctly";
            }
            db.Entry(patient).State = EntityState.Modified;

            try
            {
                patient.MRNO = mrno;
                await db.SaveChangesAsync();
                return patient.MRNO;
            }
            catch (Exception e)
            {
                return "MRID NOT UPDATED YET";
            }
        }
        [ResponseType(typeof(Patient))]
        [System.Web.Http.HttpGet]
        [ActionName("Checkpatient")]
        public async Task<String> checkpatient(string mrno)
        {
            SqlConnection sqlConnection1 = new SqlConnection(strcon);

            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;
            cmd.Parameters.Add("@Varmrno", SqlDbType.VarChar, 50).Value = mrno;
            cmd.CommandText = storedprocedure;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = sqlConnection1;

            sqlConnection1.Open();
            var patient_mrno = "";
            var patient_name = "";
            var fees_paid = "";
            var message = "";
            reader = cmd.ExecuteReader();
            // Data is accessible through the DataReader object here.


            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    patient_mrno = reader.GetString(0); //The 0 stands for "the 0'th column", so the first column of the result.
                    patient_name = reader.GetString(1);                     // Do somthing with this rows string, for example to put them in to a list
                    fees_paid = reader.GetString(2);
                    message = reader.GetString(3);

                }
            }
            else
            {
                return "0";
            }


            sqlConnection1.Close();
            return patient_mrno + "$" + patient_name + "$" + fees_paid + "$" + message;
        }
        [ResponseType(typeof(Patient))]
        [System.Web.Http.HttpGet]
        [ActionName("Checkmrno")]
        public async Task<String> checkmrno(string mrno)
        {
            SqlConnection sqlConnection1 = new SqlConnection(strcon);

            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;
            cmd.Parameters.Add("@qno", SqlDbType.VarChar, 50).Value = mrno;
            cmd.CommandText = storedprocedure;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = sqlConnection1;

            sqlConnection1.Open();
            var qno = 0;
            var question = "";
            reader = cmd.ExecuteReader();
            // Data is accessible through the DataReader object here.


            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    qno = reader.GetInt32(0); //The 0 stands for "the 0'th column", so the first column of the result.
                    question = reader.GetString(1);                               // Do somthing with this rows string, for example to put them in to a list

                }
            }
            else
            {
                return "0";
            }


            sqlConnection1.Close();
            return qno + "$" + question;
        }
        [ResponseType(typeof(Patient))]
        [System.Web.Http.HttpGet]
        [ActionName("Getnoofregistration")]
        public async Task<String> Getcount()
        {
            int count = 0;
            foreach (var record in db.Patients)
            {
                if (record.Dt == DateTime.Now.Date)
                {
                    count++;
                }
            }
            return count.ToString();
        }

        // POST: api/Patients
        [ResponseType(typeof(Patient))]
        [System.Web.Http.HttpGet]
        [ActionName("PostPatient")]
        public async Task<String> PostPatient([FromUri]Patient patient)
        {
            string logfile = ConfigurationManager.AppSettings["filename"];
            DateTime dt = DateTime.Now.Date;
            int count = 0;
            if (!ModelState.IsValid)
            {

                return "Please enter all your details correctly";
            }
            else
            {
                try
                {
                    db.Patients.Add(patient);
                    await db.SaveChangesAsync();
                    // var filename = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\" + logfile;
                    int randomnnumber = rnd.Next(1, 1000);
                    var filename = backuplocation + logfile;
                    var sw = new System.IO.StreamWriter(filename, true);
                    FileInfo fileinformation = new FileInfo(filename);
                    var size = fileinformation.Length;
                    long backupsize = Int32.Parse(backupfilesizelimit);
                    if (size > backupsize)
                    {
                        sw.Close();
                        // File.Delete(filename);
                        string date = DateTime.Now.ToString("yyyyMMddHHmmss");
                        string newfilename = randomnnumber + date + logfile;
                        var newfile = backuplocation + newfilename;
                        System.IO.File.Move(filename, newfile);
                        sw = new System.IO.StreamWriter(filename, true);
                    }
                    sw.WriteLine(DateTime.Now.ToString() + " " + " Server Application : OTP Generation successful. Current OTP is " + patient.PIN);

                    foreach (var record in db.Patients)
                    {
                        if (record.Dt == DateTime.Now.Date)
                        {
                            count++;
                        }
                    }
                    count++;

                    sw.WriteLine(DateTime.Now.ToString() + " " + " Server Application : Number of Registrations occured so far " + count);
                    sw.Close();
                }
                catch (Exception e)
                {

                    // var filename = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\" + logfile;
                    var filename = backuplocation + logfile;
                    int randomnnumber = rnd.Next(1, 1000);
                    var sw = new System.IO.StreamWriter(filename, true);
                    FileInfo fileinformation = new FileInfo(filename);
                    var size = fileinformation.Length;
                    long backupsize = Int32.Parse(backupfilesizelimit);
                    if (size > backupsize)
                    {
                        sw.Close();
                        //File.Delete(filename);
                        string date = DateTime.Now.ToString("yyyyMMddHHmmss");
                        string newfilename = randomnnumber + date + logfile;
                        var newfile = backuplocation + newfilename;
                        System.IO.File.Move(filename, newfile);
                        sw = new System.IO.StreamWriter(filename, true);
                    }
                    sw.WriteLine(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                    sw.Close();

                }
                return patient.PIN.ToString();
            }
        }



        // DELETE: api/Patients/5
        [ResponseType(typeof(Patient))]
        public async Task<IHttpActionResult> DeletePatient(int id)
        {
            Patient patient = await db.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            db.Patients.Remove(patient);
            await db.SaveChangesAsync();

            return Ok(patient);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PatientExists(int id)
        {
            return db.Patients.Count(e => e.PIN == id) > 0;
        }
    }
}