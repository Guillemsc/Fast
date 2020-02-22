using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Logic
{
    public class TurnLogicActionController
    {
        private List<LogicAction> actions_to_play = new List<LogicAction>();

        private LogicAction playing_action = null;

        public void Update()
        {
            UpdateActions();
        }

        public void PushBack(LogicAction action)
        {
            if (action != null)
            {
                actions_to_play.Add(action);
            }
        }

        public void PushFront(LogicAction action)
        {
            if (action != null)
            {
                actions_to_play.Insert(0, action);
            }
        }

        public void FinishCurrent()
        {
            if (playing_action != null)
            {
                playing_action.Finish();

                UpdateActions();
            }
        }

        public void FinishAll()
        {
            while(actions_to_play.Count > 0)
            {
                FinishCurrent();

                UpdateActions();
            }
        }

        private void UpdateActions()
        {
            if (playing_action != null)
            {
                if (playing_action.Finished)
                {
                    playing_action = null;

                    UpdateActions();
                }
            }
            else
            {
                if (actions_to_play.Count > 0)
                {
                    playing_action = actions_to_play[0];

                    actions_to_play.RemoveAt(0);

                    if (!playing_action.Started)
                    {
                        playing_action.Start();
                    }
                }
            }
        }
    }
}
