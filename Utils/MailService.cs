using System.Net;
using System.Net.Mail;
using XtramileBackend.Models.APIModels;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.UnitOfWork;

namespace XtramileBackend.Utils
{
    public class MailService : IMailService
    {
        private readonly IUnitOfWork _unitOfWork;
        public MailService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        private class TravelAdminTeam
        {
            public string Name { get; set; }
            public string Email { get; set; }
        }

        /// <summary>
        /// Retrieves the details of the travel admin team members including their names and emails.
        /// </summary>
        /// <returns>A collection of objects containing the name and email of each travel admin team member.</returns>
        private async Task<IEnumerable<TravelAdminTeam>> GetTravelAdminTeamAsync()
        {
            IEnumerable<TBL_EMPLOYEE> travelAdminData = await _unitOfWork.EmployeeRepository.GetAllAsync();
            IEnumerable<TBL_PROJECT_MAPPING> projectMappingData = await _unitOfWork.ProjectMappingRepository.GetAllAsync();
            IEnumerable<TBL_ROLES> rolesData = await _unitOfWork.RoleRepository.GetAllAsync();
            IEnumerable<TBL_PROJECT> projectData = await _unitOfWork.ProjectRepository.GetAllAsync();
            IEnumerable<TBL_DEPARTMENT> departmentData = await _unitOfWork.DepartmentRepository.GetAllAsync();

            // Query to get travel admin details
            var travelAdminTeam = (from travelAdmin in travelAdminData
                                   join projectMapping in projectMappingData on travelAdmin.EmpId equals projectMapping.EmpId
                                   join project in projectData on projectMapping.ProjectId equals project.ProjectId
                                   join department in departmentData on project.DepartmentId equals department.DepartmentId
                                   join role in rolesData on travelAdmin.RoleId equals role.RoleId
                                   where department.DepartmentCode == "TA" && (travelAdmin.RoleId == 2 || travelAdmin.RoleId == 3)
                                   select new TravelAdminTeam
                                   {
                                       Name = travelAdmin.FirstName + " " + travelAdmin.LastName,
                                       Email = travelAdmin.Email,
                                   }).ToList();

            return travelAdminTeam;
        }

        /// <summary>
        /// Sets the details of the email to be sent to employee on submit
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        public async Task SendToEmployeeOnSubmit(int requestId)
        {
            Mail mail = new Mail();

            //Get necessary information
            TBL_REQUEST request = await _unitOfWork.RequestRepository.GetByIdAsync(requestId);
            TBL_EMPLOYEE employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(request.CreatedBy);

            if (employee != null)
            {
                mail.recipientName = employee.FirstName + " " + employee.LastName;
                mail.recipientEmail = employee.Email;
                mail.emailBody = $"Dear {mail.recipientName},<br><br>Your request with code <b>{request.RequestCode}</b> has been submitted.<br><br>Thank you.<br>";
                await SendMail(mail);
            }
        }

        /// <summary>
        /// Sets the details of the email to be sent to reporting manager on submit
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        public async Task SendToManagersOnSubmit(int requestId)
        {
            Mail mail = new Mail();

            //Get necessary information
            TBL_REQUEST request = await _unitOfWork.RequestRepository.GetByIdAsync(requestId);
            TBL_EMPLOYEE employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(request.CreatedBy);
            TBL_EMPLOYEE manager = await _unitOfWork.EmployeeRepository.GetByIdAsync(employee.ReportsTo ?? -1);

            if (manager != null)
            {
                mail.recipientName = manager.FirstName + " " + manager.LastName;
                mail.recipientEmail = manager.Email;
                mail.requestSubmittedBy = employee.FirstName + " " + employee.LastName;
                mail.emailBody = $"Dear {mail.recipientName},<br><br>A request with code <b>{request.RequestCode}</b> has been submitted by {mail.requestSubmittedBy}.<br>Please verify the details and do the needful.<br><br>Thank you.<br>";
                await SendMail(mail);
            }
        }

