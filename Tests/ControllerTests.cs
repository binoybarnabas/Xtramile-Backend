using Microsoft.AspNetCore.Mvc;
using Moq;
using XtramileBackend.Controllers.DepartmentControllers;
using XtramileBackend.Controllers.ProductControllers;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Services.DepartmentService;
using XtramileBackend.Services.ProjectService;
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

    }
}
