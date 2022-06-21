using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Core.Events;

namespace Framework.Domain
{
    public class AggregateRootBase<T> : EntityBase<T>, IAggregateRoot
    {
        private IEventAggregator Publisher { get; set; }
        private readonly List<DomainEvent> UncommittedEvents;

        protected AggregateRootBase()
        {
            this.UncommittedEvents = new List<DomainEvent>();   
        }
        protected AggregateRootBase(T id) : base(id)
        {
        }
        public AggregateRootBase(IEventAggregator publisher)
        {
            this.Publisher = publisher;
            this.UncommittedEvents = new List<DomainEvent>();
        }

        public IReadOnlyList<DomainEvent> GetUncommittedEvents() => UncommittedEvents.AsReadOnly();

        public void ClearUncommittedEvents()
        {
            this.UncommittedEvents.Clear();
        }
        public void Publish<TEvent>(TEvent @event) where TEvent : DomainEvent
        {
            this.UncommittedEvents.Add(@event);
            this.Publisher.Publish(@event);
        }
        public void SetPublisher(IEventAggregator publisher)
        {
            Debug.Assert(this.Publisher == null);
            this.Publisher = publisher;
        }
        public void ClearEvents()
        {
            this.UncommittedEvents.Clear();
        }
    }
}
