using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sitecorepro3.Foundation.DependencyInjection.Infrastructure
{
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.DependencyInjection;
    using System.Web.Services.Description;

    public class MvcControllerServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddMvcControllers("*.Feature.*");
            serviceCollection.AddClassesWithServiceAttribute("*.Feature.*");
            serviceCollection.AddClassesWithServiceAttribute("*.Foundation.*");
        }
    }
}