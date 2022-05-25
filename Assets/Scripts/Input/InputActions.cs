using System;
using UnityEngine;

namespace OpenBlock.Input
{
    public struct InputActions
    {
        public Action<Vector2> move;
        public Action<Vector2> look;
        public Action digStart;
        public Action digEnd;
        public Action place;
        public Action jump;
        public Action descend;
        public Action menu;
        public Action<float> select;
        public Action any;
    }
}
