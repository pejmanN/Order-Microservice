﻿using System.Collections.Generic;
using System.Linq;

namespace Framework.Core.Events
{
    public class EventAggregator : IEventAggregator
    {
        private readonly List<object> _subscribers;
        public EventAggregator()
        {
            _subscribers = new List<object>();
        }
        
        //private 
        public void Publish<T>(T @event) where T : IEvent
        {
            var targetHandlers = _subscribers.OfType<IEventHandler<T>>().ToList();
            foreach (var handler in targetHandlers)
            {
                handler.Handle(@event);
            }
        }

        public void Subscribe<T>(IEventHandler<T> handler) where T : IEvent
        {
            _subscribers.Add(handler);
        }

        public void Unsubcribe<T>(IEventHandler<T> handler) where T : IEvent
        {
            _subscribers.Remove(handler);
        }
    }
}