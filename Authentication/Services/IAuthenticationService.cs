namespace Authentication.Services
{
    public interface IAuthenticationService
    {
        void StartListening();
         bool ValidateToken(string token);
    }
}