using Refit;
using System.Threading.Tasks;
using VendingMachine.Service.Products.Application.ViewModels.ProductItems;
using VendingMachine.Service.Products.Application.ViewModels.Products;
using VendingMachine.Service.Products.Read.Models;

namespace VendingMachine.Service.Gateway.RefitModels
{
    [Headers("x-api-version: 2.0", "Authorization: Bearer")]
    public interface IProductApiV2
    {
        [Get("/product-api/Products/{productId}")]
        Task<ColdDrinkViewModel> GetColdDrinkAsync(int productId);

        [Get("/product-api/Products/{productId}")]
        Task<HotDrinkViewModel> GetHotDrinkAsync(int productId);

        [Get("/product-api/Products/{productId}")]
        Task<SnackViewModel> GetSnakAsync(int productId);

        [Post("/product-api/Products/Snack")]
        Task<string> PostCreateSnackAsync([Body] Products.Application.ViewModels.Products.SnackViewModel model);

        [Post("/product-api/Products/ColdDrink")]
        Task<string> PostCreateColdDrinkAsync([Body] Products.Application.ViewModels.Products.ColdDrinkViewModel model);

        [Post("/product-api/Products/HotDrink")]
        Task<string> PostCreateHotDrinkAsync([Body] Products.Application.ViewModels.Products.HotDrinkViewModel model);

        [Post("/product-api/Products/{productId}")]
        Task DeleteProductAsync(int productId);
    }

    [Headers("Authorization: Bearer")]
    public interface IProductItemApi
    {
        [Get("/{productItemId}")]
        Task<ProductItemReadModel> GetInfosAsync(int productItemId);

        [Post("/product-api/ProductItems/")]
        Task<string> PostCreateProductItemAsync([Body] Products.Application.ViewModels.ProductItems.ProductItemViewModel model);
    }
}
