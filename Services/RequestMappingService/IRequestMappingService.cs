using XtramileBackend.Models.APIModels;

namespace XtramileBackend.Services.RequestMappingService
{
    public interface IRequestMappingService
    {
        public Task<OptionCard> GetSelectedOptions(int reqId);
        public Task AddSelectedOptionForRequest(TBL_REQ_MAPPING option);
    }
}
