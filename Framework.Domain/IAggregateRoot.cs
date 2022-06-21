using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Core.Events;

namespace Framework.Domain
{
    public interface IAggregateRoot
    {
        IReadOnlyList<DomainEvent> GetUncommittedEvents();
        void ClearUncommittedEvents();
    }
}
