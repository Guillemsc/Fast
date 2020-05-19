using System;
using System.Collections.Generic;

namespace Fast.Logic.View
{
    public class LogicViewActionsController : Fast.IController, Fast.IUpdatable
    {
        private readonly List<LogicViewAction> actions_to_play = new List<LogicViewAction>();

        public void Update()
        {
            UpdateActions();
        }

        private void UpdateActions()
        {
            if(actions_to_play.Count <= 0)
            {
                return;
            }

            LogicViewAction curr_action = actions_to_play[0];

            if(!curr_action.Started)
            {
                curr_action.Start();
            }
            else if(!curr_action.Finished)
            {
                curr_action.Update();
            }
            else
            {
                actions_to_play.RemoveAt(0);
            }
        }

        public void PushAction(LogicViewAction action)
        {
            if(action == null)
            {
                return;
            }

            actions_to_play.Add(action);
        }
    }
}
