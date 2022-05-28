using OpenBlock.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenBlock
{
    public enum GlobalMessageType
    {
        World
    }

    public struct GlobalMessage
    {
        public GlobalMessageType type;
        public string message;
    }

    public class MessageStorage: CommonSingleton<MessageStorage>
    {

        private Queue<GlobalMessage> msgQueue;
        private Dictionary<GlobalMessageType, System.Action<GlobalMessage>> msgHandlers;
        public MessageStorage()
        {
            msgQueue = new Queue<GlobalMessage>();
            msgHandlers = new Dictionary<GlobalMessageType, System.Action<GlobalMessage>>();
        }

        public void RegisterHandler(GlobalMessageType ty, System.Action<GlobalMessage> handler)
        {
            msgHandlers.Add(ty, handler);
        }

        public bool PollMessage()
        {
            if (msgQueue.Count == 0) return false;

            var msg = msgQueue.Dequeue();
            if (msgHandlers.TryGetValue(msg.type, out var handler))
            {
                handler(msg);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}