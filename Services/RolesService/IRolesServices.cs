using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.RolesService
{
    public interface IRolesServices
    {
        public Task<IEnumerable<Roles>> GetAllRolesAsync();
        public Task AddRoleAsync(Roles roles);
    }
}
