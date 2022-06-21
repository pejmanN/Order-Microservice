using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core
{
    public interface IServiceLocator
    {
        T Resolve<T>() where T : class;
    }

    public static class ServiceLocator
    {
        public static IServiceLocator Current { get; private set; }
        public static void Set(IServiceLocator locator)
        {
            Current = locator;
        }
    }
}
