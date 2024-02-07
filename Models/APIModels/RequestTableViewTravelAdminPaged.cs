
namespace XtramileBackend.Models.APIModels
{
    public class RequestTableViewTravelAdminPaged
    {
        public IReadOnlyCollection<RequestTableViewTravelAdmin> TravelRequest { get; set; }

        public int TotalPages { get; set; }

        public int PageCount { get; set; }

        public static explicit operator RequestTableViewTravelAdminPaged(Task<RequestTableViewTravelAdminPaged> v)
        {
            throw new NotImplementedException();
        }
    }

}
