using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.DepartmentService
{
    public interface IDepartmentServices
    {
        public Task<IEnumerable<TBL_DEPARTMENT>> GetDepartmentAsync();
        public Task SetDepartmentAsync(TBL_DEPARTMENT department);
    }
}
