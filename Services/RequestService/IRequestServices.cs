using XtramileBackend.Models.APIModels;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.RequestService
{
    public interface IRequestServices

    {
        public Task<IEnumerable<TBL_REQUEST>> GetAllRequestAsync();
        public Task AddRequestAsync(TBL_REQUEST request);

        public string GenerateRandomCode(int suffix);


        public Task<int> GetRequestIdByRequestCode(string requestCode);


        public Task<TBL_REQUEST> GetRequestById (int id);

        public Task<string> GetReasonDescriptionByRequestId(int requestId);

    }
}

