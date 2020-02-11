using System.Net.Http;
using System.Threading.Tasks;

namespace VendingMachine.Service.Gateway.RefitModels
{
    public interface IAuthClient
    {
        HttpClient GetClient(string baseUrl);
    }

    public interface IAuthUserClient
    {
        Task<string> GetToken(string username, string password);
        HttpClient GetClient(string baseUrl, string token);
    }
}
