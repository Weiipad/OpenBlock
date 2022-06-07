using OpenBlock.Core.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace OpenBlock.Core.Event.PlayerControl
{
    public struct LookEvent : IGameEvent
    {
        public GameEventType type => GameEventType.PlayerControl;
        public Vector2 delta;

        public LookEvent(Vector2 delta)
        {
            this.delta = delta;
        }
    }
}
