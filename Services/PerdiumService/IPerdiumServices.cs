using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.PerdiumService
{
    public interface IPerdiumServices
    {
        Task<IEnumerable<TBL_PER_DIUM>> GetPerdiumAsync();
        Task AddPerdiumAsync(TBL_PER_DIUM perdium);


    }
}
