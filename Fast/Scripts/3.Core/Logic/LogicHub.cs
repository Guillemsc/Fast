using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fast.Logic
{
    // Intermediate class between shared logic and views
    public abstract class LogicHub : Fast.Scenes.SceneInstance
    {
        // Local cluster that controls the logic
        // If online match, this would be on a server
        private readonly LogicCluster local_cluster = new LogicCluster();

        private View.LogicView logic_view = null;

        private bool started = false;

        private void Awake()
        {
            GatherLogicView();
        }

        private void LateUpdate()
        {
            LogicUpdateLocal();

            UpdateLogicAndView();
        }

        private void GatherLogicView()
        {
            if(logic_view != null)
            {
                return;
            }

            logic_view = gameObject.GetComponent<View.LogicView>();

            if(logic_view != null)
            {
                logic_view.SetLogicHub(this);
            }
        }

        private void UpdateLogicAndView()
        {
            if (!started)
            {
                return;
            }

            if(logic_view == null)
            {
                return;
            }

            local_cluster.UpdateLogic();

            IReadOnlyList<Commands.ILogicCommandEffect> effects = local_cluster.PopEffects();

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

            local_cluster.Init(data);
        }

        public void LogicStartLocal()
        {
            if(!local_cluster.Initialized)
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
            local_cluster.PushInput(input);
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
