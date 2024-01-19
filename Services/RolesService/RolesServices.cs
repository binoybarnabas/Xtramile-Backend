using XtramileBackend.Models.EntityModels;
using XtramileBackend.UnitOfWork;

namespace XtramileBackend.Services.RolesService
{
    public class RolesServices : IRolesServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public RolesServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<IEnumerable<TBL_ROLES>> GetAllRolesAsync()
        {
            try
            {
                var rolesData = await _unitOfWork.RoleRepository.GetAllAsync();
                return rolesData;
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting all roles: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }

        public async Task AddRoleAsync(TBL_ROLES role)
        {
            try
            {
                await _unitOfWork.RoleRepository.AddAsync(role);
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while adding a roles: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }
    }
}
