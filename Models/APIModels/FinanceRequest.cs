﻿namespace XtramileBackend.Models.APIModels
{
    public class FinanceRequest
    {

        public int requestId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime RequestDate { get; set; }
        public string TypeName { get; set; }
    }
}
