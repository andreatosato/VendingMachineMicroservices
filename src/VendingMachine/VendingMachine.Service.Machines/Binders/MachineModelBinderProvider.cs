using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using VendingMachine.Service.Machines.Application.ViewModels;

namespace VendingMachine.Service.Machines.Binders
{
    public class MachineModelBinderProvider : IModelBinderProvider
    {
        private readonly IList<IInputFormatter> formatters;
        private BodyModelBinderProvider defaultProvider;

        public MachineModelBinderProvider(IList<IInputFormatter> formatters)
        {
            this.formatters = formatters;
        }

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            var readerFactory = context.Services.GetRequiredService<IHttpRequestStreamReaderFactory>();
            defaultProvider = new BodyModelBinderProvider(formatters, readerFactory);
            IModelBinder modelBinder = defaultProvider.GetBinder(context);

            // default provider returns null when there is error.So for not null setting our binder
            if (context.Metadata.ModelType == typeof(BuyProductsViewModel))
            {
                modelBinder = new BuyProductsBinder(formatters, readerFactory);
            }

            if (context.Metadata.ModelType == typeof(LoadProductsViewModel))
            {
                modelBinder = new LoadProductsBinder(formatters, readerFactory);
            }

            return modelBinder;
        }
    }
}
