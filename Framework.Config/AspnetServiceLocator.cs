using Framework.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Config
{
    public class AspnetServiceLocator : IServiceLocator
    {
        private readonly IServiceProvider _serviceProvider;
        public AspnetServiceLocator(IServiceProvider services)
        {
            _serviceProvider = services;
        }

        public T Resolve<T>() where T : class
        {
            //var t1 = _serviceProvider.GetService(typeof(ICommandHandler<CreateAuctionCommand>));
            var service = _serviceProvider.GetRequiredService<T>();
            return service;
        }
    }
}
