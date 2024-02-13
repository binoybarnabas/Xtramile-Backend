using Moq;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Repositories.EmployeeRepository;
using XtramileBackend.Repositories.ProjectRepository;
using XtramileBackend.Repositories.RequestRepository;
using XtramileBackend.Repositories.RequestStatusRepository;
using XtramileBackend.Repositories.StatusRepository;
using XtramileBackend.Services.TravelAdminService;
using XtramileBackend.UnitOfWork;
using Xunit;


namespace YourNamespace.Tests
{
    public class TravelAdminServiceTests
    {
        [Fact]
        public async Task OnGoingTravel_ReturnsOngoingTravelAdminList()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            // Mock employee data
            var employeeData = new List<TBL_EMPLOYEE>
            {
                new TBL_EMPLOYEE { EmpId = 1, FirstName = "John", LastName = "Doe" }
            }.AsQueryable();

            // Mock status data
            var statusData = new List<TBL_STATUS>
            {
                new TBL_STATUS { StatusId = 1, StatusCode = "OG" }
            }.AsQueryable();

            // Mock request status mapping data
            var requestStatusMappingData = new List<TBL_REQ_APPROVE>
            {
                new TBL_REQ_APPROVE { RequestId = 1, EmpId = 1, PrimaryStatusId = 1, date = DateTime.Now }
            }.AsQueryable();

            // Mock request data
            var requestData = new List<TBL_REQUEST>
            {
                new TBL_REQUEST { RequestId = 1, ProjectId = 1, SourceCity = "CityA", DestinationCity = "CityB" }
            }.AsQueryable();

            // Mock project data
            var projectData = new List<TBL_PROJECT>
            {
                new TBL_PROJECT { ProjectId = 1, ProjectCode = "P001", ProjectName = "Project A" }
            }.AsQueryable();

            // Mock EmployeeRepository
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(employeeData);

            // Mock StatusRepository
            var statusRepositoryMock = new Mock<IStatusRepository>();
            statusRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(statusData);

            // Mock RequestStatusRepository
            var requestStatusRepositoryMock = new Mock<IRequestStatusRepository>();
            requestStatusRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(requestStatusMappingData);

            // Mock RequestRepository
            var requestRepositoryMock = new Mock<IRequestRepository>();
            requestRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(requestData);

            // Mock ProjectRepository
            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(projectData);

            // Set up UnitOfWork mock
            unitOfWorkMock.Setup(uow => uow.EmployeeRepository).Returns(employeeRepositoryMock.Object);
            unitOfWorkMock.Setup(uow => uow.StatusRepository).Returns(statusRepositoryMock.Object);
            unitOfWorkMock.Setup(uow => uow.RequestStatusRepository).Returns(requestStatusRepositoryMock.Object);
            unitOfWorkMock.Setup(uow => uow.RequestRepository).Returns(requestRepositoryMock.Object);
            unitOfWorkMock.Setup(uow => uow.ProjectRepository).Returns(projectRepositoryMock.Object);

            var travelAdminService = new TravelAdminService(unitOfWorkMock.Object);

            // Act
            var result = await travelAdminService.OnGoingTravel();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result); // Assuming there is only one ongoing travel request in the test data
            var ongoingTravel = result.First();
            Assert.Equal(1, ongoingTravel.requestId);
            Assert.Equal("P001", ongoingTravel.ProjectCode);
            Assert.Equal("Project A", ongoingTravel.ProjectName);
            Assert.Equal("John Doe", ongoingTravel.Name);
            Assert.Equal("CityA", ongoingTravel.SourceCity);
            Assert.Equal("CityB", ongoingTravel.DestinationCity);
        }
    }
}