        /// <summary>
        /// Sends notification emails to the travel admin team members when a request is submitted.
        /// </summary>
        /// <param name="requestId">The ID of the submitted request.</param>
        /// <returns></returns>
        public async Task SendToTravelAdminTeamOnSubmit(int requestId)
        {
            var travelAdminTeam = await GetTravelAdminTeamAsync();
            foreach (var travelAdmin in travelAdminTeam)
            {
                Mail mail = new Mail();

                //Get necessary information
                TBL_REQUEST request = await _unitOfWork.RequestRepository.GetByIdAsync(requestId);
                TBL_EMPLOYEE employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(request.CreatedBy);

                if (travelAdmin != null)
                {
                    mail.recipientName = travelAdmin.Name;
                    mail.recipientEmail = travelAdmin.Email;
                    mail.requestSubmittedBy = employee.FirstName + " " + employee.LastName;
                    mail.emailBody = $"Dear {mail.recipientName},<br><br>A request with code <b>{request.RequestCode}</b> has been submitted by {mail.requestSubmittedBy}.<br>Please verify the details and do the needful.<br><br>Thank you.<br>";
                    await SendMail(mail);
                }
            }
        }

        /// <summary>
        /// Sends a notification email to the employee upon manager's approval of their request.
        /// </summary>
        /// <param name="requestId">The ID of the request that has been approved by the manager.</param>
        /// <returns></returns>
        public async Task SendToEmployeeOnManagerApproval(int requestId)
        {
            Mail mail = new Mail();

            //Get necessary information
            TBL_REQUEST request = await _unitOfWork.RequestRepository.GetByIdAsync(requestId);
            TBL_EMPLOYEE employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(request.CreatedBy);
            TBL_EMPLOYEE manager = await _unitOfWork.EmployeeRepository.GetByIdAsync(employee.ReportsTo ?? -1);

            if (employee != null)
            {
                mail.recipientName = employee.FirstName + " " + employee.LastName;
                mail.recipientEmail = employee.Email;
                mail.managerName = manager.FirstName + " " + manager.LastName;
                mail.emailBody = $"Dear {mail.recipientName},<br><br>Your request with code <b>{request.RequestCode}</b> has been Approved by {mail.managerName}.<br><br>Thank you.<br>";
                await SendMail(mail);
            }
        }

        /// <summary>
        /// Sends a notification email to the travel admin team upon manager's approval of a request.
        /// </summary>
        /// <param name="requestId">The ID of the request that has been approved by the manager.</param>
        /// <returns></returns>
        public async Task SendToTravelAdminTeamOnManagerApproval(int requestId)
        {
            var travelAdminTeam = await GetTravelAdminTeamAsync();
            foreach (var travelAdmin in travelAdminTeam)
            {
                Mail mail = new Mail();

                //Get necessary information
                TBL_REQUEST request = await _unitOfWork.RequestRepository.GetByIdAsync(requestId);
                TBL_EMPLOYEE employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(request.CreatedBy);
                TBL_EMPLOYEE manager = await _unitOfWork.EmployeeRepository.GetByIdAsync(employee.ReportsTo ?? -1);

                if (travelAdmin != null)
                {
                    mail.recipientName = travelAdmin.Name;
                    mail.recipientEmail = travelAdmin.Email;
                    mail.requestSubmittedBy = employee.FirstName + " " + employee.LastName;
                    mail.emailBody = $"Dear {mail.recipientName},<br><br>The request with code <b>{request.RequestCode}</b> has been Approved by {mail.managerName}.<br><br>Thank you.<br>";
                    await SendMail(mail);
                }
            }
        }

        /// <summary>
        /// Sends an email to employee when their request has been rejected by their reporting manager
        /// </summary>
        /// <param name="requestId">Request Id of the rejected request</param>
        /// <returns></returns>
        public async Task SendToEmployeeOnManagerDenial(int requestId)
        {
            Mail mail = new Mail();

            //Get necessary information
            TBL_REQUEST request = await _unitOfWork.RequestRepository.GetByIdAsync(requestId);
            TBL_EMPLOYEE employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(request.CreatedBy);
            TBL_EMPLOYEE manager = await _unitOfWork.EmployeeRepository.GetByIdAsync(employee.ReportsTo ?? -1);
            TBL_REASON reason = await _unitOfWork.ReasonRepository.GetByIdAsync(request.ReasonId ?? -1);

            if (employee != null)
            {
                mail.recipientName = employee.FirstName + " " + employee.LastName;
                mail.recipientEmail = employee.Email;
                mail.managerName = manager.FirstName + " " + manager.LastName;
                mail.emailBody = $"Dear {mail.recipientName},<br><br>Your request with code <b>{request.RequestCode}</b> has been Rejected by {mail.managerName}.<br>The reason for the rejection is : <b>{reason.Description}</b>.<br><br>Thank you.<br>";
                await SendMail(mail);
            }
        }

