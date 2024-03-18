namespace XtramileBackend.Models.APIModels
{
    public class CompletedTripsCard
    {
        public string From { get; set; }
        public string To { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public DateTime CompletedDate { get; set; }
        public int Count { get; set; }
    }
}
