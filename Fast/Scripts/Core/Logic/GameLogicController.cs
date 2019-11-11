using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Logic
{
    class GameLogicController
    {
        private List<GameLogic> game_logic_running = new List<GameLogic>();

        public void StartLogic(GameLogic logic)
        {
            if (logic != null)
            {
                bool already_running = false;

                for(int i = 0; i < game_logic_running.Count; ++i)
                {
                    GameLogic curr_game_logic = game_logic_running[i];

                    if(curr_game_logic == logic)
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

        public void StopLogic(GameLogic logic)
        {
            if (logic != null)
            {
                for (int i = 0; i < game_logic_running.Count; ++i)
                {
                    GameLogic curr_game_logic = game_logic_running[i];

                    if (curr_game_logic == logic)
                    {
                        if(curr_game_logic.Running)
                        {
                            curr_game_logic.Finish();
                        }

                        game_logic_running.RemoveAt(i);

                        break;
                    }
                }
            }
        }

        public void Update()
        {
            List<GameLogic> to_update = new List<GameLogic>(game_logic_running);

            for (int i = 0; i < to_update.Count; ++i)
            {
                GameLogic curr_game_logic = to_update[i];

                if(!curr_game_logic.Running)
                {
                    curr_game_logic.Start();
                }

                curr_game_logic.Update();
            }
        }
    }
}
