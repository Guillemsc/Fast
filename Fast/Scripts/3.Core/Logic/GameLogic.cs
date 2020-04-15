using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fast.Logic
{
    public class GameLogic : MonoBehaviour
    {
        private readonly List<GameSubLogic> game_sub_logics = new List<GameSubLogic>();

        private bool running = false;
        private bool finished = false;

        private Callback on_start = new Callback();
        private Callback on_finish = new Callback();

        public void Awake()
        {
            FindSubLogics();
        }

        public bool Running
        {
            get { return running; }
        }

        public bool Finished
        {
            get { return finished; }
        }

        private void FindSubLogics()
        {
            GameSubLogic[] sub_logic_array = gameObject.GetComponents<GameSubLogic>();

            game_sub_logics.AddRange(sub_logic_array);
        }

        public void StartLogic()
        {
            if (!running)
            {
                finished = false;
                running = true;

                on_start.Invoke();

                OnStartLogicInternal();

                for(int i = 0; i < game_sub_logics.Count; ++i)
                {
                    game_sub_logics[i].StartLogic(this);
                }
            }
        }

        public void UpdateLogic()
        {
            if(running && !finished)
            {
                OnUpdateLogicInternal();

                for (int i = 0; i < game_sub_logics.Count; ++i)
                {
                    game_sub_logics[i].UpdateLogic();
                }
            }
        }

        public void FinishLogic()
        {
            if (running && !finished)
            {
                OnFinishLogicInternal();

                for (int i = 0; i < game_sub_logics.Count; ++i)
                {
                    game_sub_logics[i].FinishLogic();
                }

                on_finish.Invoke();

                running = false;
                finished = true;
            }
        }

        public Callback OnStart
        {
            get { return on_start; }
        }

        public Callback OnFinish
        {
            get { return on_finish; }
        }

        protected virtual void OnStartLogicInternal()
        {

        }

        protected virtual void OnUpdateLogicInternal()
        {

        }

        protected virtual void OnFinishLogicInternal()
        {

        }
    }
}
