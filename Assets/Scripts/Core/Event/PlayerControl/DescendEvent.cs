using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBlock.Core.Event.PlayerControl
{
    public struct DescendEvent : IGameEvent
    {
        public GameEventType type => GameEventType.PlayerControl;
    }
}
