using Refit;
using System.Threading.Tasks;

namespace VendingMachine.Service.Gateway.RefitModels
{
    /// <summary>
    /// https://github.com/reactiveui/refit
    /// </summary>
    public interface IGatewayApi : 
        IProductApiV2, IProductItemApi
    {
        [Get("/get?result=Foo")]
        Task<string> Foo();
    }
}
