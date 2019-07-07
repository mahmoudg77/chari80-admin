
using Chair80CP.Requests;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;

namespace Chair80CP.Libs
{
    public class General
    {

        public static Dictionary<string,object> getLangData(string json)
        {
            var obj = new Dictionary<string, object>();
            var returned = new Dictionary<string, object>();
            

            try
            {
                obj = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            }
            catch (Exception)
            {

            }


            foreach (string lang in Chair80CP.BLL.GlobalData.Languages)
            {
                try
                {
                    if (!returned.Keys.Contains(lang))
                       if (obj.Keys.Contains(lang)) returned.Add(lang, obj.FirstOrDefault(a => a.Key == lang).Value);
                    else returned.Add(lang, "");
                }
                catch (Exception)
                {
                    if (!returned.Keys.Contains(lang))
                        returned.Add(lang, "");

                }
            }

            return returned;

        
        }

        public static object getResponse(string url, string data = "", string method = "GET")
        {
            Type ExcelType = Type.GetTypeFromProgID("MSXML2.ServerXMLHTTP");
            dynamic xmlhttp = Activator.CreateInstance(ExcelType);

            // var xmlhttp = CreateObject();
            xmlhttp.open(method, url, false);
            xmlhttp.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            xmlhttp.send(data);
            string s = xmlhttp.responseText;
            JsonConvert.DeserializeObject<object>(s);

            xmlhttp = null;

            return JsonConvert.DeserializeObject<object>(s);
        }


        public static IPResult GetResponse(string url, string data = "", string method = "GET")
        {
            Type ExcelType = Type.GetTypeFromProgID("MSXML2.ServerXMLHTTP");
            dynamic xmlhttp = Activator.CreateInstance(ExcelType);

            // var xmlhttp = CreateObject();
            xmlhttp.open(method, url, false);
            xmlhttp.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            xmlhttp.send(data);
            string s = xmlhttp.responseText;
            //JsonConvert.DeserializeObject<object>(s);

            xmlhttp = null;

            return JsonConvert.DeserializeObject<IPResult>(s);
        }


        public static UserAgent getAgent(string agentdata)
        {
            string u_agent = agentdata.ToLower();
            string bname = "Unknown";
            string platform = "Unknown";
            string version = "";

            //First get the platform? 
            if (u_agent.Contains("android"))
            {
                platform = "android";
            }
            else if (u_agent.Contains("linux"))
            {
                platform = "linux";
            }
            else if (u_agent.Contains("macintosh") || u_agent.Contains("mac os x"))
            {
                platform = "mac";
            }
            else if (u_agent.Contains("windows") || u_agent.Contains("win32"))
            {
                platform = "windows";
            }

            string ub = "Unknown";

            // Next get the name of the useragent yes seperately and for good reason
            if (u_agent.Contains("msie") && !u_agent.Contains("opera"))
            {
                bname = "Internet Explorer";
                ub = "MSIE";
            }
            else if (u_agent.Contains("firefox"))
            {
                bname = "Mozilla Firefox";
                ub = "Firefox";
            }
            else if (u_agent.Contains("chrome"))
            {
                bname = "Google Chrome";
                ub = "Chrome";
            }
            else if (u_agent.Contains("safari"))
            {
                bname = "Apple Safari";
                ub = "Safari";
            }
            else if (u_agent.Contains("opera"))
            {
                bname = "Opera";
                ub = "Opera";
            }
            else if (u_agent.Contains("netscape"))
            {
                bname = "Netscape";
                ub = "Netscape";
            }

            UserAgent u = new UserAgent();
            u.name = bname;
            u.platform = platform;
            return u;

        }

        /// <summary>
        /// Equivalent to PHP preg_match but only for 3 requied parameters
        /// </summary>
        /// <param name="regex"></param>
        /// <param name="input"></param>
        /// <param name="matches"></param>
        /// <returns></returns>
        public static bool preg_match(Regex regex, string input, out List<string> matches)
        {
            var match = regex.Match(input);
            var groups = (from object g in match.Groups select g.ToString()).ToList();

            matches = groups;
            return match.Success;
        }

