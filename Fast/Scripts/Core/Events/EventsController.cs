using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast
{
    class EventsController
    {
        private Dictionary<int, Callback<Event>> events_list = new Dictionary<int, Callback<Event>>();

        public void Subscribe(int event_id, Action<Event> callback)
        {
            Callback<Event> event_callback = null;

            bool exists = events_list.TryGetValue(event_id, out event_callback);

            if(!exists)
            {
                event_callback = new Callback<Event>();

                events_list[event_id] = event_callback;
            }

            event_callback.Subscribe(callback);
        }

        public void UnSubscribe(int event_id, Action<Event> callback)
        {
            Callback<Event> event_callback = null;

            bool exists = events_list.TryGetValue(event_id, out event_callback);

            if(exists)
            {
                event_callback.UnSubscribe(callback);
            }
        }

        public void Invoke(Event ev)
        {
            if (ev != null)
            {
                Callback<Event> event_callback = null;

                bool exists = events_list.TryGetValue(ev.Index, out event_callback);

                if (exists)
                {
                    event_callback.Invoke(ev);
                }
            }
        }
    }
}
