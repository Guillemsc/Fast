using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.UI
{
    public class UIController : Fast.IController, Fast.IUpdatable
    {
        private readonly Fast.Time.TimeContext time_context = null;
        private readonly List<UIBehaviour> behaviours_to_start = new List<UIBehaviour>();
        private int curr_behaivour_instruction = 0;

        private Fast.PrefabScenes.PrefabScene<Fast.UI.Form> curr_form = null;
        private Fast.PrefabScenes.PrefabScene<Fast.UI.Form> curr_sub_form = null;
        private readonly List<Fast.PrefabScenes.PrefabScene<Fast.UI.Form>> curr_sub_forms = new List<PrefabScenes.PrefabScene<Form>>();

        public UIController(Fast.Time.TimeContext time_context)
        {
            this.time_context = time_context;
        }

        public void Update()
        {
            UpdateBehaviours();
        }

        public Fast.Time.TimeContext TimeContext => time_context;

        public Fast.PrefabScenes.PrefabScene<Fast.UI.Form> CurrForm
        {
            get { return curr_form; }
            set { curr_form = value; }
        }

        public Fast.PrefabScenes.PrefabScene<Fast.UI.Form> CurrSubForm
        {
            get { return curr_sub_form; }
            set { curr_sub_form = value; }
        }

        public List<Fast.PrefabScenes.PrefabScene<Fast.UI.Form>> CurrSubForms
        {
            get { return curr_sub_forms; }
        }

        public void PlayBehaviour(UIBehaviour behaviour)
        {
            if(behaviour == null)
            {
                return;
            }

            behaviours_to_start.Add(behaviour);
        }

        private void UpdateBehaviours()
        {
            if(behaviours_to_start.Count <= 0)
            {
                return;
            }

            UIBehaviour curr_behaviour = behaviours_to_start[0];

            if(curr_behaivour_instruction >= curr_behaviour.Instructions.Count)
            {
                curr_behaviour.OnFinish.Invoke();

                behaviours_to_start.RemoveAt(0);
                curr_behaivour_instruction = 0;

                return;
            }

            UIInstruction curr_instruction = curr_behaviour.Instructions[curr_behaivour_instruction];

            if(curr_instruction.Finished)
            {
                ++curr_behaivour_instruction;

                return;
            }
            else if (!curr_instruction.Started)
            {
                if(curr_behaivour_instruction == 0)
                {
                    Fast.UI.UIBehaviourContext context = new UIBehaviourContext(this);
                    curr_behaviour.Context = context;
                }

                curr_instruction.Start(curr_behaviour.Context);
            }
        }
    }
}
