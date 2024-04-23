using XtramileBackend.Models.APIModels;
using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.RequestService
{
    public interface IRequestServices

    {
        public Task<IEnumerable<Request>> GetAllRequestAsync();
        public Task AddRequestAsync(Request request);

        public string GenerateRandomCode(int suffix);


        public Task<int> GetRequestIdByRequestCode(string requestCode);


        public Task<Request> GetRequestById (int id);

        public Task<string> GetReasonDescriptionByRequestId(int requestId);

    }
}

