using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KIOSK.Models;
using System.Net.Http;
using System.Net;
using System.IO;
using System.Configuration;
using System.Web.Script.Serialization;
using KIOSK.Models;
using System.Globalization;
using System.Security.AccessControl;

namespace KIOSK.Controllers
{
    public class FormController : Controller
    {
        String url = ConfigurationManager.AppSettings["updatetomyserver"];
        String backupfilesizelimit = ConfigurationManager.AppSettings["backupsize"];
        String backuplocation = ConfigurationManager.AppSettings["locationofthelogfile"];
        String maxfoldersize = ConfigurationManager.AppSettings["maxfoldersize"];
        String nooffiles = ConfigurationManager.AppSettings["numberoffilestobedeleted"];
        long totalFileSize;
        private Entities3 db = new Entities3();
        private Entities4 db1 = new Entities4();
        private Entities5 db2 = new Entities5();
        private Entities6 db3 = new Entities6();
        Random rnd = new Random();
        // GET: Form
    //    [OutputCache(Duration = 28800, VaryByParam = "none")]
        public ActionResult form()
        {
            return View();
        }
        [HttpPost]
        public ActionResult form(form ob, string command)
        {
            String url1;
            String sLine = "";
            int dateofbirth;
            
            string logFilename = ConfigurationManager.AppSettings["filename"];
            if (ModelState.IsValid)
            {
              
                try
                {
                   
                    string kithkin = ob.relation +" "+ob.nextofkin;
                    DateTime dt = DateTime.Now;
                    string dob = ob.dob.ToString("MM/dd/yyyy HH:mm:ss");
                    string s = dt.ToString("MM/dd/yyyy HH:mm:ss");
                  
                    dateofbirth = ob.dob.Year - dt.Year;
                    Taluk_Master table = db.Taluk_Master.Find(ob.taluk);
                    District_Master table1 = db1.District_Master.Find(table.DISTRICT_CODE);
                    ob.district = table1.DISTRICT_CODE;
                    State_Master table2 = db2.State_Master.Find(table1.STATE_CODE);
                    ob.state = table2.STATE_CODE;
                    Country_Master table3 = db3.Country_Master.Find(table2.COUNTRY_CODE);
                    ob.country = table3.COUNTRY_CODE;
                    int span = (DateTime.Now).Year - (ob.dob).Year;
                    int span1 = (DateTime.Now).Month - (ob.dob).Month;
                    int span2 = (DateTime.Now).Day - (ob.dob).Day;
                    if(span==0)     //if dob year equals current year
                    {
                        if(span1<0)     //current month less than dob month, birthday yet to come
                        {
                            ob.age = "0";   //zero months old

                        }
                        else if(span1>0)    // current month greater than dob month, birthday already over.
                        {
                            string ageInMonths=span1.ToString();
                            Console.WriteLine("Age in months=" + ageInMonths);
                        }
                    }
                    else if (span1 < 0)//current month less than dob month, birthday yet to come
                    {
                        if (span1 == 0)//current month equals dob month
                        {
                            if (span2 < 0)//dob day greater than current day
                            {
                                span = span - 1;//subtracting 1 from year because birthday is yet to come
                            }
                            else
                            {
                                span = span + 1;//adding 1 to year because birthday already over
                            }
                        }
                        else
                        {
                            span = span - 1;//current month less or greater than dob month
                        }
                    }
                    //ob.age = span.ToString();
                    ob.alreadyVisted = "No";
                   WebRequest wrGETURL;
                    if (ob.mobile != null&&ob.landline!=null)
                    {
                        url1 = url + System.Uri.EscapeDataString(ob.name) + "&Age=" + ob.age + "&Dob=" +dob+ "&Gender=" + ob.sex + "&Nextofkin=" + System.Uri.EscapeDataString(kithkin) + "&Dt=" + s + "&Email=" + ob.email + "&MobileNo=" + System.Uri.EscapeDataString(ob.mobile) + "&LandlineNo=" + System.Uri.EscapeDataString(ob.landline) + "&Address=" + System.Uri.EscapeDataString(ob.street) + "&Locality=" + System.Uri.EscapeDataString(ob.locality) + "&Taluk=" + System.Uri.EscapeDataString(ob.taluk) + "&City=" + System.Uri.EscapeDataString(ob.city) + "&District=" + System.Uri.EscapeDataString(ob.district) + "&State=" + System.Uri.EscapeDataString(ob.state) + "&Country=" + System.Uri.EscapeDataString(ob.country) + "&Pincode=" + ob.pin + "&Visit=" + ob.alreadyVisted;
                    }
                    else if(ob.mobile != null&&ob.landline==null)
                    {
                         url1 = url + System.Uri.EscapeDataString(ob.name) + "&Age=" + ob.age + "&Dob=" + dob + "&Gender=" + ob.sex + "&Nextofkin=" + System.Uri.EscapeDataString(kithkin) + "&Dt=" + s+ "&Email=" + ob.email + "&MobileNo=" + System.Uri.EscapeDataString(ob.mobile) + "&LandlineNo=" + ob.landline+"&Address=" + System.Uri.EscapeDataString(ob.street) + "&Locality=" + System.Uri.EscapeDataString(ob.locality) + "&Taluk=" + System.Uri.EscapeDataString(ob.taluk) + "&City=" + System.Uri.EscapeDataString(ob.city) + "&District=" + System.Uri.EscapeDataString(ob.district) + "&State=" + System.Uri.EscapeDataString(ob.state) + "&Country=" + System.Uri.EscapeDataString(ob.country) + "&Pincode=" + ob.pin + "&Visit=" + ob.alreadyVisted;
                    }
                    else if(ob.mobile==null&ob.landline!=null)
                    {
                         url1 = url + System.Uri.EscapeDataString(ob.name) + "&Age=" + ob.age + "&Dob=" +dob + "&Gender=" + ob.sex + "&Nextofkin=" + System.Uri.EscapeDataString(kithkin) + "&Dt=" + s+ "&Email=" + ob.email + "&MobileNo=" + ob.mobile+ "&LandlineNo=" + System.Uri.EscapeDataString(ob.landline) + "&Address=" + System.Uri.EscapeDataString(ob.street) + "&Locality=" + System.Uri.EscapeDataString(ob.locality) + "&Taluk=" + System.Uri.EscapeDataString(ob.taluk) + "&City=" + System.Uri.EscapeDataString(ob.city) + "&District=" + System.Uri.EscapeDataString(ob.district) + "&State=" + System.Uri.EscapeDataString(ob.state) + "&Country=" + System.Uri.EscapeDataString(ob.country) + "&Pincode=" + ob.pin + "&Visit=" + ob.alreadyVisted;
                    }
                    else
                    {
                         url1 = url + System.Uri.EscapeDataString(ob.name) + "&Age=" + ob.age + "&Dob=" +dob+ "&Gender=" + ob.sex + "&Nextofkin=" + System.Uri.EscapeDataString(kithkin) + "&Dt=" + s+ "&Email=" + ob.email + "&MobileNo=" + ob.mobile + "&LandlineNo=" + ob.landline + "&Address=" + System.Uri.EscapeDataString(ob.street) + "&Locality=" + System.Uri.EscapeDataString(ob.locality) + "&Taluk=" + System.Uri.EscapeDataString(ob.taluk) + "&City=" + System.Uri.EscapeDataString(ob.city) + "&District=" + System.Uri.EscapeDataString(ob.district) + "&State=" + System.Uri.EscapeDataString(ob.state) + "&Country=" + System.Uri.EscapeDataString(ob.country) + "&Pincode=" + ob.pin + "&Visit=" + ob.alreadyVisted;
                    }
                        wrGETURL = WebRequest.Create( url1);
                   // wrGETURL = WebRequest.Create("http://localhost:50872/api/Patients/PostPatient/patient?PIN=0000&Name=sdfsdf&Age=21&Dob=02/04/1995&Gender=1&Nextofkin=sdfsdf&Dt=3/4/2016&Email=yesfsj&MobileNo=878787&LandlineNo=8787878&Address=sfsdf&City=sddf&District=sfsdfs&State=sdsdf&Country=sdfsd&Taluk=sdfs&Pincode=4543453&Visit=yes");
                    Stream objStream;
                    objStream = wrGETURL.GetResponse().GetResponseStream();
                    StreamReader objReader = new StreamReader(objStream);
                    sLine= objReader.ReadLine();
                    Otpnumber details = new Otpnumber();
                    details.otp = sLine;
                    int nooffilestobedeleted = Int32.Parse(nooffiles);
                    long maxsizelimit = Int32.Parse(maxfoldersize);
                    /*string[] dirFiles = Directory.GetFiles(backuplocation, "*.log",
                          System.IO.SearchOption.AllDirectories);
                    foreach (string fileName in dirFiles)
                    {
                        // Use FileInfo to get length of each file.
                        FileInfo info = new FileInfo(fileName);
                        
                        totalFileSize = totalFileSize + info.Length;
                    }
                    if (totalFileSize >maxsizelimit)
                    {
                        int count = 0;
                        foreach (string fileName in dirFiles)
                        {
                            if (nooffilestobedeleted > 0)
                            {
                                if (count < nooffilestobedeleted)
                                {
                                    System.IO.File.Delete(fileName);
                                    count++;
                                }
                            }

                        }
                    }*/
                    DirectoryInfo info = new DirectoryInfo(backuplocation);
                    //   string[] dirFiles = Directory.GetFiles(backuplocation, "*.log",
                    //             System.IO.SearchOption.AllDirectories);
                    FileInfo[] files = info.GetFiles().OrderBy(p => p.CreationTime).ToArray();
                    foreach (FileInfo fileName in files)
                    {

                        totalFileSize = totalFileSize + fileName.Length;
                    }
                    //  long maxsizelimit = Int32.Parse(maxfoldersize);
                    //int nooffilestobedeleted = Int32.Parse(nooffiles);
                    if (maxsizelimit > 0)
                    {
                        if (totalFileSize > maxsizelimit)
                        {
                            int count = 0;
                            foreach (FileInfo fileName in files)
                            {
                                if (nooffilestobedeleted > 0)
                                {
                                    if (count < nooffilestobedeleted)
                                    {
                                        // string tempname = backuplocation + fileName.FullName;
                                        System.IO.File.Delete(fileName.FullName);
                                        count++;
                                    }
                                }

                            }
                        }
                    }


                    int randomnnumber = rnd.Next(1, 1000);
                    // var filename1 = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\" + logFilename;
                    var filename1= backuplocation+logFilename;
                    var sw1 = new System.IO.StreamWriter(filename1, true);
                    FileInfo fileinformation = new FileInfo(filename1);
                    var size = fileinformation.Length;

                    long backupsize=Int32.Parse(backupfilesizelimit);
                    if (size> backupsize)
                    {
                        sw1.Close();
                        //System.IO.File.Delete(filename1);
                      string date = DateTime.Now.ToString("yyyyMMddHHmmss");
                        string newfilename = randomnnumber.ToString()+date + logFilename;
                        // var newfile = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\" + newfilename;
                        var newfile = backuplocation + newfilename;
                        System.IO.File.Move(filename1,newfile);
                        sw1 = new System.IO.StreamWriter(newfile, true);
                    }
                    sw1.WriteLine(DateTime.Now.ToString() + " " + " Kiosk Application : Form field validation passed");
                    sw1.WriteLine(DateTime.Now.ToString() +" "+ " Kiosk Application : OTP Generation successful. Current OTP is " + details.otp);
                    sw1.Close();
                    //otp = sLine;
                    //JavaScriptSerializer serializer1 = new JavaScriptSerializer();
                    //Otpnumber details = serializer1.Deserialize<Otpnumber>(sLine);
                    //ob.age = span.ToString();
                    return View("Result", details);
                    
                }
                catch (Exception e)
                {
                    /* string[] dirFiles = Directory.GetFiles(backuplocation, "*.log",
                             System.IO.SearchOption.AllDirectories);
                     int nooffilestobedeleted = Int32.Parse(nooffiles);
                     foreach (string fileName in dirFiles)
                     {
                         // Use FileInfo to get length of each file.
                         FileInfo info = new FileInfo(fileName);
                         totalFileSize = totalFileSize + info.Length;
                     }
                     long maxsizelimit = Int32.Parse(maxfoldersize);
                     if (totalFileSize > maxsizelimit)
                     {
                         int count = 0;
                         foreach (string fileName in dirFiles)
                         {
                             if (nooffilestobedeleted > 0)
                             {
                                 if (count < nooffilestobedeleted)
                                 {
                                     System.IO.File.Delete(fileName);
                                     count++;
                                 }
                             }

                         }
                     }*/
                    DirectoryInfo info = new DirectoryInfo(backuplocation);
                    //   string[] dirFiles = Directory.GetFiles(backuplocation, "*.log",
                    //             System.IO.SearchOption.AllDirectories);
                    FileInfo[] files = info.GetFiles().OrderBy(p => p.CreationTime).ToArray();
                    foreach (FileInfo fileName in files)
                    {

                        totalFileSize = totalFileSize + fileName.Length;
                    }
                    long maxsizelimit = Int32.Parse(maxfoldersize);
                    int nooffilestobedeleted = Int32.Parse(nooffiles);
                    if (maxsizelimit > 0)
                    {
                        if (totalFileSize > maxsizelimit)
                        {
                            int count = 0;
                            foreach (FileInfo fileName in files)
                            {
                                if (nooffilestobedeleted > 0)
                                {
                                    if (count < nooffilestobedeleted)
                                    {
                                        // string tempname = backuplocation + fileName.FullName;
                                        System.IO.File.Delete(fileName.FullName);
                                        count++;
                                    }
                                }

                            }
                        }
                    }
                    int randomnnumber = rnd.Next(1, 1000);
                    // var filename = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\" + logFilename;
                    var filename = backuplocation + logFilename;
                    var sw1 = new System.IO.StreamWriter(filename, true);
                    FileInfo fileinformation = new FileInfo(filename);
                    var size = fileinformation.Length;
                    long backupsize = Int32.Parse(backupfilesizelimit);
                    if (size > backupsize)
                    {
                       
                        sw1.Close();
                        string date = DateTime.Now.ToString("yyyyMMddHHmmss");
                        string newfilename = randomnnumber.ToString()+date + logFilename;
                        //  System.IO.File.Delete(filename);
                        var newfile = backuplocation + newfilename;
                        System.IO.File.Move(filename, newfile);
                        sw1 = new System.IO.StreamWriter(filename, true);
                    }
                  
                    sw1.WriteLine(DateTime.Now.ToString() + " Kiosk Application : " + e.Message + " " + e.InnerException);
                    sw1.Close();
                    return View();
                }


            }
            else
            {
                DirectoryInfo info = new DirectoryInfo(backuplocation);
             //   string[] dirFiles = Directory.GetFiles(backuplocation, "*.log",
               //             System.IO.SearchOption.AllDirectories);
                FileInfo[] files = info.GetFiles().OrderBy(p => p.CreationTime).ToArray();
                foreach (FileInfo fileName in files)
                {
                    
                     totalFileSize = totalFileSize + fileName.Length;
                }
                long maxsizelimit = Int32.Parse(maxfoldersize);
                int nooffilestobedeleted = Int32.Parse(nooffiles);
                if (maxsizelimit > 0)
                {
                    if (totalFileSize > maxsizelimit)
                    {
                        int count = 0;
                        foreach (FileInfo fileName in files)
                        {
                            if (nooffilestobedeleted > 0)
                            {
                                if (count < nooffilestobedeleted)
                                {
                                    // string tempname = backuplocation + fileName.FullName;
                                    System.IO.File.Delete(fileName.FullName);
                                    count++;
                                }
                            }

                        }
                    }
                }
                int randomnnumber = rnd.Next(1, 1000);
                //  var filename = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\" + logFilename;
                var filename = backuplocation+ logFilename;
                var sw1 = new System.IO.StreamWriter(filename, true);
                FileInfo fileinformation = new FileInfo(filename);
                var size = fileinformation.Length;
                long backupsize = Int32.Parse(backupfilesizelimit);
                if (size > backupsize)
                {
                    sw1.Close();
                    string date = DateTime.Now.ToString("yyyyMMddHHmmss");
                    string newfilename = randomnnumber.ToString()+date + logFilename;
                    // System.IO.File.Delete(filename);
                    var newfile = backuplocation + newfilename;
                    System.IO.File.Move(filename, newfile);
                    sw1 = new System.IO.StreamWriter(filename, true);
                }
               
                sw1.WriteLine(DateTime.Now.ToString() +" "+ " Kiosk Application : Form field validation failed");
                sw1.Close();
                return View();
            }
                    
        }
        [HttpPost]
        public ActionResult RsvpForm()
        {
            return RedirectToAction("Index1", "Home");
        }
        public ActionResult  Otpnumber()
        {
            return View();
        }
        public ActionResult Result()
        {
            return View();
        }
    
    }
    }