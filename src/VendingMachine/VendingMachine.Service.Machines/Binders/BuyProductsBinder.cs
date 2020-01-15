using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Threading.Tasks;
using VendingMachine.Service.Machines.Application.ViewModels;

namespace VendingMachine.Service.Machines.Binders
{
    public class BuyProductsBinder : IModelBinder
    {
        private readonly BodyModelBinder defaultBinder;

        public BuyProductsBinder(IList<IInputFormatter> formatters, IHttpRequestStreamReaderFactory readerFactory) // : base(formatters, readerFactory)
        {
            defaultBinder = new BodyModelBinder(formatters, readerFactory);
        }

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new System.ArgumentNullException(nameof(bindingContext));
            }
            await defaultBinder.BindModelAsync(bindingContext);

            int machineId;
            if (bindingContext.HttpContext.GetRouteData().Values.TryGetValue("machineId", out var machineRoute))
            {
                machineId = int.Parse((string)machineRoute);
                if (bindingContext.Result.IsModelSet)
                {
                    var model = bindingContext.Result.Model as BuyProductsViewModel;
                    if (model != null)
                    {
                        model.SetMachineId(machineId);
                        bindingContext.Result = ModelBindingResult.Success(model);
                    }
                }
            }
            else
            {
                bindingContext.ModelState.AddModelError("MachineId", "MachineId is required");
            }
        }
    }

    public class LoadProductsBinder : IModelBinder
    {
        private readonly BodyModelBinder defaultBinder;

        public LoadProductsBinder(IList<IInputFormatter> formatters, IHttpRequestStreamReaderFactory readerFactory) // : base(formatters, readerFactory)
        {
            defaultBinder = new BodyModelBinder(formatters, readerFactory);
        }

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new System.ArgumentNullException(nameof(bindingContext));
            }
            await defaultBinder.BindModelAsync(bindingContext);

            int machineId;
            if (bindingContext.HttpContext.GetRouteData().Values.TryGetValue("machineId", out var machineRoute))
            {
                machineId = int.Parse((string)machineRoute);
                if (bindingContext.Result.IsModelSet)
                {
                    var model = bindingContext.Result.Model as LoadProductsViewModel;
                    if (model != null)
                    {
                        model.SetMachineId(machineId);
                        bindingContext.Result = ModelBindingResult.Success(model);
                    }
                }
            }
            else
            {
                bindingContext.ModelState.AddModelError("MachineId", "MachineId is required");
            }
        }
    }
}
