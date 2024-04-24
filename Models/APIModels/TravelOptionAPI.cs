namespace XtramileBackend.Models.APIModels
{
    public class TravelOptionAPI
    {
        public DateTime Date { get; set; } 
        public string[] Description { get; set; }

        public int EmpId { get; set; }

        // Assuming File is a custom type or model representing files, adjust accordingly
        public IFormFile[] Images { get; set; }

        public int PrimaryStatusId { get; set; }

        public int RequestId { get; set; }

        public int SecondaryStatusId { get; set; }

        public string[] Texts { get; set; }
    }

}
