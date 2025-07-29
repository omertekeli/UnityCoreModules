using System;
using System.Collections.Generic;

namespace UnityCoreModules.Services.EventBus
{
    public class EventBus : IEventBus
    {
        private readonly Dictionary<Type, Delegate> _subscribers;
        private readonly object _lock = new object(); // Thread-safe

        public EventBus()
        {
            _subscribers = new Dictionary<Type, Delegate>();
        }

        public void Fire<T>(T eventData) where T : IEvent
        {
            lock (_lock)
            {
                if (_subscribers.TryGetValue(typeof(T), out var handlers))
                {
                    (handlers as Action<T>)?.Invoke(eventData);
                }
            }
        }

        public void Subscribe<T>(Action<T> handler) where T : IEvent
        {
            lock (_lock)
            {
                var eventType = typeof(T);
                _subscribers.TryGetValue(eventType, out var handlers);
                _subscribers[eventType] = Delegate.Combine(handlers, handler);
            }
        }

        public void Unsubscribe<T>(Action<T> handler) where T : IEvent
        {
            lock (_lock)
            {
                var eventType = typeof(T);
                if (_subscribers.TryGetValue(eventType, out var handlers))
                {
                    var newHandlers = Delegate.Remove(handlers, handler);
                    if (newHandlers == null)
                    {
                        _subscribers.Remove(eventType);
                    }
                    else
                    {
                        _subscribers[eventType] = newHandlers;
                    }
                }
            }
        }
    }
}