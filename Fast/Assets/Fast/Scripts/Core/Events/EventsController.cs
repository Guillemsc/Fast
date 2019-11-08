using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast
{
    class EventsController
    {
        private Dictionary<int, Callback<int>> events_list = new Dictionary<int, Callback<int>>();

        public void Subscribe(int event_id, Action<int> callback)
        {
            Callback<int> event_callback = null;

            bool exists = events_list.TryGetValue(event_id, out event_callback);

            if(!exists)
            {
                event_callback = new Callback<int>();

                events_list[event_id] = event_callback;
            }

            event_callback.Subscribe(callback);
        }

        public void UnSubscribe(int event_id, Action<int> callback)
        {
            Callback<int> event_callback = null;

            bool exists = events_list.TryGetValue(event_id, out event_callback);

            if(exists)
            {
                event_callback.UnSubscribe(callback);
            }
        }

        public void Invoke(int event_id)
        {
            Callback<int> event_callback = null;

            bool exists = events_list.TryGetValue(event_id, out event_callback);

            if (exists)
            {
                event_callback.Invoke(event_id);
            }
        }
    }
}
