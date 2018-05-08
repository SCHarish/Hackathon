using System;
using System.Collections.Generic;
using System.Web.Mvc;
using FeedbackAPP.Models;
using System.Net;
using System.IO;
using System.Configuration;

namespace FeedbackAPP.Controllers
{
    public class HomeController : Controller
    {

        string[][] result = new string[3][];
        int q1_a = 0, q1_b = 0, q1_c = 0, q2_a = 0, q2_b = 0, q2_c = 0, q3_a = 0, q3_b = 0, q3_c = 0, q4_a = 0, q4_b = 0, q4_c = 0, q5_a = 0, q5_b = 0, q5_c = 0;
        List<string> q1a_user = new List<string>();
        List<string> q1b_user = new List<string>();
        List<string> q1c_user = new List<string>();
        List<string> q2a_user = new List<string>();
        List<string> q2b_user = new List<string>();
        List<string> q2c_user = new List<string>();
        List<string> q3a_user = new List<string>();
        List<string> q3b_user = new List<string>();
        List<string> q3c_user = new List<string>();
        List<string> q4a_user = new List<string>();
        List<string> q4b_user = new List<string>();
        List<string> q4c_user = new List<string>();
        List<string> q5a_user = new List<string>();
        List<string> q5b_user = new List<string>();
        List<string> q5c_user = new List<string>();
        

        List<string> q1a_userName = new List<string>();
        List<string> q1b_userName = new List<string>();
        List<string> q1c_userName = new List<string>();
        List<string> q2a_userName = new List<string>();
        List<string> q2b_userName = new List<string>();
        List<string> q2c_userName = new List<string>();
        List<string> q3a_userName = new List<string>();
        List<string> q3b_userName = new List<string>();
        List<string> q3c_userName = new List<string>();
        List<string> q4a_userName = new List<string>();
        List<string> q4b_userName = new List<string>();
        List<string> q4c_userName = new List<string>();
        List<string> q5a_userName = new List<string>();
        List<string> q5b_userName = new List<string>();
        List<string> q5c_userName = new List<string>();

        String adminpassword = ConfigurationManager.AppSettings["adminpassword"];
        String getnoofquestions = ConfigurationManager.AppSettings["getnoofquestions"];
        int beforeValue=int.Parse(ConfigurationManager.AppSettings["beforeValue"]), afterValue=int.Parse(ConfigurationManager.AppSettings["afterValue"]);

