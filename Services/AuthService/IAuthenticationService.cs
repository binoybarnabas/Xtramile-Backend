using XtramileBackend.Models.APIModels;

namespace XtramileBackend.Services.AuthService
{
    public interface IAuthenticationService
    {
        public string checkCredentials(Credentials credential);

    }
}
