using Microsoft.Extensions.Primitives;
using System.Dynamic;
using XtramileBackend.Models.APIModels;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.UnitOfWork;
using XtramileBackend.Utils;

namespace XtramileBackend.Services.RequestService
{
    public class RequestServices : IRequestServices
    {

        private readonly IUnitOfWork _unitOfWork;
        private Random random;

        public RequestServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));


            // Initialize Random with a unique seed (e.g., based on the current time)
            random = new Random(Guid.NewGuid().GetHashCode());
        }


        public Task<IEnumerable<TBL_REQUEST>> GetAllRequestAsync()
        {
            try
            {
                var request = _unitOfWork.RequestRepository.GetAllAsync();
                return request;
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting request: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }

        }

        public async Task AddRequestAsync(TBL_REQUEST request)
        {
            try
            {
                await _unitOfWork.RequestRepository.AddAsync(request);         
                _unitOfWork.Complete();

                int empId = request.CreatedBy;

                //Mail information to be sent to employee on submit

                string requestCode = request.RequestCode;

                Mail sendToEmployee = new Mail();

                TBL_EMPLOYEE employeeData = await _unitOfWork.EmployeeRepository.GetByIdAsync(empId);

                if (employeeData != null)
                {
                    sendToEmployee.recipientName = employeeData.FirstName + " " + employeeData.LastName;
                    sendToEmployee.recipientEmail = employeeData.Email;
                }

                sendToEmployee.requestCode = requestCode;

                sendToEmployee.mailContext = "submit";

                MailService.SendMail(sendToEmployee);


                //mail information to be sent to reporting manager on submit

                Mail sendToManager = new Mail();

                TBL_EMPLOYEE managerData = await _unitOfWork.EmployeeRepository.GetByIdAsync(employeeData.ReportsTo ?? -1);
                if (managerData != null)
                {
                    sendToManager.recipientName = managerData.FirstName + " " + managerData.LastName;
                    sendToManager.recipientEmail = managerData.Email;
                    sendToManager.requestSubmittedBy = employeeData.FirstName + " " + employeeData.LastName;
                }

                sendToManager.requestCode = requestCode;
                sendToManager.mailContext = "sendToHigherPersonnelOnSubmit";

                MailService.SendMail(sendToManager);


                //mail information to be sent to travel admins on submit

                // Creating a list to store individual mails for each travel admin
                List<Mail> travelAdminMails = new List<Mail>();

                // Retrieve data from repositories
                IEnumerable<TBL_EMPLOYEE> travelAdminData = await _unitOfWork.EmployeeRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT_MAPPING> projectMappingData = await _unitOfWork.ProjectMappingRepository.GetAllAsync();
                IEnumerable<TBL_ROLES> rolesData = await _unitOfWork.RoleRepository.GetAllAsync();
                IEnumerable<TBL_PROJECT> projectData = await _unitOfWork.ProjectRepository.GetAllAsync();
                IEnumerable<TBL_DEPARTMENT> departmentData = await _unitOfWork.DepartmentRepository.GetAllAsync();

                // Query to get travel admin details
                var travelAdminEmails = (from travelAdmin in travelAdminData
                                         join projectMapping in projectMappingData on travelAdmin.EmpId equals projectMapping.EmpId
                                         join project in projectData on projectMapping.ProjectId equals project.ProjectId
                                         join department in departmentData on project.DepartmentId equals department.DepartmentId
                                         join role in rolesData on travelAdmin.RoleId equals role.RoleId
                                         where department.DepartmentCode == "TA" && (travelAdmin.RoleId == 2 || travelAdmin.RoleId == 3)
                                         select new { travelAdmin.FirstName, travelAdmin.LastName, travelAdmin.Email }).ToList();

                // Iterate through each travel admin and create a separate mail for each
                foreach (var travelAdminAndManager in travelAdminEmails)
                {
                    // Create a new mail instance for each travel admin
                    Mail sendToTravelAdmin = new Mail();

                    // Set recipient details
                    sendToTravelAdmin.recipientName = travelAdminAndManager.FirstName + " " + travelAdminAndManager.LastName;
                    sendToTravelAdmin.recipientEmail = travelAdminAndManager.Email;

                    // Set other mail details
                    sendToTravelAdmin.requestCode = requestCode;
                    sendToTravelAdmin.mailContext = "sendToHigherPersonnelOnSubmit";
                    sendToTravelAdmin.requestSubmittedBy = employeeData.FirstName + " " + employeeData.LastName;

                    // Add the mail to the list
                    travelAdminMails.Add(sendToTravelAdmin);
                }

                // Send mails to all travel admins
                foreach (var mail in travelAdminMails)
                {
                    MailService.SendMail(mail);
                }


            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while adding a request: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }

        }


        //Generate Random Code
        public string GenerateRandomCode(int suffix)
        {
            int prefix = random.Next(100, 1000);
            string randomCode = $"{prefix}{suffix}";
            return randomCode;
        }


        // Async Method to get request ID by accepting EmpID as an argument
        public async Task<int> GetRequestIdByRequestCode(string requestCode)
        {
            try
            {
                IEnumerable<TBL_REQUEST> requestData = await _unitOfWork.RequestRepository.GetAllAsync();

                var requestId = (from item in requestData
                                 where item.RequestCode == requestCode
                                 select item.RequestId).FirstOrDefault();

                return requestId;

            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting request id: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
            //EOF
        }





    }
}
