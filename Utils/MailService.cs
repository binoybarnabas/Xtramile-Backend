using System.Net.Mail;
using System.Net;
using Azure.Core;
using XtramileBackend.Models.APIModels;

namespace XtramileBackend.Utils
{
    public class MailService
    {

        /// <summary>
        /// To send mail based on activities like request submission from employees, request approval, request rejection, when the travel admin send ticket options to 
        /// the employee.
        /// </summary>
        /// <param name="mailInfo"></param>
        public static async Task SendMail(Mail mailInfo)
        {
            await Task.Run(() =>
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

                    switch (mailInfo.mailContext)
                    {

                        case "submit":

                            // Salutation
                            salutation = $"Dear {mailInfo.recipientName},\n"; // Replace "John" with the recipient's name

                            // Body of the email
                            body = $"Your request with code <b>{mailInfo.requestCode}</b> has been submitted.\n" +
                                          "Thank you \n\n";

                            // Concatenate the salutation and body
                            emailContent = salutation + "\n\n" + body;
                            mail.Body = emailContent;
                            mail.IsBodyHtml = true;
                            break;

                        case "approve":

                            // Salutation
                            salutation = $"Dear {mailInfo.recipientName},\n"; // Replace "John" with the recipient's name

                            // Body of the email
                            body = $"The request with code <b>{mailInfo.requestCode}</b> has been Approved by {mailInfo.managerName}\n" +
                                          "Thank you \n\n";

                            // Concatenate the salutation and body
                            emailContent = salutation + "\n\n" + body;
                            mail.Body = emailContent;
                            mail.IsBodyHtml = true;
                            break;

                        case "reject":

                            // Salutation
                            salutation = $"Dear {mailInfo.recipientName},\n"; // Replace "John" with the recipient's name

                            // Body of the email
                            body = $"The request with code <b>{mailInfo.requestCode}</b> has been Rejected by {mailInfo.managerName}\n" +
                                    $"The reason for your rejection is {mailInfo.reasonForRejection} \n" +
                                          "Thank you \n\n";

                            // Concatenate the salutation and body
                            emailContent = salutation + "\n\n" + body;
                            mail.Body = emailContent;
                            mail.IsBodyHtml = true;
                            break;

                        case "options":

                            // Salutation
                            salutation = $"Dear {mailInfo.recipientName},\n";

                            // Body of the email
                            body = $"Your request with code <b>{mailInfo.requestCode}</b> has been Approved by travel admin {mailInfo.managerName}\n" +
                                    "Pick the tickets for the trip based on your interest.\n" +
                                          "Thank you \n\n";

                            // Concatenate the salutation and body
                            emailContent = salutation + "\n\n" + body;
                            mail.Body = emailContent;
                            mail.IsBodyHtml = true;
                            break;

                        case "sendToHigherPersonnelOnSubmit":

                            // Salutation
                            salutation = $"Dear {mailInfo.recipientName},\n";

                            // Body of the email
                            body = $"A request with code <b>{mailInfo.requestCode}</b> has been submitted by {mailInfo.requestSubmittedBy}\n" +
                                    "Please verify the details and do the needful.\n" +
                                          "Thank you \n\n";

                            // Concatenate the salutation and body
                            emailContent = salutation + "\n\n" + body;
                            mail.Body = emailContent;
                            mail.IsBodyHtml = true;
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
                        smtpClient.SendMailAsync(mail);
                        Console.WriteLine("Email sent successfully!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to send email: {ex.Message}");
                    }
                }
            });
        }

    }
}