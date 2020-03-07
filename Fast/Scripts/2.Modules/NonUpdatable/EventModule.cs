using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Fast.Modules
{
    public class EventModule : Module
    {
        private Fast.EventsController controller = new EventsController();

        public EventModule(FastService fast) : base(fast)
        {

        }

        public void Subscribe(int event_id, Action<Event> callback)
        {
            controller.Subscribe(event_id, callback);
        }

        public void UnSubscribe(int event_id, Action<Event> callback)
        {
            controller.UnSubscribe(event_id, callback);
        }

        public void SendEvent(Event ev)
        {
            controller.SendEvent(ev);
        }
    }
}
