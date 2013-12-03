using System.Threading.Tasks;

namespace DocaLabs.Http.Client.Remote.Integration.Tests._Contract._Authentication
{
    public interface IAuthenticatedUser
    {
        AuthenticatedUser Get();
    }
    public interface IAuthenticatedUserAsync
    {
        Task<AuthenticatedUser> Get();
    }
}