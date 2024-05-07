namespace XtramileBackend.Models.APIModels
{
    public class RelevantDocument
    {
        public TravelDocumentViewModel? TravelAuthDocument { get; set; }
        public TravelDocumentViewModel? PassportDocument { get; set; }
        public TravelDocumentViewModel? VisaDocument { get; set; }
        public IEnumerable<TravelDocumentViewModel>? IdCardDocuments { get; set; }
    }
}
