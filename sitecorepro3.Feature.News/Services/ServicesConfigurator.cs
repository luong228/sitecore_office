using Sitecore.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using sitecorepro3.Feature.News.Controllers;
using sitecorepro3.Feature.News.Repositories;
using sitecorepro3.Foundation.DependencyInjection;

namespace sitecorepro3.Feature.News.Services
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.Add(typeof(INewsRepository),
                typeof(NewsRepository), Lifetime.Transient);

            serviceCollection.Add(typeof(NewsApiController),
                typeof(NewsApiController), Lifetime.Transient);

            //serviceCollection.Replace(ServiceDescriptor.Transient(typeof(NewsApiController),
            //    typeof(NewsApiController)));

            serviceCollection.AddScoped<INewsRepository, NewsRepository>();
        }
    }
}