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
using System.Configuration;
using System.IO;
using System.Net.Mail;

namespace TestApp.Controllers
{
    public class FeedConfigsController : ApiController
    {
        private ARAVIND_DBEntities1 db = new ARAVIND_DBEntities1();
        private Entities1 db1 = new Entities1();
        String url = ConfigurationManager.AppSettings["getmatrix"];
        Int64 flag1 = 0;
        [ResponseType(typeof(Patient))]
        [System.Web.Http.HttpGet]
        [ActionName("writefeedback")]
        public async Task<String> writefeedback()
        {
            int count = 0;
            int flag = 0, pos = 0;
            int totalhappy = 0, totalsatisfied = 0, totalunhappy = 0;
            foreach (var record in db.FeedConfigs)
            {
                count++;
            }
            WebRequest wrGETURL;

            wrGETURL = WebRequest.Create(url);

            Stream objStream;
            objStream = wrGETURL.GetResponse().GetResponseStream();
            StreamReader objReader = new StreamReader(objStream);
            String matrix = objReader.ReadLine();
            string[] temp = matrix.Split(']');
            temp[0] = temp[0].Replace("[", "");
            string[] happyforeachquestion = temp[0].Split(',');

            temp[1] = temp[1].Replace("[", "");
            temp[1].Insert(0, "");
            string[] satisfiedforeachquestion = temp[1].Split(',');

            temp[2] = temp[2].Replace("[", "");
            temp[2].Insert(0, "");

            string[] unhappyforeachquestion = temp[2].Split(',');

            for (int i = 0; i < count; i++)
            {
                totalhappy = totalhappy + Int32.Parse(happyforeachquestion[i]);
            }
            for (int i = 1; i <= count; i++)
            {
                totalsatisfied = totalsatisfied + Int32.Parse(satisfiedforeachquestion[i]);
            }
            for (int i = 1; i <= count; i++)
            {
                totalunhappy = totalunhappy + Int32.Parse(unhappyforeachquestion[i]);
            }

            FeedConfigsController p = new FeedConfigsController();
            //Patient patient = await db.Patients.FindAsync(id);
            DataTable employeeTable = new DataTable("Employee");
            employeeTable.Columns.Add("SNO");
            employeeTable.Columns.Add("MRNO");
            employeeTable.Columns.Add("PATIENT FEEDS");
            employeeTable.Columns.Add("REMARKS");
            employeeTable.Columns.Add("DATE");

            DataTable stat = new DataTable("Total");
            stat.Columns.Add("QUESTIONS");
            stat.Columns.Add("STATUS");
            stat.Columns.Add("COUNT");

            //stat.Columns.Add("TOTAL SATISFIED");
            //stat.Columns.Add("TOTAL UNHAPPY");
            //stat.Rows.Add(totalhappy,totalsatisfied,totalunhappy);
            int tcount = 0;
            foreach (var record in db.FeedConfigs)
            {
                stat.Rows.Add(record.Question,"HAPPY", happyforeachquestion[tcount]);
                tcount++;
            }
            tcount = 1;
            foreach (var record in db.FeedConfigs)
            {
                stat.Rows.Add( record.Question,"SATISFIED", satisfiedforeachquestion[tcount]);
                tcount++;
            }
            tcount = 1;
            foreach (var record in db.FeedConfigs)
            {
                stat.Rows.Add(record.Question,"UNHAPPY", unhappyforeachquestion[tcount]);
                tcount++;
            }
            /*foreach (var record in happyforeachquestion)
            {
                stat.Rows.Add("",record);
            }*/
            foreach (var pin in db1.Feeds)
            {
                employeeTable.Rows.Add(pin.sno, pin.MRNO, pin.Patient_feeds, pin.Remarks, pin.Dt);
            }

            //Create a Department Table
            /*  DataTable departmentTable = new DataTable("Department");
              departmentTable.Columns.Add("Department ID");
              departmentTable.Columns.Add("Department Name");
              departmentTable.Rows.Add("1", "IT");
              departmentTable.Rows.Add("2", "HR");
              departmentTable.Rows.Add("3", "Finance");*/

            //Create a DataSet with the existing DataTables
            DataSet ds = new DataSet("Organization");
            ds.Tables.Add(employeeTable);
            ds.Tables.Add(stat);
            // ds.Tables.Add(departmentTable);

            p.ExportDataSetToExcel(ds);
            return "success";

        }
        private void ExportDataSetToExcel(DataSet ds)
        {
            //Creae an Excel application instance
            Excel.Application excelApp = new Excel.Application();
            //string date = DateTime.Now.ToString();
           string date = DateTime.Now.ToString("yyyy-mm-dd");
            string loc = ConfigurationManager.AppSettings["location"];
            string path = date.Trim();

            string myPath = @loc + date + ".xls";
            //Create an Excel workbook instance and open it from the predefined location
          //  Excel.Workbook excel1 = excelApp.Workbooks.Add(myPath);
            Excel.Workbook excelWorkBook = excelApp.Workbooks.Add();
            
            foreach (DataTable table in ds.Tables)
            {
                //Add a new worksheet to workbook with the Datatable name
                Excel.Worksheet excelWorkSheet = excelWorkBook.Sheets.Add();
                excelWorkSheet.Name = table.TableName;

                for (int i = 1; i < table.Columns.Count + 1; i++)
                {
                    excelWorkSheet.Cells[1, i] = table.Columns[i - 1].ColumnName;
                }

                for (int j = 0; j < table.Rows.Count; j++)
                {
                    for (int k = 0; k < table.Columns.Count; k++)
                    {
                        excelWorkSheet.Cells[j + 2, k + 1] = table.Rows[j].ItemArray[k].ToString();
                    }
                }
            }

            excelWorkBook.SaveAs(myPath);
            excelWorkBook.Close();
            excelApp.Quit();

        }

