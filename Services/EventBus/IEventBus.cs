using System;

namespace UnityCoreModules.Services.EventBus
{
    /// <summary>
    /// Listen only
    /// </summary>
    public interface IEventSubscriber
    {
        void Subscribe<T>(Action<T> handler) where T : IEvent;
        void Unsubscribe<T>(Action<T> handler) where T : IEvent;
    }

    /// <summary>
    /// Fire only
    /// </summary>
    public interface IEventPublisher
    {
        void Fire<T>(T eventData) where T : IEvent;
    }

    public interface IEventBus : IEventSubscriber, IEventPublisher
    {
        
    }
}