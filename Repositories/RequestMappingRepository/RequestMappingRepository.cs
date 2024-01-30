using XtramileBackend.Data;
using XtramileBackend.Models.APIModels;

namespace XtramileBackend.Repositories.RequestMappingRepository
{
    public class RequestMappingRepository: Repository<TBL_REQ_MAPPING>, IRequestMappingRepsitory
    {
        public RequestMappingRepository(AppDBContext context) : base(context) { }
    }
}