        // GET: api/FeedConfigs
        [System.Web.Http.HttpGet]
        [ActionName("Getallquestions")]
        public IQueryable<FeedConfig> GetFeedConfigs()
        {
          /*  MailMessage mail = new MailMessage();

               
                mail.From = new MailAddress("harish.sc@honeywell.com");
                mail.To.Add("gokul.r@honeywell.com");
                mail.Subject = "Test Mail - 1";
                mail.Body = "mail with attachment";

            SmtpClient smtpClient = new SmtpClient("smtp-mail.outlook.com")
            {
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential("harish.sc@honeywell.com", "H0neywell"),
                Port = 25,
                EnableSsl = true,
            };
            smtpClient.Send(mail);
            mail.Dispose();
            smtpClient.Dispose();*/
            return db.FeedConfigs;
        }
        [System.Web.Http.HttpGet]
        [ActionName("Getallquestionsfrombackup")]
        public List<string> GetFeedConfigsfrombackup(int flag)
        {
            
            List<string> questions = new List<string>();
            foreach(var record in db.FeedsConfig_Backup)
            {
                if(record.Flag==flag)
                {
                    questions.Add(record.Question);
                }
            }

            return questions;
        }
        [System.Web.Http.HttpGet]
        [ActionName("getnoofquestions")]
        public async Task<string>getnoofquestions()
        {
            int count = 0;
            foreach(var record in db.FeedConfigs)
            {
                count++;
            }
            return count.ToString();
        }

