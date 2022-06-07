using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

namespace OpenBlock.Core.Event
{
    public class EventQueue
    {
        private ConcurrentQueue<IGameEvent> queue;

        private Dictionary<GameEventType, System.Action<IGameEvent>> eventHandlers;

        public EventQueue()
        {
            queue = new ConcurrentQueue<IGameEvent>();
            eventHandlers = new Dictionary<GameEventType, System.Action<IGameEvent>>();
        }

        public void SendEvent(IGameEvent gameEvent)
        {
            queue.Enqueue(gameEvent);
        }

        public void RegisterHandler(GameEventType type, System.Action<IGameEvent> handler)
        {
            if (eventHandlers.ContainsKey(type))
            {
                eventHandlers[type] += handler;
            }
            else
            {
                eventHandlers.Add(type, handler);
            }
        }

        public void RemoveHandler(GameEventType type, System.Action<IGameEvent> handler)
        {
            if (!eventHandlers.ContainsKey(type)) return;
            eventHandlers[type] -= handler;
        }

        public void HandleEvents()
        {
            while (queue.TryDequeue(out IGameEvent e))
            {
                if (eventHandlers.TryGetValue(e.type, out var handler))
                {
                    handler?.Invoke(e);
                }
            }
        }
    }
}
