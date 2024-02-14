namespace XtramileBackend.Models.APIModels
{
    public class TravelOptionViewModel
    {
        public int OptionId { get; set; }
        public string?  RequestId { get; set; }

        public string? Description { get; set; }

        public IFormFile? OptionFile { get; set; }


        public string? OptionFileURL { get; set; }


    }
}