        [System.Web.Http.HttpGet]
        [ResponseType(typeof(FeedConfig))]
        [ActionName("deletequestionexport")]
        public async Task<String> deletequestionexport(int id)
        {
            int totalhappy = 0, totalsatisfied = 0, totalunhappy = 0;
            string url = ConfigurationManager.AppSettings["exportfeedback"];
            string url1 = ConfigurationManager.AppSettings["getmatrix"];
            int pos = 1;
            int count = 0;
            int temp;
            string url2 = ConfigurationManager.AppSettings["Backup"];
            WebRequest wrGETURL1;
            url2 = url2 + "d";
            wrGETURL1 = WebRequest.Create(url2);

            Stream objStream1;
            objStream1 = wrGETURL1.GetResponse().GetResponseStream();
            StreamReader objReader1 = new StreamReader(objStream1);
            String response = objReader1.ReadLine();
            Console.Write(response);
            /*
            WebRequest wrGETURL;

            wrGETURL = WebRequest.Create(url1);

            Stream objStream;
            objStream = wrGETURL.GetResponse().GetResponseStream();
            StreamReader objReader = new StreamReader(objStream);
            String matrix = objReader.ReadLine();
            string[] temparray = matrix.Split(']');
            temparray[0] = temparray[0].Replace("[", "");
            string[] happyforeachquestion = temparray[0].Split(',');

            temparray[1] = temparray[1].Replace("[", "");
            temparray[1].Insert(0, "");
            string[] satisfiedforeachquestion = temparray[1].Split(',');

            temparray[2] = temparray[2].Replace("[", "");
            temparray[2].Insert(0, "");

            string[] unhappyforeachquestion = temparray[2].Split(',');

            for (int i = 0; i < count; i++)
            {
                totalhappy = totalhappy + Int32.Parse(happyforeachquestion[i]);
            }
            for (int i = 1; i <= count; i++)
            {
                totalsatisfied = totalsatisfied + Int32.Parse(satisfiedforeachquestion[i]);
            }
            for (int i = 1; i <= count; i++)
            {
                totalunhappy = totalunhappy + Int32.Parse(unhappyforeachquestion[i]);
            }
           DataTable stat = new DataTable("Total");
            stat.Columns.Add("QUESTIONS");
            stat.Columns.Add("STATUS");
            stat.Columns.Add("COUNT");

            //stat.Columns.Add("TOTAL SATISFIED");
            //stat.Columns.Add("TOTAL UNHAPPY");
            //stat.Rows.Add(totalhappy,totalsatisfied,totalunhappy);
            int tcount = 0;
            foreach (var record in db.FeedConfigs)
            {
                stat.Rows.Add(record.Question, "HAPPY", happyforeachquestion[tcount]);
                tcount++;
            }
            tcount = 1;
            foreach (var record in db.FeedConfigs)
            {
                stat.Rows.Add(record.Question, "SATISFIED", satisfiedforeachquestion[tcount]);
                tcount++;
            }
            tcount = 1;
            foreach (var record in db.FeedConfigs)
            {
                stat.Rows.Add(record.Question, "UNHAPPY", unhappyforeachquestion[tcount]);
                tcount++;
            }
            FeedConfigsController p = new FeedConfigsController();
            //Patient patient = await db.Patients.FindAsync(id);
            DataTable employeeTable = new DataTable("Patient feedbacks");
            employeeTable.Columns.Add("SNO");
            employeeTable.Columns.Add("MRNO");
            employeeTable.Columns.Add("PATIENT FEEDS");
            employeeTable.Columns.Add("REMARKS");
            employeeTable.Columns.Add("DATE");
            foreach (var pin in db1.Feeds)
            {
                employeeTable.Rows.Add(pin.sno, pin.MRNO, pin.Patient_feeds, pin.Remarks, pin.Dt);
            }


            //Create a DataSet with the existing DataTables
            DataSet ds = new DataSet("Organization");
            ds.Tables.Add(employeeTable);
            ds.Tables.Add(stat);

            p.ExportDataSetToExcel(ds);*/
            foreach (var pin in db1.Feeds)
            {
                db1.Feeds.Remove(pin);
            }
            try
            {
                await db1.SaveChangesAsync();
            }
            catch (Exception e)
            {

            }
            return "Question deleted successfully";

        }

