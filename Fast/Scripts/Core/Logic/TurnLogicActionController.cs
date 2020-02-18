using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Logic
{
    public class TurnLogicActionController
    {
        private List<Logic.LogicAction> actions_to_play = new List<Logic.LogicAction>();

        private Logic.LogicAction playing_action = null;

        public void Update()
        {
            UpdateLogic();
        }

        public void PushBack(Logic.LogicAction logic)
        {
            if (logic != null)
            {
                actions_to_play.Add(logic);
            }
        }

        public void PushFront(Logic.LogicAction logic)
        {
            if (logic != null)
            {
                actions_to_play.Insert(0, logic);
            }
        }

        public void FinishCurrent()
        {
            if (playing_action != null)
            {
                playing_action.Finish();

                UpdateLogic();
            }
        }

        public void FinishAll()
        {
            while(actions_to_play.Count > 0)
            {
                FinishCurrent();

                UpdateLogic();
            }
        }

        private void UpdateLogic()
        {
            if (playing_action != null)
            {
                if (playing_action.Finished)
                {
                    playing_action = null;

                    UpdateLogic();
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
