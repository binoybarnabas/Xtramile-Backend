using System.Net.Mail;
using System.Net;
using Azure.Core;
using XtramileBackend.Models.APIModels;

namespace XtramileBackend.Utils
{
    public class MailService
    {
        public static void SendMail(Mail mailInfo)
        {
            DotNetEnv.Env.Load();
            //sender email and password
            string senderEmail = DotNetEnv.Env.GetString("senderEmail");
            string senderPassword = DotNetEnv.Env.GetString("senderPassword");
            //receipient email 
            string recipientEmail = mailInfo.recipientEmail;

            if (!string.IsNullOrEmpty(senderEmail) && !string.IsNullOrEmpty(recipientEmail))
            {
                // Mail message
                MailMessage mail = new MailMessage(senderEmail, recipientEmail);
                mail.Subject = "Travel Request Status";
                string salutation, body, emailContent;

                switch (mailInfo.mailContext) {

                    case "submit":

                        // Salutation
                        salutation = $"Dear {mailInfo.recipientName},"; // Replace "John" with the recipient's name

                        // Body of the email
                        body = "Your request has been submitted.\n" +
                                      "Thank you \n\n";

                        // Concatenate the salutation and body
                        emailContent = salutation + "\n\n" + body;
                        mail.Body = emailContent;
                        break;
                    case "approve":

                        // Salutation
                        salutation = $"Dear {mailInfo.recipientName},"; // Replace "John" with the recipient's name

                        // Body of the email
                        body = $"Your request has been Approved by {mailInfo.managerName}\n" +
                                      "Thank you \n\n";

                        // Concatenate the salutation and body
                        emailContent = salutation + "\n\n" + body;
                        mail.Body = emailContent;
                        break;

                    case "reject":

                        // Salutation
                        salutation = $"Dear {mailInfo.recipientName},"; // Replace "John" with the recipient's name

                        // Body of the email
                        body = $"Your request has been Rejected by {mailInfo.managerName}\n" + 
                                $"The reason for your rejection is {mailInfo.reasonForRejection} \n" +
                                      "Thank you \n\n";

                        // Concatenate the salutation and body
                        emailContent = salutation + "\n\n" + body;
                        mail.Body = emailContent;
                        break;

                    case "options":

                        // Salutation
                        salutation = $"Dear {mailInfo.recipientName},";

                        // Body of the email
                        body = $"Your request has been Approved by travel admin {mailInfo.managerName}\n" +
                                "Pick the tickets for the trip based on your interest.\n" +
                                      "Thank you \n\n";

                        // Concatenate the salutation and body
                        emailContent = salutation + "\n\n" + body;
                        mail.Body = emailContent;
                        break;


                }

                // SMTP client configuration
                SmtpClient smtpClient = new SmtpClient("smtp-mail.outlook.com");
                smtpClient.Port = 587; // Port for SMTP 
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
                smtpClient.EnableSsl = true; // Enable SSL/TLS

                try
                {
                    // Send the email
                    smtpClient.Send(mail);
                    Console.WriteLine("Email sent successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send email: {ex.Message}");
                }
            }
        }

        
    }
}
