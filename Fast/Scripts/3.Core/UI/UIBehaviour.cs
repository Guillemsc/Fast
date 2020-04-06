﻿using System;
using System.Collections.Generic;

namespace Fast.UI
{
    public class UIBehaviour
    {
        private Fast.UI.UIBehaviourContext context = null;

        private readonly List<UIInstruction> instructions = new List<UIInstruction>();

        private readonly Fast.Callback on_finish = new Fast.Callback();

        public IReadOnlyList<UIInstruction> Instructions => instructions;
        public Fast.Callback OnFinish => on_finish;

        public Fast.UI.UIBehaviourContext Context
        {
            get { return context; }
            set { context = value; }
        }

        public void AppendBehaviour(UIBehaviour behaviour)
        {
            if(behaviour == null)
            {
                return;
            }

            instructions.AddRange(behaviour.Instructions);
        }

        public void AddInstruction(UIInstruction behaviour)
        {
            if(behaviour == null)
            {
                return;
            }

            instructions.Add(behaviour);
        }
    }
}
