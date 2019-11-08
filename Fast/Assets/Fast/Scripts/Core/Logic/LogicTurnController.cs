using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Logic
{
    class LogicTurnController
    {
        private List<LogicAction> actions_queue = new List<LogicAction>();

        public void PushBackAction(LogicAction action)
        {
            if(action != null)
            {
                actions_queue.Add(action);
            }
        }

        public void PushFrontAction(LogicAction action)
        {
            if (action != null)
            {
                actions_queue.Insert(0, action);
            }
        }

        public void ForceFinishCurrAction()
        {
            if (actions_queue.Count > 0)
            {
                LogicAction curr_action = actions_queue[0];

                curr_action.Finish();

                Update();
            }
        }

        public void ForceFinishAllActions()
        {
            for(int i = 0; i < actions_queue.Count; ++i)
            {
                LogicAction curr_action = actions_queue[i];

                if (!curr_action.Started)
                {
                    curr_action.Start();
                }

                curr_action.Finish();
            }

            actions_queue.Clear();
        }

        public void Update()
        {
            if(actions_queue.Count > 0)
            {
                LogicAction curr_action = actions_queue[0];

                if(!curr_action.Started)
                {
                    curr_action.Start();
                }

                if(curr_action.Finished)
                {
                    actions_queue.RemoveAt(0);
                }
            }
        }
    }
}
