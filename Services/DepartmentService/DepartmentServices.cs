using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.UnitOfWork;

namespace XtramileBackend.Services.DepartmentService
{
    public class DepartmentServices : IDepartmentServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<IEnumerable<TBL_DEPARTMENT>> GetDepartmentAsync()
        {
            try
            {
                var departmentData = await _unitOfWork.DepartmentRepository.GetAllAsync();
                return departmentData;
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting departments: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }

        public async Task SetDepartmentAsync(TBL_DEPARTMENT department)
        {
            try
            {
                await _unitOfWork.DepartmentRepository.AddAsync(department);
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while setting department: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }
    }
}
