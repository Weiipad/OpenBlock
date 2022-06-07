using OpenBlock.Core.Event;
using UnityEngine;

namespace OpenBlock.Core.Event.PlayerControl
{
    public struct MoveEvent : IGameEvent
    {
        public GameEventType type => GameEventType.PlayerControl;
        public Vector2 direction;
        public MoveEvent(Vector2 direction)
        {
            this.direction = direction;
        }
    }
}
