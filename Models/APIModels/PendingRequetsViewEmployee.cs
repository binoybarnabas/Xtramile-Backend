namespace XtramileBackend.Models.APIModels
{
/*    public class PendingRequetsViewEmployee
    {
        public string statusName { get; set; }
        public int requestId { get; set; }
        public string projectName { get; set; }
        public string reasonOfTravel { get; set; }
        public string destination {  get; set; }
        public DateTime dateOfTravel { get; set; }
        
    }*/

    public class PendingRequetsViewEmployee
    {

        public int requestId { get; set; } 

        public string? requestCode { get; set; }

        public string projectCode { get; set; }

        public string tripPurpose {  get; set; }

        public string sourceCity { get; set; }

        public string sourceCountry { get; set; }

        public string destinationCity { get; set; }
        public string destinationCountry { get; set; }

        public DateTime departureDate { get; set; }

        public DateTime? returnDate { get; set; }  

        public string statusName { get; set; }

        public string travelMode { get; set; }


    }



}
