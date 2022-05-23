using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBlock.Input
{
    interface IInputHandler
    {
        void HandleInputs(ref InputActions actions);
    }
}
