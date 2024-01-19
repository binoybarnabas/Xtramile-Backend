using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.RequestService
{
    public interface IRequestServices

    {
        public Task<IEnumerable<TBL_REQUEST>> GetAllRequestAsync();
        public Task AddRequestAsync(TBL_REQUEST request);

    }
}

