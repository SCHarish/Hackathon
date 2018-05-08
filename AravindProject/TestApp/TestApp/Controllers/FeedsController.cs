using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using TestApp.Models;

namespace TestApp.Controllers
{
    public class FeedsController : ApiController
    {
        Entities1 db = new Entities1();
        Random rnd = new Random();
        int[][] result = new int[3][];
        ARAVIND_DBEntities1 db1 = new ARAVIND_DBEntities1();
        String allrecords = ConfigurationManager.AppSettings["allrecords"];
        // GET: api/Feeds
        public IQueryable<Feed> GetFeeds()
        {
            return db.Feeds;
        }
        [ResponseType(typeof(Feed))]
        [System.Web.Http.HttpGet]
        [ActionName("Exporttoexcelsheet")]
        public async Task<String> exporttoexcelsheet(string startdate, string enddate, string rangestart, string rangeend, string zone, string pattern_number, string lookup,string location)
        {

            Feed p = new Feed();
            int totalhappy = 0, totalunhappy = 0, totalsatisfied = 0;
            int count = 0;

            DateTime start_date = Convert.ToDateTime(startdate);
            DateTime end_date = Convert.ToDateTime(enddate);
            DateTime range_start = Convert.ToDateTime(rangestart);
            DateTime range_end = Convert.ToDateTime(rangeend);
            List<Feed> sam = new List<Feed>();
            List<Feeds_BackUp> list2 = new List<Feeds_BackUp>();
            if (lookup.Equals("Feeds_Backup"))
            {
                foreach (var record in db1.FeedsConfig_Backup)
                {
                    if (record.Flag.ToString() == pattern_number)
                    {
                        count++;
                    }
                }
                int[] totalHappyForEachQuestion = new int[count];
                int[] totalSatisfiedForEachQuestion = new int[count];
                int[] totalUnHappyForEachQuestion = new int[count];
                string[] remarks = new string[count];
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = new int[count];
                }
                if (zone.Equals(allrecords))
                {
                    list2 = db.Feeds_BackUp.SqlQuery("SELECT * from Feeds_BackUp WHERE Dt between @p0 and @p1 AND Flag=@p2", start_date, end_date, pattern_number).ToList();
                }
                else
                {
                    list2 = db.Feeds_BackUp.SqlQuery("SELECT * from Feeds_BackUp WHERE Dt between @p0 and @p1 AND Flag=@p2 AND Location=@p3", start_date, end_date, pattern_number, zone).ToList();
                }

                for (int i = 0; i < list2.Count; i++)
                {
                    decode(list2.ElementAt(i).Patient_feeds, list2.ElementAt(i).MRNO, list2.ElementAt(i).Name);
                }

                for (int i = 0; i < result.Length; i++)
                {
                    if (i == 0)
                    {
                        for (int j = 0; j < result[0].Length; j++)
                        {
                            totalhappy += result[i][j];
                        }
                    }
                    if (i == 1)
                    {
                        for (int j = 0; j < result[1].Length; j++)
                        {
                            totalsatisfied += result[i][j];
                        }
                    }
                    if (i == 2)
                    {
                        for (int j = 0; j < result[2].Length; j++)
                        {
                            totalunhappy += result[i][j];
                        }
                    }
                }
                for (int i = 0; i < count; i++)
                {
                    totalHappyForEachQuestion[i] = result[0][i];
                }
                for (int i = 0; i < count; i++)
                {
                    totalSatisfiedForEachQuestion[i] = result[1][i];
                }
                for (int i = 0; i < count; i++)
                {
                    totalUnHappyForEachQuestion[i] = result[2][i];
                }
                int t = 0;

                if (zone.Equals(allrecords))
                {
                    foreach (var record in db.Feeds_BackUp)
                    {
                        if (record.Dt >= start_date && record.Dt <= end_date)
                        {
                            if (record.Flag.ToString() == pattern_number)
                            {
                                remarks[t] = record.Remarks;
                                t++;
                            }
                        }
                    }
                }
                else
                {
                    foreach (var record in db.Feeds_BackUp)
                    {
                        if (record.Dt >= start_date && record.Dt <= end_date)
                        {
                            if (record.Flag.ToString() == pattern_number)
                            {
                                if (record.Location == zone)
                                {
                                    remarks[t] = record.Remarks;
                                    t++;
                                }
                            }
                        }
                    }
                }
                DataTable total = new DataTable("Total");
                total.Columns.Add("Total Happy");
                total.Columns.Add("Total Satisfied");
                total.Columns.Add("Total Unhappy");
                total.Rows.Add(totalhappy, totalsatisfied, totalunhappy);
                DataTable stat = new DataTable("Questions");
                stat.Columns.Add("QUESTIONS");
                stat.Columns.Add("STATUS");
                stat.Columns.Add("COUNT");
                DataTable rem = new DataTable("Any other suggestions");
                rem.Columns.Add("Remarks");
                foreach (var data in remarks)
                {
                    rem.Rows.Add(data);
                }
                int counter = 0;
                foreach (var record in db1.FeedsConfig_Backup)
                {

                    if (record.Flag.ToString() == pattern_number)
                    {
                        stat.Rows.Add(record.Question, "HAPPY", totalHappyForEachQuestion[counter]);
                        stat.Rows.Add(record.Question, "SATISFIED", totalSatisfiedForEachQuestion[counter]);
                        stat.Rows.Add(record.Question, "UNHAPPY", totalUnHappyForEachQuestion[counter]);
                        counter++;
                    }

                }
                DataSet ds = new DataSet("Hospital");
                ds.Tables.Add(stat);
                ds.Tables.Add(total);
                ds.Tables.Add(rem);
                string status = ExportDataSetToExcel(ds, location);
                if (status.Equals("1"))
                    return "success";
                else
                    return "failed";
            }
            else if (lookup.Equals("Feeds"))
            {
                foreach (var record in db1.FeedConfigs)
                {
                    count++;
                }
                int[] totalHappyForEachQuestion = new int[count];
                int[] totalSatisfiedForEachQuestion = new int[count];
                int[] totalUnHappyForEachQuestion = new int[count];
                string[] remarks = new string[count];
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = new int[count];
                }
                if (zone.Equals(allrecords))
                {
                    sam = db.Feeds.SqlQuery("SELECT * from Feeds WHERE Dt between @p0 and @p1 ", start_date, end_date).ToList();
                }
                else
                {
                    sam = db.Feeds.SqlQuery("SELECT * from Feeds WHERE Dt between @p0 and @p1 AND Location=@p2", start_date, end_date, zone).ToList();
                }

