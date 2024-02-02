using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.PerdiemService
{
    public interface IPerdiemServices
    {
        Task<IEnumerable<TBL_PER_DIEM>> GetPerdiemAsync();
        Task AddPerdiemAsync(TBL_PER_DIEM perdiem);


    }
}
