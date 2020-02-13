# WebApp - Blazor Web Assembly
La WebApi può essere definita in vari linguaggi.
Sicuramente Blazor è la novità e potrebbe aiutarci avendo minor necessità di conoscere il linguaggio JS.

La chiamata verso le API avviene tramite l'API Gateway, richiamata via Refit.

# Refit
Definizione
```cs
 public interface IGatewayApi : 
        IProductApiV2, IProductItemApi,
        IAggregationMachine,
        IMachineApi
{
    // For Refit
    [Get("/get?result=Foo")]
    Task<string> Foo();
}


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

[Headers("Authorization: Bearer")]
public interface IMachineApi
{
    [Get("/machine-api/MachineItems/NearbyMachineItems")]
    Task<IEnumerable<NearbyMachineReadModel>> GetNearbyMachineItemsAsync([Body] GeoSearchViewModel model);
}

```


Utilizzo
```cs
await api.GetNearbyMachineItemsAsync(new GeoSearchViewModel
{
    Latutide = (double)CurrentPosition.Latitude,
    Longitude = (double)CurrentPosition.Longitude,
    Radius = 10_000
});
```