using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Modules
{
    public class LogicModule : UpdatableModule
    {
        private readonly List<Architecture.GameLogic> game_logic_running = new List<Architecture.GameLogic>();

        public override void Update()
        {
            UpdateLogic();
        }

        public void StartLogic(Architecture.GameLogic logic)
        {
            if (logic != null)
            {
                bool already_running = false;

                for (int i = 0; i < game_logic_running.Count; ++i)
                {
                    Architecture.GameLogic curr_game_logic = game_logic_running[i];

                    if (curr_game_logic == logic)
                    {
                        already_running = true;

                        break;
                    }
                }

                if (!already_running)
                {
                    if (!logic.Running)
                    {
                        game_logic_running.Add(logic);
                    }
                }
            }
        }

        public void StopLogic(Architecture.GameLogic logic)
        {
            if (logic != null)
            {
                if (logic.Running)
                {
                    logic.Finish();

                    for (int i = 0; i < game_logic_running.Count; ++i)
                    {
                        Architecture.GameLogic curr_game_logic = game_logic_running[i];

                        if (curr_game_logic == logic)
                        {
                            game_logic_running.RemoveAt(i);

                            break;
                        }
                    }
                }
            }
        }

        private void UpdateLogic()
        {
            List<Architecture.GameLogic> to_start = new List<Architecture.GameLogic>(game_logic_running);

            for (int i = 0; i < to_start.Count; ++i)
            {
                Architecture.GameLogic curr_game_logic = to_start[i];

                if (!curr_game_logic.Running)
                {
                    curr_game_logic.Start();
                }
            }

            List<Architecture.GameLogic> to_update = new List<Architecture.GameLogic>(game_logic_running);

            for (int i = 0; i < to_update.Count; ++i)
            {
                Architecture.GameLogic curr_game_logic = to_update[i];

                curr_game_logic.Update();
            }
        }
    }
}
