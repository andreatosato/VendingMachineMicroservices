using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System.Collections.Generic;
using VendingMachine.Service.Machines.Application.ViewModels;

namespace VendingMachine.Service.Machines.Binders
{
    public class MachineModelBinderProvider : IModelBinderProvider
    {
        private readonly IList<IInputFormatter> formatters;
        private readonly IHttpRequestStreamReaderFactory readerFactory;
        private BodyModelBinderProvider defaultProvider;

        public MachineModelBinderProvider(IList<IInputFormatter> formatters, IHttpRequestStreamReaderFactory readerFactory)
        {
            this.formatters = formatters;
            this.readerFactory = readerFactory;
            defaultProvider = new BodyModelBinderProvider(formatters, readerFactory);
        }

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
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
