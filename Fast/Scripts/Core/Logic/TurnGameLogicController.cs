using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Logic
{
    public class TurnGameLogicController
    {
        private List<Logic.GameLogic> logic_to_play = new List<Logic.GameLogic>();

        private Logic.GameLogic playing_logic = null;

        public void Update()
        {
            UpdateLogic();
        }

        public void PushBack(Logic.GameLogic logic)
        {
            if (logic != null)
            {
                logic_to_play.Add(logic);
            }
        }

        public void PushFront(Logic.GameLogic logic)
        {
            if (logic != null)
            {
                logic_to_play.Insert(0, logic);
            }
        }

        public void FinishCurrent()
        {
            if (playing_logic != null)
            {
                playing_logic.Finish();

                UpdateLogic();
            }
        }

        public void FinishAll()
        {
            while (logic_to_play.Count > 0)
            {
                FinishCurrent();

                UpdateLogic();
            }
        }

        private void UpdateLogic()
        {
            if (playing_logic != null)
            {
                if (playing_logic.Finished)
                {
                    playing_logic = null;

                    UpdateLogic();
                }
                else
                {
                    playing_logic.Update();
                }
            }
            else
            {
                if (logic_to_play.Count > 0)
                {
                    playing_logic = logic_to_play[0];

                    logic_to_play.RemoveAt(0);

                    if (!playing_logic.Running)
                    {
                        playing_logic.Start();
                    }
                }
            }
        }
    }
}
