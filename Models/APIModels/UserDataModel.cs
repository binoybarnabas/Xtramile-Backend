namespace XtramileBackend.Models.APIModels
{
    public class UserDataModel
    {
        public int EmpId { get; set; }

        //used to store department code
        public string Department { get; set; }

        //used to store role code
        public string Role { get; set; }

        public string Token { get; set; }
    }

}
