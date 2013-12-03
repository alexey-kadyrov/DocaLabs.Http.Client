using System.Threading.Tasks;

namespace DocaLabs.Http.Client.Remote.Integration.Tests._Contract
{
    public interface IGoogleSearch
    {
        string GetPage();
    }

    public interface IGoogleSearchAsync
    {
        Task<string> GetPage();
    }
}