namespace XtramileBackend.Models.APIModels
{
    public class ClosedTravelAdminPaged
    {
        public IReadOnlyCollection<ClosedTravelAdmin>? ClosedTravel { get; set; }
        public int TotalCount { get; set; }
    }
}
