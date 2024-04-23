using XtramileBackend.Models.EntityModels;

namespace XtramileBackend.Services.PerdiemService
{
    public interface IPerdiemServices
    {
        Task<IEnumerable<PerDiem>> GetPerdiemAsync();
        Task AddPerdiemAsync(PerDiem perdiem);


    }
}