                for (int i = 0; i < sam.Count; i++)
                {
                    decode(sam.ElementAt(i).Patient_feeds, sam.ElementAt(i).MRNO, sam.ElementAt(i).Name);
                }

                for (int i = 0; i < result.Length; i++)
                {
                    if (i == 0)
                    {
                        for (int j = 0; j < result[0].Length; j++)
                        {
                            totalhappy += result[i][j];
                        }
                    }
                    if (i == 1)
                    {
                        for (int j = 0; j < result[1].Length; j++)
                        {
                            totalsatisfied += result[i][j];
                        }
                    }
                    if (i == 2)
                    {
                        for (int j = 0; j < result[2].Length; j++)
                        {
                            totalunhappy += result[i][j];
                        }
                    }
                }
                for (int i = 0; i < count; i++)
                {
                    totalHappyForEachQuestion[i] = result[0][i];
                }
                for (int i = 0; i < count; i++)
                {
                    totalSatisfiedForEachQuestion[i] = result[1][i];
                }
                for (int i = 0; i < count; i++)
                {
                    totalUnHappyForEachQuestion[i] = result[2][i];
                }

                int t = 0;

                if (zone.Equals(allrecords))
                {
                    foreach (var record in db.Feeds)
                    {
                        if (record.Dt >= start_date && record.Dt <= end_date)
                        {

                            remarks[t] = record.Remarks;
                            t++;

                        }
                    }
                }
                else
                {
                    foreach (var record in db.Feeds)
                    {
                        if (record.Location.Equals(zone))
                        {
                            if (record.Dt >= start_date && record.Dt <= end_date)
                            {

                                remarks[t] = record.Remarks;
                                t++;

                            }
                        }

                    }



                    DataTable total = new DataTable("Total");
                    total.Columns.Add("Total Happy");
                    total.Columns.Add("Total Satisfied");
                    total.Columns.Add("Total Unhappy");
                    total.Rows.Add(totalhappy, totalsatisfied, totalunhappy);
                    DataTable stat = new DataTable("Questions");
                    stat.Columns.Add("QUESTIONS");
                    stat.Columns.Add("STATUS");
                    stat.Columns.Add("COUNT");
                    DataTable rem = new DataTable("Any other suggestions");
                    rem.Columns.Add("Remarks");
                    foreach (var data in remarks)
                    {
                        rem.Rows.Add(data);
                    }
                    int counter = 0;
                    foreach (var record in db1.FeedConfigs)
                    {


                        stat.Rows.Add(record.Question, "HAPPY", totalHappyForEachQuestion[counter]);
                        stat.Rows.Add(record.Question, "SATISFIED", totalSatisfiedForEachQuestion[counter]);
                        stat.Rows.Add(record.Question, "UNHAPPY", totalUnHappyForEachQuestion[counter]);
                        counter++;


                    }
                    DataSet ds = new DataSet("Hospital");
                    ds.Tables.Add(stat);
                    ds.Tables.Add(total);
                    ds.Tables.Add(rem);
                    string status = ExportDataSetToExcel(ds, location);
                    if (status.Equals("1"))
                        return "success";
                    else
                        return "failed";
                }
            }
            
