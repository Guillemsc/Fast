using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Fast.Modules
{
    class EventModule : Module
    {
        private Fast.EventsController controller = new EventsController();

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
