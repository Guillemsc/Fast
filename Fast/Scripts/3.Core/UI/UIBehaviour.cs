using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.UI
{
    public class UIBehaviour
    {
        protected readonly UIController controller = null;

        private int behaviour_index = 0;

        private readonly List<UIInstruction> instructions = new List<UIInstruction>();

        public UIBehaviour(UIController controller, int behaviour_index)
        {
            this.controller = controller;
            this.behaviour_index = behaviour_index;
        }

        public UIController Controller => controller;
        public int BehaviourIndex => behaviour_index;
        public IReadOnlyList<UIInstruction> Instructions => instructions;

        public void AddInstruction(UIInstruction instruction)
        {
            instructions.Add(instruction);
        }
    }
}
