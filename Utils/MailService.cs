using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using XtramileBackend.Models.APIModels;

namespace XtramileBackend.Utils
{
    public class MailService
    {
        public static async Task SendMail(Mail mailInfo)
        {
            try
            {
                string senderEmail = GetSenderEmail();
                string senderPassword = GetSenderPassword();
                string recipientEmail = mailInfo.recipientEmail;

                if (string.IsNullOrEmpty(senderEmail) || string.IsNullOrEmpty(senderPassword) || string.IsNullOrEmpty(recipientEmail))
                {
                    Console.WriteLine("Sender email, sender password, or recipient email is missing.");
                    return;
                }

                MailMessage mail = new MailMessage(senderEmail, recipientEmail);
                mail.Subject = "Travel Request Status";
                string emailContent = GetEmailContent(mailInfo);
                mail.Body = emailContent;
                mail.IsBodyHtml = true;

                using (SmtpClient smtpClient = new SmtpClient("smtp.office365.com", 587))
                {
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
                    smtpClient.EnableSsl = true;

                    await smtpClient.SendMailAsync(mail); // Await SendMailAsync method

                    Console.WriteLine("Email sent successfully!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }
        }

        private static string GetSenderEmail()
        {
            // Fetch sender email from a secure storage or configuration
            return DotNetEnv.Env.GetString("senderEmail");
        }

        private static string GetSenderPassword()
        {
            // Fetch sender password from a secure storage or configuration
            return DotNetEnv.Env.GetString("senderPassword");
        }
        private static string GetEmailContent(Mail mailInfo)
        {
            switch (mailInfo.mailContext)
            {
                case "submit":
                    return $"Dear {mailInfo.recipientName},\n\nYour request with code <b>{mailInfo.requestCode}</b> has been submitted.\nThank you\n";
                case "approve":
                    return $"Dear {mailInfo.recipientName},\n\nThe request with code <b>{mailInfo.requestCode}</b> has been Approved by {mailInfo.managerName}\nThank you\n";
                case "reject":
                    return $"Dear {mailInfo.recipientName},\n\nThe request with code <b>{mailInfo.requestCode}</b> has been Rejected by {mailInfo.managerName}\nThe reason for your rejection is {mailInfo.reasonForRejection}\nThank you\n";
                case "options":
                    return $"Dear {mailInfo.recipientName},\n\nOptions for your request with code <b>{mailInfo.requestCode}</b> has been sent by travel admin {mailInfo.managerName}\nPick the tickets for the trip based on your interest.\nThank you\n";
                case "sendToHigherPersonnelOnSubmit":
                    return $"Dear {mailInfo.recipientName},\n\nA request with code <b>{mailInfo.requestCode}</b> has been submitted by {mailInfo.requestSubmittedBy}\nPlease verify the details and do the needful.\nThank you\n";
                case "sendToTAOnOptionSelect":
                    return $"Dear {mailInfo.recipientName},\n\nAn option has been selected for the request with code <b>{mailInfo.requestCode}</b> by {mailInfo.requestSubmittedBy}\nPlease verify the option and do the needful.\nThank you\n";
                default: return "xtramile";
            }
        }
    }
}