        // GET: Home
        public ActionResult Index()
        {
            
            return View();
        }
        public ActionResult qrscan()
        {
            return View();
        }
        public ActionResult Checkpage()
        {
            return View();
        }
        public ActionResult Zone()
        {
            return View();
        }
        public ActionResult Resultpage()
        {
            return View();
        }
        [HttpPost]
        public ActionResult mrnosubmit(User p)
        {
            if(p.pattern==null&p.MRID==null)
            {
                return View("qrscan");
            }
            TempData["mr"] = p.pattern+"-"+p.MRID;
            TempData["name"] = "UNKNOWN";
            return View("UserFeedback");
        }
        [HttpPost]
        public ActionResult qrscan(User p)
        {
            bool b;
            int pos = 0,num1,num2;
            string mrno;
            string extract = "";
            bool before=false, after=false;

            if (p.pin.Contains("$"))
            {
                string[] nameno = p.pin.Split('$');
                if (nameno[1].Contains("-"))
                {
                    string[] num = nameno[1].Split('-');
                    if (num[0].Length == beforeValue && int.TryParse(num[0], out num1))
                    {
                        before = true;
                    }
                    if (num[1].Length == afterValue && int.TryParse(num[1], out num2))
                    {
                        after = true;
                    }
                }
                else
                {
                    before = false;
                    after = false;
                }
                WebRequest wrGETURL;
                String url = ConfigurationManager.AppSettings["validateuser"];
                url = url + "_username=" + nameno[0] + "&_password=" + nameno[1];
                wrGETURL = WebRequest.Create(url);
                Stream objStream;
                objStream = wrGETURL.GetResponse().GetResponseStream();
                StreamReader objReader = new StreamReader(objStream);
                String isvalid = objReader.ReadLine();
                if (isvalid.Equals("true"))
                {

                    p.UserName = nameno[0];
                    p.Password = nameno[1];
                    return RedirectToAction("AdminFeedback", p);
                }
                else if (p.MRID == null || p.pattern == null)
                {
                    if (p.pin == null)
                    {
                        return View("qrscan");
                    }
                    else if (before && after)
                    {
                        for (int i = 0; i < p.pin.Length; i++)
                        {
                            if (p.pin[i] != '$')
                                p.name = p.name + p.pin[i];
                            else
                            {
                                pos = i;
                                break;
                            }
                        }
                        for (int i = pos + 1; i < p.pin.Length; i++)
                        {
                            extract = extract + p.pin[i];
                        }
                        b = true;
                        p.pin = extract;
                        mrno = extract;

                    }
                    else
                    {
                        b = false;
                        mrno = "";
                    }
                }
                else
                {
                    mrno = p.pattern + "-" + p.MRID;
                    b = nameno[1].Contains("-");
                }
                if (b == true)
                {
                    if (before && after)
                    {
                        TempData["name"] = p.name;
                        TempData["mr"] = nameno[1];
                        return RedirectToAction("UserFeedback", p);
                    }
                    else
                        return View("qrscan");
                }
                else
                {
                    return View("qrscan");
                }



            }
            else
            {
                return View("qrscan");
            }

            }
        //for login http post method
        [HttpPost]
        public ActionResult login(User login_user)
        {
           
                WebRequest wrGETURL;
                String url = ConfigurationManager.AppSettings["validateuser"];
             
            var hash = System.Security.Cryptography.SHA1.Create();
            var encoder = new System.Text.ASCIIEncoding();
            var combined = encoder.GetBytes(login_user.Password ?? "");
            login_user.Password= BitConverter.ToString(hash.ComputeHash(combined)).ToLower().Replace("-", "");
       
        url = url + "_username=" + login_user.UserName + "&_password=" + login_user.Password;
                wrGETURL = WebRequest.Create(url);
                Stream objStream;
                objStream = wrGETURL.GetResponse().GetResponseStream();
                StreamReader objReader = new StreamReader(objStream);
                String isvalid = objReader.ReadLine();
                if (isvalid.Equals("true"))
                {
                    
                    return RedirectToAction("AdminFeedback",login_user);
                }
                else
                {
                    ViewBag.status = "F";
                    return View("qrscan");
                }
            
            
                
           
           
        }
        public ActionResult AdminFeedback(User login_user)
        {
            WebRequest wrGETURL;
            String url = ConfigurationManager.AppSettings["getpermissions"];
            url = url + "_username=" + login_user.UserName + "&_password=" + login_user.Password;
            wrGETURL = WebRequest.Create(url);
            Stream objStream;
            objStream = wrGETURL.GetResponse().GetResponseStream();
            StreamReader objReader = new StreamReader(objStream);
            String permissions = objReader.ReadLine();
           
            string[] permission_list = permissions.Split(' ');
            ViewBag.updatequestions = permission_list[0];
            ViewBag.getresults = permission_list[1];
            ViewBag.newsignup = permission_list[2];
            return View("AdminFeedback",login_user);
        }
        public ActionResult UserFeedback()
        {

            return View();
        }
        public ActionResult temp()
        {

            return View();
        }
        public ActionResult tempview()
        {
            return View();
        }
        public ActionResult Sample()
        {
            return View();
        }
        public ActionResult SampleFeedback()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SampleFeedback(User f)
        {
            String url1, mrno;
            DateTime dt = DateTime.Now;
            String url = ConfigurationManager.AppSettings["feedback"];
            WebRequest wrGETURL;
            String feed = "(1)" + f.ans1 + "(2)" + f.ans2 + "(3)" + f.ans3 + "(4)" + f.ans4 + "(5)" + f.ans5;
            if (f.pin != null)
            {
                url1 = url + f.pin + "&Patient_feeds=" + feed + "&remarks=" + f.remarks + "&Dt=" + dt;
                wrGETURL = WebRequest.Create(url1);
            }

            else if ((f.MRID != null) && (f.pattern != null))
            {
                mrno = f.pattern + "-" + f.MRID;
                url1 = url + mrno + "&Patient_feeds=" + feed + "&remarks=" + f.remarks + "&Dt=" + dt;
                wrGETURL = WebRequest.Create(url1);
            }
            else
            {
                mrno = f.pattern + "-" + f.MRID;
                url1 = url + mrno + f.MRID + "&Patient_feeds=" + feed + "&remarks=" + f.remarks + "&Dt=" + dt;
                wrGETURL = WebRequest.Create(url1);
            }

            // wrGETURL = WebRequest.Create("http://localhost:50872/api/Patients/PostPatient/patient?PIN=0000&Name=sdfsdf&Age=21&Dob=02/04/1995&Gender=1&Nextofkin=sdfsdf&Dt=3/4/2016&Email=yesfsj&MobileNo=878787&LandlineNo=8787878&Address=sfsdf&City=sddf&District=sfsdfs&State=sdsdf&Country=sdfsd&Taluk=sdfs&Pincode=4543453&Visit=yes");
            Stream objStream;
            objStream = wrGETURL.GetResponse().GetResponseStream();
            StreamReader objReader = new StreamReader(objStream);
            //sLine = objReader.ReadLine();
            //Otpnumber details = new Otpnumber();
            // details.otp = sLine;

            return View("Result");

        }
        public ActionResult FeedbackResult()
        {
            WebRequest wrGETURL;
            wrGETURL = WebRequest.Create(getnoofquestions);
            Stream objStream;
            objStream = wrGETURL.GetResponse().GetResponseStream();
            StreamReader objReader = new StreamReader(objStream);
            string sLine = objReader.ReadLine();
            sLine=sLine.Replace("\"","");
            int noofquestions = Int32.Parse(sLine);
            Entities db = new Entities();
          /*  foreach (var feed in db.Feeds)
            {
                decode(feed.Patient_feeds, feed.MRNO,feed.Name);
            }*/
          
            return View();
        }
        public void decode(string feedback, string mrno,string name)
        {
            int pos = 0;
            for (int i = 1; i <=feedback.Length; i = i + 4)
            {
                switch (feedback[i])
                {
                    case '1':
                        pos = i + 2;
                        switch (feedback[pos])
                        {
                            case 'a':
                                q1_a++;
                                q1a_user.Add(mrno);
                                q1a_userName.Add(name);
                                break;
                            case 'b':
                                q1_b++;
                                q1b_user.Add(mrno);
                                q1b_userName.Add(name);
                                break;
                            case 'c':
                                q1_c++;
                                q1c_user.Add(mrno);
                                q1c_userName.Add(name);
                                break;
                        }
                        break;
                    case '2':
                        pos = i + 2;
                        switch (feedback[pos])
                        {
                            case 'a':
                                q2_a++;
                                q2a_user.Add(mrno);
                                q2a_userName.Add(name);
                                break;
                            case 'b':
                                q2_b++;
                                q2b_user.Add(mrno);
                                q2b_userName.Add(name);
                                break;
                            case 'c':
                                q2_c++;
                                q2c_user.Add(mrno);
                                q2c_userName.Add(name);
                                break;


                        }
                        break;
                    case '3':
                        pos = i + 2;
                        switch (feedback[pos])
                        {
                            case 'a':
                                q3_a++;
                                q3a_user.Add(mrno);
                                q3a_userName.Add(name);
                                break;
                            case 'b':
                                q3_b++;
                                q3b_user.Add(mrno);
                                q3b_userName.Add(name);
                                break;
                            case 'c':
                                q3_c++;
                                q3c_user.Add(mrno);
                                q3c_userName.Add(name);
                                break;
                        }
                        break;
                    case '4':
                        pos = i + 2;
                        switch (feedback[pos])
                        {
                            case 'a':
                                q4_a++;
                                q4a_user.Add(mrno);
                                q4a_userName.Add(name);
                                break;
                            case 'b':
                                q4_b++;
                                q4b_user.Add(mrno);
                                q4b_userName.Add(name);
                                break;
                            case 'c':
                                q4_c++;
                                q4c_user.Add(mrno);
                                q4c_userName.Add(name);
                                break;
                        }
                        break;
                    case '5':
                        pos = i + 2;
                        switch (feedback[pos])
                        {
                            case 'a':
                                q5_a++;
                                q5a_user.Add(mrno);
                                q5a_userName.Add(name);
                                break;
                            case 'b':
                                q5_b++;
                                q5b_user.Add(mrno);
                                q5b_userName.Add(name);
                                break;
                            case 'c':
                                q5_c++;
                                q5c_user.Add(mrno);
                                q5c_userName.Add(name);
                                break;
                        }
                        break;
                }

            }
        }
        public ActionResult Result()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UserFeedback(User f)
        {
            
            
            
            //sLine = objReader.ReadLine();
            //Otpnumber details = new Otpnumber();
            // details.otp = sLine;

            return View("Result");
            /*  bool b;
              int pos = 0;
              string mrno;
              string extract = "";
              if (p.pin == "ADMINQR")
              {
                  return View("AdminFeedback");
              }
              else if (p.MRID == null || p.pattern == null)
              {
                  if (p.pin == null)
                  {
                      return View("qrscan");
                  }
                  else if (p.pin.Contains("432-"))
                  {
                      for (int i = 0; i < p.pin.Length; i++)
                      {
                          if (p.pin[i] != '$')
                              p.name = p.name + p.pin[i];
                          else
                          {
                              pos = i;
                              break;
                          }
                      }
                      for (int i = pos + 1; i < p.pin.Length; i++)
                      {
                          extract = extract + p.pin[i];
                      }
                      b = true;
                      p.pin = extract;
                      mrno = extract;

                  }
                  else
                  {
                      b = false;
                      mrno = "";
                  }
              }
              else
              {
                  mrno = p.pattern + "-" + p.MRID;
                  b = mrno.Contains("432-");
              }
              if (b == true)
              {
                  if (mrno.Contains("432-"))
                  {
                      TempData["name"] = p.name;
                      return RedirectToAction("UserFeedback", p);
                  }
                  else
                      return View("qrscan");
              }
              else
             {
                  return View("qrscan");
              }*/
        }
    }
}