                return "failed";
            
        }
        // GET: api/Feeds/5
        [ResponseType(typeof(Feed))]
        public async Task<IHttpActionResult> GetFeed(int id)
        {
            Feed feed = await db.Feeds.FindAsync(id);
            if (feed == null)
            {
                return NotFound();
            }

            return Ok(feed);
        }
        private string ExportDataSetToExcel(DataSet ds,string location)
        {
            //Creae an Excel application instance
            Excel.Application excelApp = new Excel.Application();
            //string date = DateTime.Now.ToString();
            
                string loc = location;
                //Path.GetFullPath(loc);
            //   string path = date.Trim();
            int randomnnumber = rnd.Next(1, 1000);
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            string myPath = @loc + randomnnumber + date + ".xls";
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
            try
            {
                excelWorkBook.SaveAs(myPath);
            excelWorkBook.Close();
            excelApp.Quit();
            }
            catch(Exception e)
            {
                return "0";
            }
            return "1";
        }

        // PUT: api/Feeds/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutFeed(int id, Feed feed)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != feed.sno)
            {
                return BadRequest();
            }

            db.Entry(feed).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeedExists(id))
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

        // POST: api/Feeds
        [ResponseType(typeof(Feed))]
        [System.Web.Http.HttpGet]
       
        public async Task<String> PostFeed([FromUri]Feed feed)
        {
            if (!ModelState.IsValid)
            {
                return "Error";
            }
            // feed.Dt = DateTime.Now;
            try {
                db.Feeds.Add(feed);
            }
            catch(Exception e)
            {

            }
            await db.SaveChangesAsync();

            return "Thankyou for submitting your Feedback";
        }
        [ResponseType(typeof(Feed))]
        [System.Web.Http.HttpGet]
        [ActionName("getlistofnames")]
        public async Task<List<String>>getallnames(string qno,string status,DateTime d,DateTime d1,string zone)
        {
            DateTime g = d.Date;
            DateTime g1 = d1.Date;
            DateTime n = Convert.ToDateTime(null);
            int questionnumber = Int32.Parse(qno) + 1;
            List<String> mrnoandnames = new List<String>();
            string statuscode = "";
            if (status.Equals("Happy"))
            {
                statuscode = "a";
            }
            else if (status.Equals("Satisfied"))
            {
                statuscode = "b"; 
            }
            else
            {
                statuscode = "c";
            }
            string format = "(" + questionnumber + ")" + statuscode;
            foreach(var record in db.Feeds)
            {
                if (zone != ConfigurationManager.AppSettings["allrecords"])
                {
                    
                        if (record.Dt >= g && record.Dt <= g1)
                        {
                            if (record.Location.Equals(zone))
                            {
                                if (record.Patient_feeds.Contains(format))
                                {
                                    mrnoandnames.Add(record.Location+ "$" + record.Dt+"$"+record.MRNO+"$"+record.Name);
                                }
                            }
                        }
                    
                }
                else
                {
                    
                        if (record.Dt >= g && record.Dt <= g1)
                        {
                            if (record.Patient_feeds.Contains(format))
                            {
                                mrnoandnames.Add(record.Location + "$" + record.Dt + "$" + record.MRNO + "$" + record.Name);
                            }
                        }
                    
                }
            }
            
            return mrnoandnames;

        }
        [ResponseType(typeof(Feed))]
        [System.Web.Http.HttpGet]
        [ActionName("getlistofnamesforbackup")]
        public async Task<List<String>> getallnamesforbackup(string qno, string status,int flag,string zone,DateTime d,DateTime d1)
        {
            DateTime g = d.Date;
            DateTime g1 = d1.Date;
            DateTime n = Convert.ToDateTime(null);
            int questionnumber = Int32.Parse(qno) + 1;
            List<String> mrnoandnames = new List<String>();
            string statuscode = "";
            if (status.Equals("Happy"))
            {
                statuscode = "a";
            }
            else if (status.Equals("Satisfied"))
            {
                statuscode = "b";
            }
            else
            {
                statuscode = "c";
            }
            string format = "(" + questionnumber + ")" + statuscode;
            foreach (var record in db.Feeds_BackUp)
            {
                if (zone!= ConfigurationManager.AppSettings["allrecords"]) 
                {
                    if (record.Flag == flag)
                    {
                        if (record.Dt >= g && record.Dt <= g1)
                        {
                            if(record.Location.Equals(zone))
                            { 
                                if (record.Patient_feeds.Contains(format))
                                {
                                    mrnoandnames.Add(record.Location + "$" + record.Dt+"$"+record.MRNO+"$"+record.Name);
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (record.Flag == flag)
                    {
                        if (record.Dt >= g && record.Dt <= g1)
                        {
                                if (record.Patient_feeds.Contains(format))
                                {
                                    mrnoandnames.Add(record.Location+ "$" + record.Dt+"$"+record.MRNO+"$"+record.Name);
                                }
                        }
                    }
                }
                
            }

            return mrnoandnames;

        }
        [HttpGet]
        public async Task<IQueryable<DateTime?>> GetFromDate(long? f, string lookup, string zone, DateTime d, DateTime d1)
        {
            DateTime g = d.Date;
            DateTime g1 = d1.Date;
            DateTime n = Convert.ToDateTime(null);
            IQueryable<DateTime?> result;
            if (lookup.Equals("Feeds_BackUp"))
            {
                if(!zone.Equals("ALL")) 
                    result = db.Feeds_BackUp.Where(x => x.Flag == f && x.Location == zone && x.Dt >= g && x.Dt <= g1).OrderBy(m => m.Dt).Select(m => m.Dt).Take(1);
                else
                    result = db.Feeds_BackUp.Where(x => x.Flag == f  && x.Dt >= g && x.Dt <= g1).OrderBy(m => m.Dt).Select(m => m.Dt).Take(1);
            }
            else
            {
                if(!zone.Equals("ALL")) 
                    result = db.Feeds.Where(x => x.Location == zone && x.Dt >= g && x.Dt <= g1).OrderBy(m => m.Dt).Select(m => m.Dt).Take(1);
                else
                    result = db.Feeds.Where(x=>x.Dt >= g && x.Dt <= g1).OrderBy(m => m.Dt).Select(m => m.Dt).Take(1);
            }

            // DateTime dt = DateTime.ParseExact(result.ToString(), "MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
            return result;
        }
        [HttpGet]
        public async Task<IQueryable<DateTime?>> GetToDate(long? f, string lookup, string zone, DateTime d, DateTime d1)
        {
            DateTime g = d.Date;
            DateTime g1 = d1.Date;
            DateTime n = Convert.ToDateTime(null);
            IQueryable<DateTime?> result;
            if (lookup.Equals("Feeds_BackUp"))
            { 
                if(!zone.Equals("ALL"))
                    result = db.Feeds_BackUp.Where(x => x.Flag == f && x.Location == zone && x.Dt >= g && x.Dt <= g1).OrderByDescending(m => m.Dt).Select(m => m.Dt).Take(1);
                else
                    result = db.Feeds_BackUp.Where(x => x.Flag == f && x.Dt >= g && x.Dt <= g1).OrderByDescending(m => m.Dt).Select(m => m.Dt).Take(1);
            }
            else
            {
                if (!zone.Equals("ALL"))
                    result = db.Feeds.Where(x => x.Location == zone && x.Dt >= g && x.Dt <= g1).OrderByDescending(m => m.Dt).Select(m => m.Dt).Take(1);
                else
                    result = db.Feeds.Where(x => x.Dt >= g && x.Dt <= g1).OrderByDescending(m => m.Dt).Select(m => m.Dt).Take(1);
            }
            return result;
        }





        [ResponseType(typeof(Feed))]
        [System.Web.Http.HttpGet]
        [ActionName("Getpattern")]
        public async Task<IQueryable<long?>>getpattern(DateTime d,DateTime d1,String zone)
        {
           // d = d + " " + "00:00:00";
            //d1 = d1 + " " + "00:00:00";
            //DateTime g = DateTime.ParseExact(d, "mm/dd/yy hh:mm:ss", null);
            //DateTime g1 = DateTime.ParseExact(d1, "mm/dd/yy hh:mm:ss", null);
           //   DateTime g1 = Convert.ToDateTime(d1);
            DateTime n = Convert.ToDateTime(null);
            DateTime g = d.Date;
            DateTime g1 = d1.Date;
            IQueryable<long?> result;
            if (!zone.Equals("ALL"))
            {


                result = db.Feeds_BackUp.Where(x => x.Dt >= g && x.Dt <= g1 && x.Location == zone).Select(m => m.Flag).Distinct();
            }
            else
            {
                result= db.Feeds_BackUp.Where(x => x.Dt >= g && x.Dt <= g1).Select(m => m.Flag).Distinct();
            }
            return result;
        }
        [ResponseType(typeof(Feed))]
        [System.Web.Http.HttpGet]
        [ActionName("feedbackresults")]
        public async Task<int[][]>Results(DateTime d, DateTime d1, String zone)
        {
            string totalhappy = "", totalsatisfied = "", totalunhappy = "";
            int count = 0;
            string[] feedarr = new string[] { };
            List<Feed> sam = new List<Feed>();
            List<Feed> list2 = new List<Feed>();
            // List<int> pattern = new List<int>();

            /* DateTime g = Convert.ToDateTime(d);
             DateTime g1 = Convert.ToDateTime(d1);*/
            DateTime g = d.Date;
            DateTime g1 = d1.Date;
            DateTime n = Convert.ToDateTime(null);
            foreach (var record in db1.FeedConfigs)
            {
                count++;
            }


            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new int[count];
            }
            try
            {
                using (db)
                {
                    if (zone.Equals(allrecords) && (!(g.Equals(n)) && !(g1.Equals(n))))
                    {
                        sam = db.Feeds.SqlQuery("SELECT * from Feeds WHERE Dt between @p0 and @p1", g.Date, g1.Date).ToList();
                        //var pattern= db.Feeds_BackUp.SqlQuery("SELECT  Flag *  FROM Feeds_BackUp").ToList();

                        //list2 = db.Feeds.SqlQuery("SELECT * from Feeds WHERE Dt between @p0 and @p1", g.Date, g1.Date).ToList();
                    }
                    else if (zone.Equals(allrecords) && g.Equals(n) && g1.Equals(n))
                    {
                        sam = db.Feeds.SqlQuery("SELECT * from Feeds").ToList();
                        //list2 = db.Feeds.SqlQuery("SELECT * from Feeds WHERE Flag=@p1", pattern).ToList();
                    }

                    else if (g.Equals(n) && g1.Equals(n))
                    {
                        sam = db.Feeds.SqlQuery("SELECT * from Feeds WHERE Location=@p0",zone).ToList();
                        //list2 = db.Feeds_BackUp.SqlQuery("SELECT * from Feeds_BackUp WHERE Location=@p0 AND Flag=@p1", zone, pattern).ToList();
                    }
                    else if (g.Equals(g1) && !zone.Equals(""))
                    {
                        sam = db.Feeds.SqlQuery("SELECT * from Feeds WHERE Dt=@p0 AND Location=@p1", g.Date,zone).ToList();
                        //list2 = db.Feeds_BackUp.SqlQuery("SELECT * from Feeds_BackUp WHERE Dt<=@p0 AND Location=@p1 AND Flag=@p2", g.Date, zone, pattern).ToList();
                    }
                    else if (g.Equals(n) && !(zone.Equals("")))
                    {
                        //DateTime today = DateTime.Today;
                        sam = db.Feeds.SqlQuery("SELECT * from Feeds WHERE Dt<=@p0 AND Location=@p1", g1.Date,zone).ToList();
                        //list2 = db.Feeds_BackUp.SqlQuery("SELECT * from Feeds_BackUp WHERE Dt<=@p0 AND Location=@p1 AND Flag=@p2", g1.Date, zone, pattern).ToList();
                    }
                    else if (g1.Equals(n) && !(zone.Equals("")))
                    {
                         sam = db.Feeds.SqlQuery("SELECT * from Feeds WHERE Dt<=@p0 AND Location=@p1", g.Date,zone).ToList();
                        //list2 = db.Feeds_BackUp.SqlQuery("SELECT * from Feeds_BackUp WHERE Dt<=@p0 AND Location=@p1 AND Flag=@p2", g.Date, zone, pattern).ToList();
                    }
                    else if (!zone.Equals("") && !g.Equals(n) && !g1.Equals(n))
                    {

                        sam = db.Feeds.SqlQuery("SELECT * from Feeds WHERE Dt between @p0 and @p1 AND Location=@p2", g.Date, g1.Date,zone).ToList();
                        //list2 = db.Feeds_BackUp.SqlQuery("SELECT * from Feeds_BackUp WHERE Dt between @p0 and @p1 AND Location=@p2 AND Flag=@p3", g.Date, g1.Date, zone, pattern).ToList();
                    }
                    for (int i = 0; i < sam.Count; i++)
                    {
                        decode(sam.ElementAt(i).Patient_feeds, sam.ElementAt(i).MRNO, sam.ElementAt(i).Name);
                    }
                  

                }


            }
            catch (Exception e)
            {
                return result;
            }
            return result;
        }
        [ResponseType(typeof(Feed))]
        [System.Web.Http.HttpGet]
        [ActionName("resultmatrix")]
        public async Task<int[][]> Resultsmatirx(DateTime d, DateTime d1, String zone)
        {
            string totalhappy = "", totalsatisfied = "", totalunhappy = "";
            int count = 0;
            string[] feedarr = new string[] { };
            List<Feed> sam = new List<Feed>();
            List<Feed> list2 = new List<Feed>();
            // List<int> pattern = new List<int>();

            //DateTime g = Convert.ToDateTime(d);
            //DateTime g1 = Convert.ToDateTime(d1);
           DateTime g = d.Date;
            DateTime g1 = d1.Date;
            DateTime n = Convert.ToDateTime(null);
            foreach (var record in db1.FeedConfigs)
            {
                count++;
            }


            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new int[count];
            }
            try
            {
                using (db)
                {
                    if (zone.Equals(allrecords) && (!(g.Equals(n)) && !(g1.Equals(n))))
                    {
                        sam = db.Feeds.SqlQuery("SELECT * from Feeds WHERE Dt between @p0 and @p1 ",g.Date,g1.Date).ToList();
                        //var pattern= db.Feeds_BackUp.SqlQuery("SELECT  Flag *  FROM Feeds_BackUp").ToList();

                        //list2 = db.Feeds.SqlQuery("SELECT * from Feeds WHERE Dt between @p0 and @p1", g.Date, g1.Date).ToList();
                    }
                    else if (zone.Equals(allrecords) && g.Equals(n) && g1.Equals(n))
                    {
                        sam = db.Feeds.SqlQuery("SELECT * from Feeds").ToList();
                        //list2 = db.Feeds.SqlQuery("SELECT * from Feeds WHERE Flag=@p1", pattern).ToList();
                    }

                    else if (g.Equals(n) && g1.Equals(n))
                    {
                        sam = db.Feeds.SqlQuery("SELECT * from Feeds WHERE Location=@p0", zone).ToList();
                        //list2 = db.Feeds_BackUp.SqlQuery("SELECT * from Feeds_BackUp WHERE Location=@p0 AND Flag=@p1", zone, pattern).ToList();
                    }
                    else if (g.Equals(g1) && !zone.Equals(""))
                    {
                        sam = db.Feeds.SqlQuery("SELECT * from Feeds WHERE Dt=@p0 AND Location=@p1", g.Date, zone).ToList();
                        //list2 = db.Feeds_BackUp.SqlQuery("SELECT * from Feeds_BackUp WHERE Dt<=@p0 AND Location=@p1 AND Flag=@p2", g.Date, zone, pattern).ToList();
                    }
                    else if (g.Equals(n) && !(zone.Equals("")))
                    {
                        //DateTime today = DateTime.Today;
                        sam = db.Feeds.SqlQuery("SELECT * from Feeds WHERE Dt<=@p0 AND Location=@p1", g1.Date, zone).ToList();
                        //list2 = db.Feeds_BackUp.SqlQuery("SELECT * from Feeds_BackUp WHERE Dt<=@p0 AND Location=@p1 AND Flag=@p2", g1.Date, zone, pattern).ToList();
                    }
                    else if (g1.Equals(n) && !(zone.Equals("")))
                    {
                        sam = db.Feeds.SqlQuery("SELECT * from Feeds WHERE Dt<=@p0 AND Location=@p1", g.Date, zone).ToList();
                        //list2 = db.Feeds_BackUp.SqlQuery("SELECT * from Feeds_BackUp WHERE Dt<=@p0 AND Location=@p1 AND Flag=@p2", g.Date, zone, pattern).ToList();
                    }
                    else if (!zone.Equals("") && !g.Equals(n) && !g1.Equals(n))
                    {

                        sam = db.Feeds.SqlQuery("SELECT * from Feeds WHERE Dt between @p0 and @p1 AND Location=@p2", g.Date, g1.Date, zone).ToList();
                        //list2 = db.Feeds_BackUp.SqlQuery("SELECT * from Feeds_BackUp WHERE Dt between @p0 and @p1 AND Location=@p2 AND Flag=@p3", g.Date, g1.Date, zone, pattern).ToList();
                    }
                    for (int i = 0; i < sam.Count; i++)
                    {
                        decode(sam.ElementAt(i).Patient_feeds, sam.ElementAt(i).MRNO, sam.ElementAt(i).Name);
                    }


                }


            }
            catch (Exception e)
            {
                return result;
            }
            return result;
        }

        [ResponseType(typeof(Feed))]
        [System.Web.Http.HttpGet]
        [ActionName("feedbackresults1")]
        public async Task<int[][]> ResultsModified(DateTime d, DateTime d1,String zone,String pattern)
        {
            string totalhappy = "", totalsatisfied = "", totalunhappy = "";
            int count = 0;
            string[] feedarr = new string[] { };
            List<Feed> sam = new List<Feed>();
            List<Feeds_BackUp> list2 = new List<Feeds_BackUp>();
            // List<int> pattern = new List<int>();
            int pat = Int32.Parse(pattern);
            //  DateTime g = Convert.ToDateTime(d);
            // DateTime g1 = Convert.ToDateTime(d1);
            DateTime g = d.Date;
            DateTime g1 = d1.Date;
            DateTime n = Convert.ToDateTime(null);
            foreach (var record in db1.FeedsConfig_Backup)
            {
                if(record.Flag==pat)
                    count++;
            }


            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new int[count];
            }
            try
            {
                using (db)
                {
                    if(zone.Equals(allrecords)&&(!(g.Equals(n))&&!(g1.Equals(n))))
                     {
                        //sam = db.Feeds.SqlQuery("SELECT * from Feeds WHERE Dt between @p0 and @p1", g.Date, g1.Date).ToList();
                        //var pattern= db.Feeds_BackUp.SqlQuery("SELECT  Flag *  FROM Feeds_BackUp").ToList();
                       
                        list2 = db.Feeds_BackUp.SqlQuery("SELECT * from Feeds_BackUp WHERE Dt between @p0 and @p1 AND Flag=@p2", g.Date, g1.Date,pattern).ToList();
                     }
                    else if(zone.Equals(allrecords) && g.Equals(n) && g1.Equals(n))
                    {
                        //sam = db.Feeds.SqlQuery("SELECT * from Feeds").ToList();
                        list2=db.Feeds_BackUp.SqlQuery("SELECT * from Feeds_BackUp WHERE Flag=@p1",pattern).ToList();
                    }
                  
                    else if(g.Equals(n)&&g1.Equals(n))
                        {
                        //sam = db.Feeds.SqlQuery("SELECT * from Feeds WHERE Location=@p0",zone).ToList();
                        list2 = db.Feeds_BackUp.SqlQuery("SELECT * from Feeds_BackUp WHERE Location=@p0 AND Flag=@p1", zone,pattern).ToList();
                    }
                    else if (g.Equals(g1)&&!zone.Equals(""))
                    {
                        //sam = db.Feeds.SqlQuery("SELECT * from Feeds WHERE Dt<=@p0 AND Location=@p1", g.Date,zone).ToList();
                        list2=db.Feeds_BackUp.SqlQuery("SELECT * from Feeds_BackUp WHERE Dt=@p0 AND Location=@p1 AND Flag=@p2", g.Date, zone,pattern).ToList();
                    }
                    else if (g.Equals(n)&&!(zone.Equals("")))
                    {
                        //DateTime today = DateTime.Today;
                        //sam = db.Feeds.SqlQuery("SELECT * from Feeds WHERE Dt<=@p0 AND Location=@p1", g1.Date,zone).ToList();
                        list2=db.Feeds_BackUp.SqlQuery("SELECT * from Feeds_BackUp WHERE Dt<=@p0 AND Location=@p1 AND Flag=@p2", g1.Date, zone,pattern).ToList();
                    }
                    else if (g1.Equals(n)&&!(zone.Equals("")))
                    {
                       // sam = db.Feeds.SqlQuery("SELECT * from Feeds WHERE Dt<=@p0 AND Location=@p1", g.Date,zone).ToList();
                        list2=db.Feeds_BackUp.SqlQuery("SELECT * from Feeds_BackUp WHERE Dt<=@p0 AND Location=@p1 AND Flag=@p2", g.Date, zone,pattern).ToList();
                    }
                    else if(!zone.Equals("")&&!g.Equals(n)&&!g1.Equals(n))
                    {
                       
                        //sam = db.Feeds.SqlQuery("SELECT * from Feeds WHERE Dt between @p0 and @p1 AND Location=@p2", g.Date, g1.Date,zone).ToList();
                        list2=db.Feeds_BackUp.SqlQuery("SELECT * from Feeds_BackUp WHERE Dt between @p0 and @p1 AND Location=@p2 AND Flag=@p3", g.Date, g1.Date, zone,pattern).ToList();
                    }
                    for (int i = 0; i < sam.Count; i++)
                    {
                        decode(sam.ElementAt(i).Patient_feeds, sam.ElementAt(i).MRNO, sam.ElementAt(i).Name);
                    }
                    for (int i = 0; i < list2.Count; i++)
                    {
                        decode(list2.ElementAt(i).Patient_feeds, list2.ElementAt(i).MRNO, list2.ElementAt(i).Name);
                    }

                }


            }
            catch (Exception e)
            {
                return result;
            }
            return result;
        }

        [System.Web.Http.HttpGet]
        [ResponseType(typeof(System_Users))]
        [ActionName("resultfortable")]
        public int[][] resultfortable(string t1,string t2)
        {
            List<Feed> sam = new List<Feed>();
            int count = 0;
            foreach (var record in db1.FeedConfigs)
            {
                count++;
            }


            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new int[count];
            }
            using (db)
            {
                sam = db.Feeds.SqlQuery("select * , cast(Dt as time) from dbo.Feeds where cast(Dt as time) between @p0 and @p1",t1,t2).ToList();
            }
            for (int i = 0; i < sam.Count; i++)
            {
                decode(sam.ElementAt(i).Patient_feeds, sam.ElementAt(i).MRNO, sam.ElementAt(i).Name);
            }

           
            return result;
        }
        [System.Web.Http.HttpGet]
        [ResponseType(typeof(System_Users))]
        [ActionName("totalnumberofpatients")]
        public int TOTALNUMBEROFPATIENTS(string t1, string t2)
        {
            List<Feed> sam = new List<Feed>();
            int count = 0;
           
            using (db)
            {
                sam = db.Feeds.SqlQuery("select * , cast(Dt as time) from dbo.Feeds where cast(Dt as time) between @p0 and @p1", t1, t2).ToList();
            }
            count = sam.Count;
            return count;

            
        }



        public void decode(string feedback, string mrno, string name)
        {
            string number="";
            int pos = 0;
            int flag = 0;
            for (int i = 0; i <feedback.Length; i++)
            {

                if (flag == 0)
                {
                    if (feedback[i] == '(')
                    {
                        flag = 1;
                        number = "";
                    }
                }
                else
                {
                    if (flag == 1)
                    {
                        if (feedback[i] == ')')
                        {
                            flag = 0;
                            int temp = Int32.Parse(number);
                            switch (feedback[i+1])
                            {

                                case 'a':
                                    result[0][temp - 1]= (result[0][temp - 1]+1);
                                    break;
                                case 'b':
                                    result[1][temp - 1] = (result[1][temp - 1] + 1);
                                    break;
                                case 'c':
                                    result[2][temp - 1] = (result[2][temp - 1] + 1);
                                    break;
                            }
                        }
                           
                        else
                            number += feedback[i];
                        Console.WriteLine(number);
                    }
                }
                
            }
        }
            // DELETE: api/Feeds/5
        [ResponseType(typeof(Feed))]
        public async Task<IHttpActionResult> DeleteFeed(int id)
        {
            Feed feed = await db.Feeds.FindAsync(id);
            if (feed == null)
            {
                return NotFound();
            }

            db.Feeds.Remove(feed);
            await db.SaveChangesAsync();

            return Ok(feed);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FeedExists(int id)
        {
            return db.Feeds.Count(e => e.sno == id) > 0;
        }
    }
}