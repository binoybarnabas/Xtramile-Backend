﻿namespace XtramileBackend.Models.APIModels
{
    public class ClosedTravelAdmin
    {
        public int requestId { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string Name { get; set; }
        public string SourceCity { get; set; }
        public string DestinationCity { get; set; }
        public string Date {  get; set; }
    }
}
