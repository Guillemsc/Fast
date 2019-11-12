
namespace Fast
{
    class Event
    {
        private int index = 0;

        public Event(int event_index)
        {
            this.index = event_index;
        }

        public int Index
        {
            get { return index; }
        }
    }
}
