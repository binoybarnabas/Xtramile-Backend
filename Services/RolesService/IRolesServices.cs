using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.RolesService
{
    public interface IRolesServices
    {
        public Task<IEnumerable<TBL_ROLES>> GetAllRolesAsync();
        public Task AddRoleAsync(TBL_ROLES roles);
    }
}
