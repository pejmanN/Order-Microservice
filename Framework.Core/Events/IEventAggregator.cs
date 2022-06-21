using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Core.Events
{
    public interface IEventAggregator
    {
        void Publish<T>(T @event) where T : IEvent;
        void Subscribe<T>(IEventHandler<T> handler) where T : IEvent;
        void Unsubcribe<T>(IEventHandler<T> handler) where T : IEvent;
    }
}
