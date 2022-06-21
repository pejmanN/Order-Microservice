using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Application
{
    public interface ICommandHandler { }
    public interface ICommandHandler<T> : ICommandHandler where T : class
    {
        Task Handle(T command);
    }
}
