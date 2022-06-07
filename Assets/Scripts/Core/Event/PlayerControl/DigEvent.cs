using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBlock.Core.Event.PlayerControl
{
    public struct DigEvent : IGameEvent
    {
        public enum Phase: byte
        {
            Start, End
        }
        public GameEventType type => GameEventType.PlayerControl;
        public Phase phase;

        public DigEvent(Phase phase)
        {
            this.phase = phase;
        }
    }
}
