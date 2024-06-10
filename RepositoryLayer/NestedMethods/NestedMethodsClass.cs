using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RepositoryLayer.NestedMethods
{
    public class NestedMethodsClass
    {
        

        //password regex
        public static bool IsStrongPassword(string password)
        {
            string pattern = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\W)(?=.*\d).{8,}$";
            return Regex.IsMatch(password, pattern);
        }

        //email regex

        public static bool IsValidGmailAddress(string email)
        {

            string pattern = @"^[a-z0-9]+@gmail\.com$";
            return Regex.IsMatch(email, pattern);
        }


        //phone number regex
        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            string pattern = @"^\d{10,15}$";
            return Regex.IsMatch(phoneNumber, pattern);
        }


        //SendMail Logic

        /* public static void sendMail(String ToMail, String otp)
         {
             System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
             try
             {
                 mailMessage.From = new System.Net.Mail.MailAddress("pujakashyapkashyap@outlook.com", "FUNDOO NOTES");
                 mailMessage.To.Add(ToMail);
                 mailMessage.Subject = "Change password for Fundoo Notes";
                 mailMessage.Body = "This is your otp please enter to change password " + otp;
                 mailMessage.IsBodyHtml = true;
                 System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient("smtp-mail.outlook.com");

                 // Specifies how email messages are delivered. Here Email is sent through the network to an SMTP server.
                 smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;

                 // Set the port for Outlook's SMTP server
                 smtpClient.Port = 587; // Outlook SMTP port for TLS/STARTTLS

                 // Enable SSL/TLS
                 smtpClient.EnableSsl = true;

                 string loginName = "pujakashyapkashyap@outlook.com";
                 string loginPassword = "Pooja@123";

                 System.Net.NetworkCredential networkCredential = new System.Net.NetworkCredential(loginName, loginPassword);
                 smtpClient.UseDefaultCredentials = false;
                 smtpClient.Credentials = networkCredential;

                 smtpClient.Send(mailMessage);
             }
             catch (Exception ex)
             {
                 Console.WriteLine("Exception caught: " + ex.Message);
             }
             finally
             {
                 mailMessage.Dispose();
             }
         }*/
    }
}
