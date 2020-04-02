using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.UI
{
    public class UIController : Fast.IController, IUpdatable
    {
        private readonly Fast.Time.TimeContext time_context = null;

        private Form curr_form = null;
        private readonly List<Form> curr_sub_forms = new List<Form>();

        private readonly List<UIBehaviour> behaviours = new List<UIBehaviour>();

        private UIBehaviour curr_playing_behaviour = null;
        private int curr_playing_behaviour_instruction_index = 0;

        public UIController(Fast.Time.TimeContext time_context)
        {
            this.time_context = time_context;
        }

        public void Update()
        {
            UpdateCurrPlayingBehaviour();
        }

        public Fast.Time.TimeContext timeContext => time_context;

        public Form CurrForm
        {
            get { return curr_form; }
            set { curr_form = value; }
        }

        public UIBehaviour CreateBehaviour(int behaviour_index)
        {
            UIBehaviour new_behabiour = new UIBehaviour(this, behaviour_index);

            behaviours.Add(new_behabiour);

            return new_behabiour;
        }

        public void PlayBehaviour(int instruction_index)
        {
            UIBehaviour to_play = GetBehaviour(instruction_index);

            if(to_play == null)
            {
                // Log error
                return;
            }

            StartPlayingBehaviour(to_play);
        }

        private UIBehaviour GetBehaviour(int behaviour_index)
        {
            for(int i = 0; i < behaviours.Count; ++i)
            {
                UIBehaviour curr_behaviour = behaviours[i];

                if (behaviours[i].BehaviourIndex == behaviour_index)
                {
                    return curr_behaviour;
                }
            }

            return null;
        }

        private void StartPlayingBehaviour(UIBehaviour to_start)
        {
            if(curr_playing_behaviour == null)
            {
                curr_playing_behaviour = to_start;
                curr_playing_behaviour_instruction_index = 0;
            }
        }

        private void UpdateCurrPlayingBehaviour()
        {
            if(curr_playing_behaviour == null)
            {
                return;
            }

            if(curr_playing_behaviour_instruction_index >= curr_playing_behaviour.Instructions.Count)
            {
                curr_playing_behaviour = null;

                return;
            }

            UIInstruction curr_instruction = curr_playing_behaviour.Instructions[curr_playing_behaviour_instruction_index];

            if (curr_instruction.Finished)
            {
                ++curr_playing_behaviour_instruction_index;

                UpdateCurrPlayingBehaviour();
            }
            else if (!curr_instruction.Started)
            {
                curr_instruction.Start();
            }
        }
    }
}
