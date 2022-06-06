using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

namespace OpenBlock.Core
{
    public interface GameEvent
    {
        int GetType();


    }
    public class EventQueue
    {
        private ConcurrentQueue<GameEvent> queue;
    }
}
