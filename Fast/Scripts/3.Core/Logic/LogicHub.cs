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
        private LogicCluster local_cluster = null;

        private View.LogicView logic_view = null;

        private bool started = false;

        private void Awake()
        {
            GatherLogicView();
        }

        private void LateUpdate()
        {
            LogicUpdateLocal();
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

        public void InitLocalLogic(LogicCluster cluster)
        {
            if(cluster == null)
            {
                return;
            }

            local_cluster = cluster;
            local_cluster.OnOutputCommandSent.Subscribe(OnOutputCommandReceived);

            Match.LogicMatchData data = InitLocalLogicInternal();

            if(data == null)
            {
                return;
            }

            local_cluster.Init(data);
        }

        public void StartLocalLogic()
        {
            if(local_cluster == null)
            {
                return;
            }

            if(!local_cluster.Initialized)
            {
                return;
            }

            if(started)
            {
                return;
            }

            started = true;

            StartLocalLogicInternal();
        }

        private void LogicUpdateLocal()
        {
            if (!started)
            {
                return;
            }

            LogicUpdateLocalInternal();
        }

        public void PushInput(Commands.ILogicInputCommand input)
        {
            if(input == null)
            {
                return;
            }

            if(local_cluster != null)
            {
                local_cluster.ReceiveInput(input);
            }
        }

        private void OnOutputCommandReceived(Commands.ILogicOutputCommand command)
        {
            if(logic_view == null)
            {
                return;
            }

            logic_view.OnCommandReceived(command);
        }

        protected virtual Match.LogicMatchData InitLocalLogicInternal()
        {
            return null;
        }

        protected virtual void StartLocalLogicInternal()
        {
           
        }

        protected virtual void LogicUpdateLocalInternal()
        {

        }
    }
}
