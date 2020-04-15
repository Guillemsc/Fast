using System;
using UnityEngine;

namespace Fast.Logic
{
    public class GameSubLogic : MonoBehaviour
    {
        private GameLogic parent_logic = null;

        private bool running = false;
        private bool finished = false;

        public void StartLogic(GameLogic parent_logic)
        {            
            this.parent_logic = parent_logic;

            finished = false;
            running = true;

            StartLogicInternal();
        }

        public void UpdateLogic()
        {            
            UpdateLogicInternal();
        }

        public void FinishLogic()
        {            
            FinishLogicInternal();

            running = false;
            finished = true;
        }

        protected GameLogic ParentLogic => parent_logic;

        protected virtual void StartLogicInternal()
        {

        }

        protected virtual void UpdateLogicInternal()
        {

        }

        protected virtual void FinishLogicInternal()
        {

        }
    }
}
