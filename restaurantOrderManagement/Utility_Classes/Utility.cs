
using Core;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;


public sealed class Utility
    {
   


    private static readonly string[] UriRfc3986CharsToEscape = new[] { "!", "*", "'", "(", ")" };
    internal static string EscapeUriDataStringRfc3986(string value)
    {
        StringBuilder escaped = new StringBuilder(Uri.EscapeDataString(value));
        for (int i = 0; i < UriRfc3986CharsToEscape.Length; i++)
        {
            escaped.Replace(UriRfc3986CharsToEscape[i], Uri.HexEscape(UriRfc3986CharsToEscape[i][0]));
        }
        return escaped.ToString();
    }

    public static string SendSMS(string sendToPhoneNumber, string messageBody, string DLT_TE_ID)
    {
        string sender_name = AppSettingHelper.GetSetting("OtherInfo", "SMSSenderName");
       
        string SMSAuthKey = AppSettingHelper.GetSetting("OtherInfo", "SMSAuthKey");

        string result = "";
        WebRequest request = null;
        HttpWebResponse response = null;
        string encodedSMSBody = EscapeUriDataStringRfc3986(messageBody);

        try
        {
            //1 = sendToPhoneNumber, 3 = MessageBody, 5 = userid, 7 = password, (0, 2, 4, 6, 8, 9, 10 = &)
            //string url = string.Format(ConfigurationManager.AppSettings["SMSGatewayUri"].ToString(), "&", sendToPhoneNumber, "&", encodedSMSBody, "&", ConfigurationManager.AppSettings["SMSGatewayUserID"].ToString(), "&", ConfigurationManager.AppSettings["SMSGatewayPassword"].ToString(), "&", "&", "&");
            //string url = string.Format(ConfigurationManager.AppSettings["SMSGatewayUri"].ToString(), ConfigurationManager.AppSettings["SMSGatewayUserID"], "&", ConfigurationManager.AppSettings["SMSGatewayPassword"], "&", ConfigurationManager.AppSettings["SMSSenderName"], "&", sendToPhoneNumber, "&", encodedSMSBody);
            string url = "http://api.msg91.com/api/sendhttp.php?country=91&sender=" + sender_name.ToString() + "&route=4&mobiles=" + sendToPhoneNumber + "&authkey=" + SMSAuthKey.ToString() + "&message=" + messageBody + "&DLT_TE_ID=" + DLT_TE_ID;
            request = WebRequest.Create(url);

            //in case u work behind proxy, uncomment the commented code and provide correct details
            /*WebProxy proxy = new WebProxy("http://proxy:80/",true);
            proxy.Credentials = new 
            NetworkCredential("userId","password", "Domain");
            request.Proxy = proxy;*/

            // Send the 'HttpWebRequest' and wait for response.
            response = (HttpWebResponse)request.GetResponse();

            Stream stream = response.GetResponseStream();
            Encoding ec = System.Text.Encoding.GetEncoding("utf-8");
            StreamReader reader = new System.IO.StreamReader(stream, ec);
            result = reader.ReadToEnd();
            reader.Close();
            stream.Close();

            return result;
        }
        catch (Exception exp)
        {
           // Utility.SaveSystemErrorLog(exp);
            //return exp.Message;
            throw new Exception(exp.Message);


        }
        finally
        {
            if (response != null)
                response.Close();
        }
    }

    public static void SendHtmlFormattedEmail(string recepientEmail, string subject, string body, bool hasAttachment = false, string FilePath = "")
    {
        using (MailMessage mailMessage = new MailMessage())
        {
            
            string UserName = AppSettingHelper.GetSetting("OtherInfo", "UserName");
            string Password = AppSettingHelper.GetSetting("OtherInfo", "Password");
            string Port = AppSettingHelper.GetSetting("OtherInfo", "Port");

            mailMessage.From = new MailAddress(UserName);
            mailMessage.Subject = subject;

            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body, Encoding.UTF8, MediaTypeNames.Text.Html);
            //  LinkedResource logo = new LinkedResource(System.Web.HttpContext.Current.Server.MapPath(""));//("~/images/logo.png"));
            //logo.ContentId = "companylogo";
            // htmlView.LinkedResources.Add(logo);
            mailMessage.AlternateViews.Add(htmlView);
            mailMessage.Body = body;

            //Attachment checking & creation
            if (hasAttachment && !string.IsNullOrEmpty(FilePath))
            {
                Attachment objAttachment = new Attachment(FilePath);
                mailMessage.Attachments.Add(objAttachment);
            }

            mailMessage.SubjectEncoding = Encoding.UTF8;
            mailMessage.BodyEncoding = Encoding.UTF8;
            mailMessage.IsBodyHtml = true;
            mailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            //mailMessage.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(body, new ContentType("text/plain")));
            List<string> Mailidlist = new List<string>();
            Mailidlist = (recepientEmail).Split(';').ToList();

            if (Mailidlist != null && Mailidlist.Count() > default(int))
            {
                foreach (var mail in Mailidlist)
                {
                    mailMessage.To.Add(new MailAddress(mail.Trim()));
                }
            }

            SmtpClient smtp = new SmtpClient();
            string Host = AppSettingHelper.GetSetting("OtherInfo", "Host");
            smtp.Host = Host;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;//later added

            string IsSSL= AppSettingHelper.GetSetting("OtherInfo", "EnableSsl");
            smtp.EnableSsl = Convert.ToBoolean(IsSSL);
            System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();


            NetworkCred.UserName = UserName;
            NetworkCred.Password = Password;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = NetworkCred;
            smtp.Port = int.Parse(Port);

            //TLS Addition
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            //Certificate hack if “The remote certificate is invalid according to the validation procedure.” using 587 port in SMTP server
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

            smtp.Send(mailMessage);
            /*Object state = mailMessage;
            smtp.SendAsync(mailMessage, state);*/
        }
    }

    public static void SaveSystemErrorLog(Exception e, string IPAddress = "")
    {

        StringBuilder sbErrorText = new StringBuilder();
        string FileName = DateTime.Now.ToString("dd-MMM-yyyy");

        string ErrorLogPath = AppSettingHelper.GetSetting("OtherInfo", "ErrorLogPath");


        sbErrorText.Append(Environment.NewLine);
        sbErrorText.Append(Environment.NewLine);
        sbErrorText.Append("======================================================================================" + Environment.NewLine);
        sbErrorText.Append("@  Error Date Time  : " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt") + Environment.NewLine);
        sbErrorText.Append("@  Error At  : " + IPAddress + " " + Environment.NewLine);
        sbErrorText.Append("--------------------------------------------------------------------------------------" + Environment.NewLine);
        sbErrorText.Append("@  Message            : " + e.Message + Environment.NewLine);
        sbErrorText.Append("@  Inner Exception    : " + e.InnerException + Environment.NewLine);
        sbErrorText.Append("@  Source             : " + e.Source + Environment.NewLine);
        sbErrorText.Append("@  Stack Trace        : " + e.StackTrace + Environment.NewLine);
        sbErrorText.Append("@  TargetSite         : " + e.Data + Environment.NewLine);
        sbErrorText.Append("--------------------------------------------------------------------------------------" + Environment.NewLine);
        string Path = ErrorLogPath + @"\" + FileName + ".txt";
        string TemplatePath = ErrorLogPath + @"\Errors.txt";
        if (!File.Exists(Path))
        {
            try
            {
                File.Copy(TemplatePath, Path);
            }
            catch (Exception ex)
            {

            }
        }

        if (File.Exists(Path))
        {
            try
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter(Path, true);
                file.WriteLine(sbErrorText);
                file.Close();
            }
            catch (Exception ex)
            {

            }

        }


    }
}

