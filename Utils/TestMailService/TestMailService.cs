using System;
using System.Net;
using System.Net.Mail;

namespace XtramileBackend.Utils.TestMailService
{
    public class TestMailService
    {
        public static async Task SendEmailAsyncTest() 
        {
            // Sender's email address and credentials
            string senderEmail = DotNetEnv.Env.GetString("senderEmail");
            string senderPassword = DotNetEnv.Env.GetString("password");

            // Receiver's email address
            string receiverEmail = DotNetEnv.Env.GetString("recieverEmail");

            // SMTP server details
            string smtpServer = "smtp.office365.com";
            int smtpPort = 587; // This may vary based on your SMTP provider

            try
            {
                // Create a new instance of MailMessage class
                MailMessage mail = new MailMessage(senderEmail, receiverEmail);

                // Subject and Body of the email
                mail.Subject = "Test Email from .NET";
                mail.Body = "This is a test email sent from a .NET application.";

                // Create a new instance of SmtpClient class
                SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);

                // Specify SMTP credentials (if required)
                smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);

                // Enable SSL encryption (if required)
                smtpClient.EnableSsl = true;
                // Send the email
                await smtpClient.SendMailAsync(mail);

                Console.WriteLine("Email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email. Error: {ex.Message}");
            }
        }
    }
}
