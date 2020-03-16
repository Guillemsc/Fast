using System;
using System.Collections.Generic;

namespace Fast
{
    public class EventsController : Fast.IController
    {
        private Dictionary<Type, Callback<IEvent>> events_list = new Dictionary<Type, Callback<IEvent>>();

        public void Subscribe<T>(Action<IEvent> callback) where T : IEvent
        {
            Type type = typeof(T);

            Callback<IEvent> event_callback = null;

            bool exists = events_list.TryGetValue(type, out event_callback);

            if(!exists)
            {
                event_callback = new Callback<IEvent>();

                events_list[type] = event_callback;
            }

            event_callback.Subscribe(callback);
        }

        public void UnSubscribe<T>(Action<IEvent> callback) where T : IEvent
        {
            Type type = typeof(T);

            Callback<IEvent> event_callback = null;

            bool exists = events_list.TryGetValue(type, out event_callback);

            if(exists)
            {
                event_callback.UnSubscribe(callback);
            }
        }

        public void SendEvent(IEvent ev) 
        {
            if (ev != null)
            {
                Type type = ev.GetType();

                Callback<IEvent> event_callback = null;

                bool exists = events_list.TryGetValue(type, out event_callback);

                if (exists)
                {
                    event_callback.Invoke(ev);
                }
            }
        }
    }
}
