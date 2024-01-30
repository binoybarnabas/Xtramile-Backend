using XtramileBackend.Models.APIModels;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.UnitOfWork;

namespace XtramileBackend.Services.RequestMappingService
{
    public class RequestMappingService : IRequestMappingService
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public RequestMappingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Adds a selected option for a specific request.
        /// </summary>
        /// <param name="option"></param>
        /// <returns>The TBL_REQ_MAPPING object representing the selected option to be added.</returns>
        public async Task AddSelectedOptionForRequest(TBL_REQ_MAPPING option)
        {
            try
            {
                await _unitOfWork.RequestMappingRepository.AddAsync(option);
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding options : {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// Retrieves the selected option details for a specific request ID.
        /// <param name="reqId"></param>
        /// <returns>An instance of OptionCard representing the selected option details, or null if not found.</returns>
        public async Task<OptionCard> GetSelectedOptions(int reqId)
        {
            try
            {
                // Fetch data from repositories
                IEnumerable<TBL_REQ_MAPPING> reqMappingData = await _unitOfWork.RequestMappingRepository.GetAllAsync();
                IEnumerable<TBL_AVAIL_OPTION> availableOptions = await _unitOfWork.AvailableOptionRepository.GetAllAsync();
                IEnumerable<TBL_REQUEST> requestData = await _unitOfWork.RequestRepository.GetAllAsync();
                IEnumerable<TBL_TRAVEL_MODE> travelModeData = await _unitOfWork.TravelModeRepository.GetAllAsync();
                IEnumerable<TBL_TRAVEL_TYPE> travelTypeData = await _unitOfWork.TravelTypeRepository.GetAllAsync();
                IEnumerable<TBL_COUNTRY> countryData = await _unitOfWork.CountryRepository.GetAllAsync();

                // Perform the join and projection
                var result = (from reqMapping in reqMappingData
                              join option in availableOptions on reqMapping.OptionId equals option.OptionId
                              join request in requestData on option.RequestId equals request.RequestId
                              join mode in travelModeData on request.ModeId equals mode.ModeId
                              join sourceCountry in countryData on request.SourceCountry equals sourceCountry.CountryName
                              join destinationCountry in countryData on request.DestinationCountry equals destinationCountry.CountryName
                              join travelType in travelTypeData on request.TravelTypeId equals travelType.TravelTypeID
                              where reqMapping.RequestId == reqId
                              select new OptionCard
                              {
                                  OptionId = option.OptionId,
                                  StartTime = option.StartTime,
                                  EndTime = option.EndTime,
                                  ServiceOfferedBy = option.ServiceOfferedBy,
                                  Class = option.Class,
                                  RequestId = option.RequestId,
                                  SourceCity = request.SourceCity,
                                  SourceState = request.SourceState,
                                  SourceCountry = request.SourceCountry,
                                  SourceCountryCode = sourceCountry.CountryCode,
                                  DestinationCity = request.DestinationCity,
                                  DestinationState = request.DestinationState,
                                  DestinationCountry = request.DestinationCountry,
                                  DestinationCountryCode = destinationCountry.CountryCode,
                                  ModeId = request.ModeId,
                                  ModeName = mode.ModeName,
                                  TravelTypeId = request.TravelTypeId,
                                  TravelTypeName = travelType.TypeName,
                              })
                              .FirstOrDefault(); // Return the first matching result or null

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while getting option : {ex.Message}");
                throw;
            }
        }

    }
}
