using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fast.Logic
{
    public abstract class LogicHub : Fast.Scenes.SceneInstance
    {
        private readonly LogicCluster cluster = new LogicCluster();

        private View.LogicView logic_view = null;

        private bool started = false;

        private void Awake()
        {
            GatherLogicPresentation();
        }

        private void Update()
        {
            LogicUpdateLocal();

            UpdateLogicAndPresentation();
        }

        private void GatherLogicPresentation()
        {
            if(logic_view != null)
            {
                return;
            }

            logic_view = gameObject.GetComponent<View.LogicView>();
        }

        private void UpdateLogicAndPresentation()
        {
            if (!started)
            {
                return;
            }

            if(logic_view == null)
            {
                return;
            }

            cluster.UpdateLogic();

            IReadOnlyList<Commands.ILogicCommandEffect> effects = cluster.PopEffects();

            for(int i = 0; i < effects.Count; ++i)
            {
                Commands.ILogicCommandEffect curr_effect = effects[i];

                logic_view.ReceiveEffect(curr_effect);
            }
        }

        public void LogicInitLocal()
        {
            Match.LogicMatchData data = LogicInitLocalInternal();

            if(data == null)
            {
                return;
            }

            cluster.Init(data);
        }

        public void LogicStartLocal()
        {
            if(!cluster.Initialized)
            {
                return;
            }

            if(started)
            {
                return;
            }

            started = true;

            LogicStartLocalInternal();
        }

        private void LogicUpdateLocal()
        {
            if (!started)
            {
                return;
            }

            LogicUpdateLocalInternal();
        }

        public void PushInput(Commands.ILogicCommandInput input)
        {
            cluster.PushInput(input);
        }

        protected virtual Match.LogicMatchData LogicInitLocalInternal()
        {
            return null;
        }

        protected virtual void LogicStartLocalInternal()
        {
           
        }

        protected virtual void LogicUpdateLocalInternal()
        {

        }
    }
}
