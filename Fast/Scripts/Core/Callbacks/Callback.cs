using System;
using System.Collections.Generic;

namespace Fast
{
    public class Callback<T>
    {
        private List<Action<T>> actions = new List<Action<T>>();

        public void Invoke(T obj)
        {
            List<Action<T>> to_invoke = new List<Action<T>>(actions);

            for (int i = 0; i < to_invoke.Count; ++i)
            {
                Action<T> curr_action = to_invoke[i];

                curr_action.Invoke(obj);
            }
        }

        public void Subscribe(Action<T> action)
        {
            bool already_added = false;

            for (int i = 0; i < actions.Count; ++i)
            {
                Action<T> curr_action = actions[i];

                if (curr_action == action)
                {
                    already_added = true;

                    break;
                }
            }

            if (!already_added)
            {
                actions.Add(action);
            }
        }

        public void SubscribeUnique(Action<T> action)
        {
            UnSubscribeAll();

            Subscribe(action);
        }

        public void UnSubscribe(Action<T> action)
        {
            for (int i = 0; i < actions.Count;)
            {
                Action<T> curr_action = actions[i];

                if(curr_action == action)
                {
                    actions.RemoveAt(i);
                }
                else
                {
                    ++i;
                }
            }
        }

        public void UnSubscribeAll()
        {
            actions.Clear();
        }
    }

    public class Callback
    {
        private List<Action> actions = new List<Action>();

        public void Invoke()
        {
            List<Action> to_invoke = new List<Action>(actions);

            for (int i = 0; i < to_invoke.Count; ++i)
            {
                Action curr_action = to_invoke[i];

                if(curr_action != null)
                    curr_action.Invoke();
            }
        }

        public void Subscribe(Action action)
        {
            if (action != null)
            {
                actions.Add(action);
            }
        }

        public void SubscribeUnique(Action action)
        {
            if (action != null)
            {
                bool already_added = false;

                for (int i = 0; i < actions.Count; ++i)
                {
                    Action curr_action = actions[i];

                    if (curr_action == action)
                    {
                        already_added = true;

                        break;
                    }
                }

                if (!already_added)
                {
                    actions.Add(action);
                }
            }
        }

        public void UnSubscribe(Action action)
        {
            if (action != null)
            {
                for (int i = 0; i < actions.Count;)
                {
                    Action curr_action = actions[i];

                    if (curr_action == action)
                    {
                        actions.RemoveAt(i);
                    }
                    else
                    {
                        ++i;
                    }
                }
            }
        }

        public void UnSubscribeAll()
        {
            actions.Clear();
        }
    }
}
