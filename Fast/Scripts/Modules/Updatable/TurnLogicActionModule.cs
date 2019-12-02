using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Modules
{
    class TurnLogicActionModule : UpdatableModule
    {
        private List<Logic.LogicAction> actions_to_play = new List<Logic.LogicAction>();

        private Logic.LogicAction playing_action = null;

        public override void Update()
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

        private void UpdateLogic()
        {
            if(playing_action != null)
            {
                if(playing_action.Finished)
                {
                    playing_action = null;

                    UpdateLogic();
                }
            }
            else
            {
                if(actions_to_play.Count > 0)
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
