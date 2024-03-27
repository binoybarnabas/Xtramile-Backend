﻿    using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XtramileBackend.Models.APIModels;
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

        //New Travel Option
        public async Task<int> AddNewTravelOptionAsync(TBL_TRAVEL_OPTION travelOption)
        {
            try
            {
                /*await _unitOfWork.TravelOptionRepository.AddAsync(travelOption);
                _unitOfWork.Complete();*/

                await _unitOfWork.TravelOptionRepository.AddAsync(travelOption);
                _unitOfWork.Complete(); // Assuming CompleteAsync returns Task
                return travelOption.OptionId;

            }
            catch(Exception ex) {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while adding an available option: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
            
        }

        //implement it
        public async Task UpdateFileIdOfOptionAsync(int fileId, int optionId)
        {
            try
            {
                TBL_TRAVEL_OPTION travelOption = await _unitOfWork.TravelOptionRepository.GetByIdAsync(optionId);
                if (travelOption != null)
                {
                    // Update the fileId
                    travelOption.FileId = fileId;

                    // Save the changes
                    _unitOfWork.TravelOptionRepository.Update(travelOption);
                    _unitOfWork.Complete();
                }
                else
                {
                    Console.WriteLine($"Option with ID {optionId} not found.");
                }
/*                _unitOfWork.Complete();
*/            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while updating file id: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }

        public async Task<IEnumerable<TBL_TRAVEL_OPTION>> GetTravelOptionsByRequestIdAsync(int reqId, bool travelOptiontext)
        {
            try
            {
                var allOptions = await _unitOfWork.TravelOptionRepository.GetAllAsync();
                

                var travelOptions = (travelOptiontext == true) ?
                                     (from item in allOptions
                                      where item.RequestId == reqId && item.FileId == null
                                      select item).ToList():
                                     (from item in allOptions
                                     where item.RequestId == reqId && item.FileId != null
                                      select item).ToList();

                return travelOptions;
            }
            catch (Exception ex )
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting options : {ex.Message}");
                throw; // Re-throw the exception to propagate it

            }
        }

        /// <summary>
        /// To add available travel option ticket details in the form of text
        /// </summary>
        /// <param name="availableOption"></param>
        /// <returns></returns>
        public async Task<string> AddAvailableTextOptionAsync(AvailableOption availableOption)
        {
            try{

                TBL_TRAVEL_OPTION availableTravelOption = new TBL_TRAVEL_OPTION();
                availableTravelOption.RequestId = availableOption.RequestId;
                availableTravelOption.Description = availableOption.HtmlContent;
                availableTravelOption.FileId = null;

                await _unitOfWork.TravelOptionRepository.AddAsync(availableTravelOption);
                _unitOfWork.Complete();
                return "successfully inserted";

            }
            catch (Exception ex) { 
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting adding text available options : {ex.Message}");
                throw; // Re-throw the exception to propagate ;
            }

        }

        public async Task DeleteTravelOptions(int[] FileIds)
        {
            try
            {
                for (int i = 0; i < FileIds.Length; i++)
                {
                    TBL_TRAVEL_OPTION travelOptionToBeDeleted = await _unitOfWork.TravelOptionRepository.GetByIdAsync(FileIds[i]);
                    if (travelOptionToBeDeleted != null)
                    {
                        _unitOfWork.TravelOptionRepository.Delete(travelOptionToBeDeleted);
                        await _unitOfWork.SaveChangesAsyn();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while deleting travel options : {ex.Message}");
                throw; // Re-throw the exception to propagate ;
            }

        }

    }
}
