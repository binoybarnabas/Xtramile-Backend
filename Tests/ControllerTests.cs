using Microsoft.AspNetCore.Mvc;
using Moq;
using XtramileBackend.Controllers.DepartmentControllers;
using XtramileBackend.Controllers.ProductControllers;
using XtramileBackend.Controllers.TravelModeController;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Services.DepartmentService;
using XtramileBackend.Services.ProjectService;
using XtramileBackend.Services.TravelModeService;
using Xunit;

namespace XtramileBackend.Tests
{
    public class ControllerTests
    {
        [Fact]
        public async Task GetDepartmentsAsync_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var expectedDepartments = new List<TBL_DEPARTMENT>
            {
                new TBL_DEPARTMENT { DepartmentId = 1, DepartmentName = "Department A", DepartmentCode = "DeptA", Description = "Description A", CreatedBy = 1, CreatedOn = DateTime.Now, ModifiedBy = null, ModifiedOn = null },
                new TBL_DEPARTMENT { DepartmentId = 2, DepartmentName = "Department B", DepartmentCode = "DeptB", Description = "Description B", CreatedBy = 1, CreatedOn = DateTime.Now, ModifiedBy = null, ModifiedOn = null }
            };
            var departmentServiceMock = new Mock<IDepartmentServices>();
            departmentServiceMock.Setup(service => service.GetDepartmentAsync()).ReturnsAsync(expectedDepartments);

            var controller = new DepartmentController(departmentServiceMock.Object);

            // Act
            var result = await controller.GetDepartmentsAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualDepartments = Assert.IsAssignableFrom<IEnumerable<TBL_DEPARTMENT>>(okResult.Value);
            Assert.Equal(expectedDepartments, actualDepartments);
        }

        [Fact]
        public async Task GetDepartmentsAsync_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var errorMessage = "Internal server error";
            var departmentServiceMock = new Mock<IDepartmentServices>();
            departmentServiceMock.Setup(service => service.GetDepartmentAsync()).ThrowsAsync(new Exception(errorMessage));

            var controller = new DepartmentController(departmentServiceMock.Object);

            // Act
            var result = await controller.GetDepartmentsAsync();

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task GetProjectsAsync_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var expectedProjects = new List<TBL_PROJECT>
            {
                new TBL_PROJECT { ProjectId = 1, ProjectCode = "P001", ProjectName = "Project A", DepartmentId = 1, Description = "Description A", CreatedBy = 1, CreatedOn = DateTime.Now, Modifiedby = null, ModifiedOn = null },
                new TBL_PROJECT { ProjectId = 2, ProjectCode = "P002", ProjectName = "Project B", DepartmentId = 2, Description = "Description B", CreatedBy = 1, CreatedOn = DateTime.Now, Modifiedby = null, ModifiedOn = null }
            };
            var projectServiceMock = new Mock<IProjectServices>();
            projectServiceMock.Setup(service => service.GetAllProjectsAsync()).ReturnsAsync(expectedProjects);

            var controller = new ProjectController(projectServiceMock.Object);

            // Act
            var result = await controller.GetProjectsAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualProjects = Assert.IsAssignableFrom<IEnumerable<TBL_PROJECT>>(okResult.Value);
            Assert.Equal(expectedProjects, actualProjects);
        }

        [Fact]
        public async Task GetProjectsAsync_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var errorMessage = "Internal server error";
            var projectServiceMock = new Mock<IProjectServices>();
            projectServiceMock.Setup(service => service.GetAllProjectsAsync()).ThrowsAsync(new Exception(errorMessage));

            var controller = new ProjectController(projectServiceMock.Object);

            // Act
            var result = await controller.GetProjectsAsync();

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task GetTravelModesAsync_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var expectedModes = new List<TBL_TRAVEL_MODE>
            {
                new TBL_TRAVEL_MODE { ModeId = 1, ModeName = "Mode A", Description = "Description A", CreatedBy = 1, CreatedOn = DateTime.Now, ModifiedBy = null, ModifiedOn = null },
                new TBL_TRAVEL_MODE { ModeId = 2, ModeName = "Mode B", Description = "Description B", CreatedBy = 1, CreatedOn = DateTime.Now, ModifiedBy = null, ModifiedOn = null }
            };
            var travelModeServiceMock = new Mock<ITravelModeService>();
            travelModeServiceMock.Setup(service => service.GetTravelModeAsync()).ReturnsAsync(expectedModes);

            var controller = new TravelModeController(travelModeServiceMock.Object);

            // Act
            var result = await controller.GetTravelModesAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualModes = Assert.IsAssignableFrom<IEnumerable<TBL_TRAVEL_MODE>>(okResult.Value);
            Assert.Equal(expectedModes, actualModes);
        }

        [Fact]
        public async Task GetTravelModesAsync_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var errorMessage = "Internal server error";
            var travelModeServiceMock = new Mock<ITravelModeService>();
            travelModeServiceMock.Setup(service => service.GetTravelModeAsync()).ThrowsAsync(new Exception(errorMessage));

            var controller = new TravelModeController(travelModeServiceMock.Object);

            // Act
            var result = await controller.GetTravelModesAsync();

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task AddTravelModeAsync_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var newMode = new TBL_TRAVEL_MODE { ModeId = 3, ModeName = "Mode C", Description = "Description C", CreatedBy = 1, CreatedOn = DateTime.Now, ModifiedBy = null, ModifiedOn = null };
            var travelModeServiceMock = new Mock<ITravelModeService>();

            var controller = new TravelModeController(travelModeServiceMock.Object);

            // Act
            var result = await controller.AddTravelModeAsync(newMode);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var addedMode = Assert.IsAssignableFrom<TBL_TRAVEL_MODE>(okResult.Value);
            Assert.Equal(newMode, addedMode);
        }

        [Fact]
        public async Task AddTravelModeAsync_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var errorMessage = "Internal server error";
            var newMode = new TBL_TRAVEL_MODE { ModeId = 3, ModeName = "Mode C", Description = "Description C", CreatedBy = 1, CreatedOn = DateTime.Now, ModifiedBy = null, ModifiedOn = null };
            var travelModeServiceMock = new Mock<ITravelModeService>();
            travelModeServiceMock.Setup(service => service.SetTravelModeAsync(newMode)).ThrowsAsync(new Exception(errorMessage));

            var controller = new TravelModeController(travelModeServiceMock.Object);

            // Act
            var result = await controller.AddTravelModeAsync(newMode);

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

    }
}
