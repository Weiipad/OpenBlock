﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBlock.Core.Event
{
    public interface IGameEvent
    {
        GameEventType type { get; }
    }
}
