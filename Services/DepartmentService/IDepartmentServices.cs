using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.DepartmentService
{
    public interface IDepartmentServices
    {
        public Task<IEnumerable<Department>> GetDepartmentAsync();
        public Task SetDepartmentAsync(Department department);
    }
}