        public static string fetchEntityError(DbEntityValidationException e)
        {
            string s = "";
            foreach (var eve in e.EntityValidationErrors)
            {
                s += string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                    eve.Entry.Entity.GetType().Name, eve.Entry.State);
                foreach (var ve in eve.ValidationErrors)
                {
                    s += string.Format("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                }
            }
            return s;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="mTo"></param>
        /// <param name="mSubject"></param>
        /// <param name="mBody"></param>
        /// <param name="SystemName"></param>
        /// <param name="mCC"></param>
        /// <param name="mBCC"></param>
        /// <param name="attachmentsFiles"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool SendMail(string mTo, string mSubject, string mBody, string SystemName = "Metookey", string mCC = "", string mBCC = "", string attachmentsFiles = "", bool isHTML = false)
        {
            if (string.IsNullOrEmpty(mTo) | string.IsNullOrEmpty(mSubject) | string.IsNullOrEmpty(mBody))
                return false;


            System.Net.Mail.SmtpClient SmtpClient = new System.Net.Mail.SmtpClient();
            MailMessage message = new MailMessage();

            try
            {


                // MailAddress fromAddress = new MailAddress(txtEmail.Text, txtName.Text)

                // You can specify the host name or ipaddress of your server
                // Default in IIS will be localhost 
                //var configurationFile = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath + "/Web.config");
                //MailSettingsSectionGroup mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings");

                //int port = int.Parse(ConfigurationManager.AppSettings["SMTPPort"]);
                //string host = ConfigurationManager.AppSettings["SMTPServer"];
                //string password = ConfigurationManager.AppSettings["SMTPPassword"];
                //string username = ConfigurationManager.AppSettings["SMTPLogin"];
                //string From = ConfigurationManager.AppSettings["EmailSender"];

                int port = int.Parse(Settings.Get("smtp_port"));
                string host = Settings.Get("smtp_host");
                string password = Settings.Get("smtp_password");
                string username = Settings.Get("smtp_user");
                string From = Settings.Get("email_sender");
                string SenderName = Settings.Get("email_name");


                SmtpClient.Host = host;
                SmtpClient.Port = port; // 587

                if (!(attachmentsFiles == null) && !(attachmentsFiles == ""))
                {
                    string[] Attach = attachmentsFiles.Split(new char[] { ';' });
                    var loopTo = Attach.Count() - 1;
                    for (int n = 0; n <= loopTo; n++)
                    {
                        if (!string.IsNullOrEmpty(Attach[n]))
                            message.Attachments.Add(new System.Net.Mail.Attachment(Attach[n].Replace("/", @"\"), "text/html"));
                    }
                }

                SmtpClient.Credentials = new System.Net.NetworkCredential(username, password);

                SmtpClient.EnableSsl = true;
                // Default port will be 25

                // From address will be given as a MailAddress Object
                message.From = new MailAddress(From, SystemName == "" ? SenderName : SystemName);

                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.SubjectEncoding = System.Text.Encoding.UTF8;

                // To address collection of MailAddress
                string[] Tos = mTo.Split(new char[] { ';' });
                var loopTo1 = Tos.Count() - 1;
                for (int n = 0; n <= loopTo1; n++)
                {
                    if (!string.IsNullOrEmpty(Tos[n]))
                        message.To.Add(Tos[n]);
                }
                message.Subject = mSubject;

                // CC and BCC optional
                // MailAddressCollection class is used to send the email to various users
                // You can specify Address as new MailAddress("admin1@yoursite.com")
                if (!(mCC == null))
                {
                    string[] CCs = mCC.Split(new char[] { ';' }); ;
                    var loopTo2 = CCs.Count() - 1;
                    for (int n = 0; n <= loopTo2; n++)
                    {
                        if (!string.IsNullOrEmpty(CCs[n]))
                            message.CC.Add(CCs[n]);
                    }
                }

                // You can specify Address directly as string
                if (!(mBCC == null))
                {
                    string[] BCCs = mBCC.Split(new char[] { ';' }); ;
                    var loopTo3 = BCCs.Count() - 1;
                    for (int n = 0; n <= loopTo3; n++)
                    {
                        if (!string.IsNullOrEmpty(BCCs[n]))
                            message.Bcc.Add(BCCs[n]);
                    }
                }
                // Body can be Html or text format
                // Specify true if it  is html message
                message.IsBodyHtml = isHTML;



                message.Body = mBody;

                // Send SMTP mail
                SmtpClient.Send(message);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public static string ApplicationUrl()
        {
            return string.Format("{0}://{1}{2}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.ServerVariables["HTTP_HOST"],HttpContext.Current.Request.ApplicationPath.Equals("/") ? string.Empty : HttpContext.Current.Request.ApplicationPath);
        }
        public static string HostUrl()
        {
            return string.Format("{0}://{1}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.ServerVariables["HTTP_HOST"]);
        }
    }
}