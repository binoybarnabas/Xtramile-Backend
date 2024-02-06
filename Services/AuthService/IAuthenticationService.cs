using XtramileBackend.Models.APIModels;

namespace XtramileBackend.Services.AuthService
{
    public interface IAuthenticationService
    {
        public UserDataModel checkCredentials(Credentials credential);

    }
}
