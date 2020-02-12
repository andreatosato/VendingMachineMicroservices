using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;
using VendingMachine.Service.Aggregators.Web.API.ViewModels.Machine;
using VendingMachine.Service.Machines.Application.ViewModels;
using VendingMachine.Service.Machines.Read.Models;
using VendingMachine.Service.Products.Application.ViewModels.Products;
using VendingMachine.Service.Products.Read.Models;

namespace VendingMachine.Service.Gateway.RefitModels
{
    public interface IGatewayApiBlazor
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



        [Get("/{productItemId}")]
        Task<ProductItemReadModel> GetInfosAsync(int productItemId);

        [Post("/product-api/ProductItems/")]
        Task<string> PostCreateProductItemAsync([Body] Products.Application.ViewModels.ProductItems.ProductItemViewModel model);




        [Get("/aggregator-api/AggregatorMachine/{machineId}")]
        Task<MachineItemViewModels> GetMachineCurrentStatusAsync(int machineId);



        [Get("/machine-api/MachineItems/NearbyMachineItems")]
        Task<IEnumerable<NearbyMachineReadModel>> GetNearbyMachineItemsAsync(GeoSearchViewModel model);

    }

    public class GeoSearchViewModel
    {
        [AliasAs("Latutide")]
        public double Latutide { get; set; }
        [AliasAs("Longitude")]
        public double Longitude { get; set; }
        [AliasAs("Radius")]
        public decimal? Radius { get; set; } = 50;
    }
}
