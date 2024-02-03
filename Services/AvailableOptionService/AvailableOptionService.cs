    using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.UnitOfWork;

namespace XtramileBackend.Services.AvailableOptionService
{
    public class AvailableOptionServices : IAvailableOptionServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public AvailableOptionServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<IEnumerable<TBL_AVAIL_OPTION>> GetAvailableOptionsAsync()
        {
            try
            {
                var availableOptionData = await _unitOfWork.AvailableOptionRepository.GetAllAsync();
                return availableOptionData;
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting available options: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }

        public async Task AddAvailableOptionAsync(TBL_AVAIL_OPTION availableOption)
        {
            try
            {
                await _unitOfWork.AvailableOptionRepository.AddAsync(availableOption);
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while adding an available option: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }
    }
}
