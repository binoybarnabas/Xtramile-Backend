namespace XtramileBackend.Models.APIModels
{
    public class UpdatePriorityAndStatusModel
    {
       public int RequestId { get; set; } 
        
       public int? PriorityId { get; set; }
       public int ManagerId { get; set; }
    }
}