        /// <summary>
        /// Sends an email to the travel admin team when a request has been rejected by a manager
        /// </summary>
        /// <param name="requestId">Request Id of the rejected request</param>
        /// <returns></returns>
        public async Task SendToTravelAdminTeamOnManagerDenial(int requestId)
        {
            var travelAdminTeam = await GetTravelAdminTeamAsync();
            foreach (var travelAdmin in travelAdminTeam)
            {
                Mail mail = new Mail();

                //Get necessary information
                TBL_REQUEST request = await _unitOfWork.RequestRepository.GetByIdAsync(requestId);
                TBL_EMPLOYEE employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(request.CreatedBy);
                TBL_EMPLOYEE manager = await _unitOfWork.EmployeeRepository.GetByIdAsync(employee.ReportsTo ?? -1);
                TBL_REASON reason = await _unitOfWork.ReasonRepository.GetByIdAsync(request.ReasonId ?? -1);

                if (travelAdmin != null)
                {
                    mail.recipientName = travelAdmin.Name;
                    mail.recipientEmail = travelAdmin.Email;
                    mail.managerName = manager.FirstName + " " + manager.LastName;
                    mail.emailBody = $"Dear {mail.recipientName},<br><br>The request with code <b>{request.RequestCode}</b> has been Rejected by {mail.managerName}.<br>The reason for the rejection is : <b>{reason.Description}</b>.<br><br>Thank you.<br>";
                    await SendMail(mail);
                }
            }
        }

        /// <summary>
        /// Send an email that notifies the reporting managers that travel options have been sent
        /// </summary>
        /// <param name="requestId">Id of the request for which options are sent</param>
        /// <returns></returns>
        public async Task SendToReportingManagerOnOptionSent(int requestId)
        {
            Mail mail = new Mail();

            TBL_REQUEST request = await _unitOfWork.RequestRepository.GetByIdAsync(requestId);
            TBL_EMPLOYEE employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(request.CreatedBy);
            TBL_EMPLOYEE manager = await _unitOfWork.EmployeeRepository.GetByIdAsync(employee.ReportsTo ?? -1);

            if (manager != null)
            {
                mail.recipientName = manager.FirstName + " " + manager.LastName;
                mail.recipientEmail = manager.Email;
                mail.emailBody = $"Dear {mail.recipientName},<br><br>Some travel options have been sent by the travel admin team for the request with code <b>{request.RequestCode}</b>.<br>Please pick a suitable option.<br><br>Thank you.<br>";
                await SendMail(mail);
            }
        }

        /// <summary>
        /// Sends an email to the travel admin team that notifies them when an option for a request has been selected
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        public async Task SendToTrvaelAdminTeamOnOptionSelection(int requestId)
        {
            var travelAdminTeam = await GetTravelAdminTeamAsync();
            foreach (var travelAdmin in travelAdminTeam)
            {
                Mail mail = new Mail();

                //Get necessary information
                TBL_REQUEST request = await _unitOfWork.RequestRepository.GetByIdAsync(requestId);

                if (travelAdmin != null)
                {
                    mail.recipientName = travelAdmin.Name;
                    mail.recipientEmail = travelAdmin.Email;
                    mail.emailBody = $"Dear {mail.recipientName},<br><br>An option has been selected for the request with code <b>{request.RequestCode}</b>.<br>Please check the option and kindly do the needful.<br><br>Thank you.<br>";
                    await SendMail(mail);
                }
            }
        }

        /// <summary>
        /// Sends an email to the requester when a travel admin personnel has approved their request
        /// </summary>
        /// <param name="requestId">Request Id of the approved request</param>
        /// <returns></returns>
        public async Task SendToEmployeeOnTravelAdminApproval(int requestId)
        {
            Mail mail = new Mail();

            TBL_REQUEST request = await _unitOfWork.RequestRepository.GetByIdAsync(requestId);
            TBL_EMPLOYEE employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(request.CreatedBy);

            if(employee!= null)
            {
                mail.recipientName = employee.FirstName + " " + employee.LastName;
                mail.recipientEmail = employee.Email;
                mail.emailBody = $"Dear {mail.recipientName},<br><br>Your request with code <b>{request.RequestCode}</b> has been Approved by the travel admin team.<br><br>Thank you.<br>";
                await SendMail(mail);
            }
        }

        private async Task SendMail(Mail mailInfo)
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
                mail.Body = mailInfo.emailBody;
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
    }
}