        [System.Web.Http.HttpGet]
        [ResponseType(typeof(FeedConfig))]
        [ActionName("Updatefeedback")]
        public async Task<String> updatefeedback(int id,String feedback,String questionno)
        {
          
            int totalhappy = 0, totalsatisfied = 0, totalunhappy = 0;
            string url = ConfigurationManager.AppSettings["exportfeedback"];
            string url1 = ConfigurationManager.AppSettings["getmatrix"];
            int pos=1;
            int count = 0;
            int temp;
            string []questions=
                new string[id];
            string url2 = ConfigurationManager.AppSettings["Backup"];
            WebRequest wrGETURL1;
            url2 = url2 + "u";
            wrGETURL1 = WebRequest.Create(url2);

            Stream objStream1;
            objStream1 = wrGETURL1.GetResponse().GetResponseStream();
            StreamReader objReader1 = new StreamReader(objStream1);
            String response = objReader1.ReadLine();
            Console.Write(response);


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
            string[] qno= questionno.Split('$');
            for(int i=0;i<id;i++)
            {
                temp = Int32.Parse(qno[i]);
                FeedConfig feedConfig = await db.FeedConfigs.FindAsync(temp);
                if (!(feedConfig.Question.Equals(questions[i])))
                {
                    feedConfig.Question = questions[i];
                    feedConfig.Dt = DateTime.Now;
                }


               
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
            //count = 0;
            /*foreach (var record in db.FeedConfigs)
            {
                count++;
            }
            WebRequest wrGETURL;

            wrGETURL = WebRequest.Create(url1);

            Stream objStream;
            objStream = wrGETURL.GetResponse().GetResponseStream();
            StreamReader objReader = new StreamReader(objStream);
            String matrix = objReader.ReadLine();
            string[] temparray = matrix.Split(']');
            temparray[0] = temparray[0].Replace("[", "");
            string[] happyforeachquestion = temparray[0].Split(',');

            temparray[1] = temparray[1].Replace("[", "");
            temparray[1].Insert(0, "");
            string[] satisfiedforeachquestion = temparray[1].Split(',');

            temparray[2] = temparray[2].Replace("[", "");
            temparray[2].Insert(0, "");

            string[] unhappyforeachquestion = temparray[2].Split(',');

            for (int i = 0; i < count; i++)
            {
                totalhappy = totalhappy + Int32.Parse(happyforeachquestion[i]);
            }
            for (int i = 1; i <= count; i++)
            {
                totalsatisfied = totalsatisfied + Int32.Parse(satisfiedforeachquestion[i]);
            }
            for (int i = 1; i <= count; i++)
            {
                totalunhappy = totalunhappy + Int32.Parse(unhappyforeachquestion[i]);
            }
            DataTable stat = new DataTable("Total");
            stat.Columns.Add("QUESTIONS");
            stat.Columns.Add("STATUS");
            stat.Columns.Add("COUNT");

            //stat.Columns.Add("TOTAL SATISFIED");
            //stat.Columns.Add("TOTAL UNHAPPY");
            //stat.Rows.Add(totalhappy,totalsatisfied,totalunhappy);
            int tcount = 0;
            foreach (var record in db.FeedConfigs)
            {
                stat.Rows.Add(record.Question, "HAPPY", happyforeachquestion[tcount]);
                tcount++;
            }
            tcount = 1;
            foreach (var record in db.FeedConfigs)
            {
                stat.Rows.Add(record.Question, "SATISFIED", satisfiedforeachquestion[tcount]);
                tcount++;
            }
            tcount = 1;
            foreach (var record in db.FeedConfigs)
            {
                stat.Rows.Add(record.Question, "UNHAPPY", unhappyforeachquestion[tcount]);
                tcount++;
            }
            FeedConfigsController p = new FeedConfigsController();
            //Patient patient = await db.Patients.FindAsync(id);
            DataTable employeeTable = new DataTable("Patient feedbacks");
            employeeTable.Columns.Add("SNO");
            employeeTable.Columns.Add("MRNO");
            employeeTable.Columns.Add("PATIENT FEEDS");
            employeeTable.Columns.Add("REMARKS");
            employeeTable.Columns.Add("DATE");
            foreach (var pin in db1.Feeds)
            {
                employeeTable.Rows.Add(pin.sno, pin.MRNO, pin.Patient_feeds, pin.Remarks, pin.Dt);
            }


            //Create a DataSet with the existing DataTables
            DataSet ds = new DataSet("Organization");
            ds.Tables.Add(employeeTable);
           ds.Tables.Add(stat);
          
            p.ExportDataSetToExcel(ds);*/
            foreach(var pin in db1.Feeds)
            {
                db1.Feeds.Remove(pin);
            }
            try
            {
                await db1.SaveChangesAsync();
            }
            catch(Exception e)
            {

            }
            return "Feedback updated successfully";
        }





        [HttpGet]
        [ActionName("BackupData")]
        public async Task<string> BackupData(string flag)
        {
            FeedConfig feed;
            string Result = "";
            int count = 1, count1 = 1;
            string questions;
            //int flag1 = 0;
            List<Feed> Ans = new List<Feed>();
            List<Feeds_BackUp> inData = new List<Feeds_BackUp>();
            List<FeedConfig> data = new List<FeedConfig>();
            List<FeedsConfig_Backup> InData1 = new List<FeedsConfig_Backup>();

            foreach (var record in db.FeedsConfig_Backup)
            {
                count1 = record.qno;
                flag1 = record.Flag.Value;
            }

            count1 = count1 + 1;

            try
            {
                using (db)
                {
                    data = db.FeedConfigs.SqlQuery("Select * from FeedConfig").ToList();


                }
                flag1++;
                // flag = data.Count.ToString() + "q" + flag;

                for (int i = 0; i < data.Count; i++)
                {
                    FeedsConfig_Backup Bdata = new FeedsConfig_Backup();

                    questions = data.ElementAt(i).Question;
                    Bdata.Question = questions;
                    Bdata.qno = count1++;
                    Bdata.Flag = flag1;
                    // DateTime d = DateTime.Now;
                   Bdata.Location = data.ElementAt(i).Location;
                   // Bdata.Dt = data.ElementAt(i).Dt;
                    InData1.Add(Bdata);

                }


            }
            catch
            {
                return Result;

            }
            for (int i = 0; i < InData1.Count; i++)
            {
                using (ARAVIND_DBEntities1 db123 = new ARAVIND_DBEntities1())
                {
                    db123.FeedsConfig_Backup.Add(InData1.ElementAt(i));
                    await db123.SaveChangesAsync();

                    //int f = db123.Database.ExecuteSqlCommand("INSERT INTO FeedsConfig_Backup ([qno],[Question],[flag],[Dt],[Location]) VALUES(@p0,@p1,@p2,@p3,@p4)", InData1.ElementAt(i).qno, InData1.ElementAt(i).Question, InData1.ElementAt(i).Flag, InData1.ElementAt(i).Dt,InData1.ElementAt(i).Location);
                    //  continue;


                }
            }
            foreach (var record in db1.Feeds_BackUp)
           {
                count = record.sno;
            }
            if (count != 1)
            {
                count = count + 1;
            }
            try
            {
                using (db1)
                {
                    Ans = db1.Feeds.SqlQuery("Select * from Feeds").ToList();

                }

                for (int i = 0; i < Ans.Count; i++)
                {
                    Feeds_BackUp f = new Feeds_BackUp();
                    f.Flag = (int)flag1;
                    f.MRNO = Ans.ElementAt(i).MRNO;
                    f.Patient_feeds = Ans.ElementAt(i).Patient_feeds;
                    f.Name = Ans.ElementAt(i).Name;
                    f.sno = 0;
                    f.Reasons = Ans.ElementAt(i).Reasons;
                    f.Remarks = Ans.ElementAt(i).Remarks;
                    f.Dt = Ans.ElementAt(i).Dt;
                    f.Location = Ans.ElementAt(i).Location;
                    inData.Add(f);


                }


            }
            catch (Exception e)
            {
                return "exception";
            }
            for (int i = 0; i < inData.Count; i++)
            {
                using (Entities1 dat123 = new Entities1())
                {

                    dat123.Feeds_BackUp.Add(inData.ElementAt(i));
                    // int f = db12.Database.ExecuteSqlCommand("INSERT INTO Feeds_BackUp([Sno],[MRNO],[Patient_feeds],[Remarks],[Dt],[Reasons],[Name] ,[Flag]) VALUES(@p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7)", inData.ElementAt(i).sno, inData.ElementAt(i).MRNO, inData.ElementAt(i).Patient_feeds, inData.ElementAt(i).Remarks, inData.ElementAt(i).Dt, inData.ElementAt(i).Reasons, inData.ElementAt(i).Name, inData.ElementAt(i).Flag);
                    await dat123.SaveChangesAsync();


                }
            }


            return "Done";
        }



























        // GET: api/FeedConfigs/5
        [ResponseType(typeof(FeedConfig))]
        public async Task<IHttpActionResult> GetFeedConfig(int id)
        {
            FeedConfig feedConfig = await db.FeedConfigs.FindAsync(id);
            if (feedConfig == null)
            {
                return NotFound();
            }

            return Ok(feedConfig);
        }
        [System.Web.Http.HttpGet]
        [ResponseType(typeof(FeedConfig))]
        [ActionName("validateuser")]
        public bool IsValid(string _username, string _password)
        {
            ARAVIND_DBEntities records = new ARAVIND_DBEntities();
            bool b = false;
            foreach (var record in records.System_Users)
            {
                //var pass = SHA1.Encode(_password);
                if ((record.Username.Equals(_username)) && (record.Password.Equals(_password)))
                {
                    b = true;
                    break;
                }
                else
                {
                    b = false;
                }

            }
            return b;
        }
        [System.Web.Http.HttpGet]
        [ResponseType(typeof(FeedConfig))]
        [ActionName("getpermissions")]
        public string getpermissions(string _username, string _password)
        {
            ARAVIND_DBEntities records = new ARAVIND_DBEntities();
         
            foreach (var record in records.System_Users)
            {
                if ((record.Username.Equals(_username)) && (record.Password.Equals(_password)))
                {
                    return record.Questions_Update + " " + record.View_Results + " " + record.New_Signup;
                }
               

            }

            return null;
        }
        [System.Web.Http.HttpGet]
        [ResponseType(typeof(FeedConfig))]
        [ActionName("Adduser")]
        public async Task<String> adduser(String username, String password,String questionsupdate,String getresults,String newsignup)
        {
            int flag = 0;
            ARAVIND_DBEntities record = new ARAVIND_DBEntities();
            System_Users user = new System_Users();
            user.Username = username;
            user.Password = SHA1.Encode(password);
            user.Questions_Update = questionsupdate;
            user.View_Results = getresults;
            user.New_Signup = newsignup;
            try
            {
                record.System_Users.Add(user);
                await record.SaveChangesAsync();
                return "1";
            }
            catch(Exception e)
            {
                return "0";
            }
        }
        [System.Web.Http.HttpGet]
        [ResponseType(typeof(System_Users))]
        [ActionName("getallusernames")]
        public IQueryable<System_Users> getallusernames()
        {
            ARAVIND_DBEntities records = new ARAVIND_DBEntities();
            return records.System_Users;
        }
        [System.Web.Http.HttpGet]
        [ResponseType(typeof(System_Users))]
        [ActionName("Removeuser")]
        public async Task<String> removeuser(String allusernames)
        {
            ARAVIND_DBEntities records = new ARAVIND_DBEntities();
          
            if (allusernames == null)
            {
                return "0";
            }
            else
            {
                string []usernames = allusernames.Split('$');
                for(int i=0;i<usernames.Length;i++)
                {
                    if (usernames[i] != null)
                    {
                        try
                        {
                            string temp = usernames[i];
                            System_Users user = await records.System_Users.FindAsync(temp);               
                                records.System_Users.Remove(user);
                            await records.SaveChangesAsync();

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
       
        // PUT: api/FeedConfigs/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutFeedConfig(int id, FeedConfig feedConfig)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != feedConfig.qno)
            {
                return BadRequest();
            }

            db.Entry(feedConfig).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeedConfigExists(id))
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

        // POST: api/FeedConfigs
        [ResponseType(typeof(FeedConfig))]
        [HttpGet]
        [ActionName("PostFeedConfig")]
        public async Task<IHttpActionResult> PostFeedConfig([FromUri]FeedConfig feedConfig)
        {
            string url2 = ConfigurationManager.AppSettings["Backup"];
            WebRequest wrGETURL1;
            url2 = url2 + "a";
            wrGETURL1 = WebRequest.Create(url2);

            Stream objStream1;
            objStream1 = wrGETURL1.GetResponse().GetResponseStream();
            StreamReader objReader1 = new StreamReader(objStream1);
            String response = objReader1.ReadLine();
            Console.Write(response);

            int count = 1;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

          foreach(var record in db.FeedConfigs)
            {
                count = record.qno;
            }
            feedConfig.qno = count+1;
            try
            {
                db.FeedConfigs.Add(feedConfig);
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
            
            }
            foreach (var pin in db1.Feeds)
            {
                db1.Feeds.Remove(pin);
            }
            try
            {
                await db1.SaveChangesAsync();
            }
            catch (Exception e)
            {

            }

            return CreatedAtRoute("DefaultApi", new { id = feedConfig.qno }, feedConfig);
        }

       













        // DELETE: api/FeedConfigs/5
        [ResponseType(typeof(FeedConfig))]
        [System.Web.Http.HttpGet]
        [ActionName("Deletequestion")]
        public async Task<String> DeleteFeedConfig(int id)
        {
            int count = 0;
            FeedConfig feedConfig = await db.FeedConfigs.FindAsync(id);
            if (feedConfig == null)
            {
                return "0";
            }

            else
            {
                db.FeedConfigs.Remove(feedConfig);
               await db.SaveChangesAsync();
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

        private bool FeedConfigExists(int id)
        {
            return db.FeedConfigs.Count(e => e.qno == id) > 0;
        }
    }
}