using Refit;
using System.Threading.Tasks;

namespace VendingMachine.Service.Gateway.RefitModels
{
    /// <summary>
    /// https://github.com/reactiveui/refit
    /// </summary>
    public interface IGatewayApi : IAuthenticationApi, 
        IProductApiV2, IProductItemApi
    {

    }
}
