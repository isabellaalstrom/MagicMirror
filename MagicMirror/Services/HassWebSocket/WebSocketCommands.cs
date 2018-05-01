﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagicMirror.Services.HassWebSocket
{
    public class SubscribeToEventsCommand
    {
        public int id { get; }
        public string type { get; } = "subscribe_events";
        public string event_type { get; }

        public SubscribeToEventsCommand(EventType eventType, int id)
        {
            this.event_type = Enum.GetName(typeof(EventType), eventType);
            this.id = id;
        }
    }

    public enum EventType
    {
        state_changed,
        click
    }
}
