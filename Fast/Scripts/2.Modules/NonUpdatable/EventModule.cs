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

        public void Subscribe<T>(Action<IEvent> callback) where T : IEvent
        {
            controller.Subscribe<T>(callback);
        }

        public void UnSubscribe<T>(Action<IEvent> callback) where T : IEvent
        {
            controller.UnSubscribe<T>(callback);
        }

        public void SendEvent(IEvent ev)
        {
            controller.SendEvent(ev);
        }
    }
}
