using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.UnitOfWork;

namespace XtramileBackend.Services.PriorityService
{
    public class PriorityServices : IPriorityServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public PriorityServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<IEnumerable<Priority>> GetPrioritiesAsync()
        {
            try
            {
                var priorityData = await _unitOfWork.PriorityRepository.GetAllAsync();
                return priorityData;
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting priorities: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }

        public async Task AddPriorityAsync(Priority priority)
        {
            try
            {
                await _unitOfWork.PriorityRepository.AddAsync(priority);
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while adding a priority: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }
    }
}
