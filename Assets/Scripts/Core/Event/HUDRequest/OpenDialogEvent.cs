using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBlock.Core.Event.HUDRequest
{
    public struct OpenDialogEvent : IGameEvent
    {
        public GameEventType type => GameEventType.HUDRequest;
        public string msg;

        public OpenDialogEvent(string message)
        {
            msg = message;
        }
    }
}
