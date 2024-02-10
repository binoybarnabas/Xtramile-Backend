using System.Net.Mail;
using System.Net;
using Azure.Core;

namespace XtramileBackend.Utils
{
    public class MailService
    {
       
        public static void SendMail(string recipientEmail,string recipientName,string mailContext,string managerName = "",string reasonForRejection)
        {
            DotNetEnv.Env.Load();
            //sender email and password
            string senderEmail = DotNetEnv.Env.GetString("senderEmail");
            string senderPassword = DotNetEnv.Env.GetString("senderPassword");
            //receipient email 
            string recipient = recipientEmail;

            if (!string.IsNullOrEmpty(senderEmail) && !string.IsNullOrEmpty(recipientEmail))
            {
                // Mail message
                MailMessage mail = new MailMessage(senderEmail, recipientEmail);
                mail.Subject = "Travel Request Status";
                string salutation, body, emailContent;

                switch (mailContext) {

                    case "submit":

                        // Salutation
                        salutation = $"Dear {recipientName}, \n\n"; // Replace "John" with the recipient's name

                        // Body of the email
                        body = "Your request has been submitted.\n" +
                                      "Thank you \n\n";

                        // Concatenate the salutation and body
                        emailContent = salutation + "\n\n" + body;
                        mail.Body = emailContent;
                        break;
                    case "approve":

                        // Salutation
                        salutation = $"Dear {recipientName}, \n\n"; // Replace "John" with the recipient's name

                        // Body of the email
                        body = $"Your request has been Approved by {managerName}\n" +
                                      "Thank you \n\n";

                        // Concatenate the salutation and body
                        emailContent = salutation + "\n\n" + body;
                        mail.Body = emailContent;
                        break;

                    case "reject":

                        // Salutation
                        salutation = $"Dear {recipientName}, \n\n"; // Replace "John" with the recipient's name

                        // Body of the email
                        body = $"Your request has been Rejected by {managerName}\n" + 
                                $"The reason for your rejection is {reasonForRejection} \n" +
                                      "Thank you \n\n";

                        // Concatenate the salutation and body
                        emailContent = salutation + "\n\n" + body;
                        mail.Body = emailContent;
                        break;

                    case "options":

                        // Salutation
                        salutation = $"Dear {recipientName}, \n\n"; // Replace "John" with the recipient's name

                        // Body of the email
                        body = $"Your request has been Approved by travel admin {managerName}\n" +
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
