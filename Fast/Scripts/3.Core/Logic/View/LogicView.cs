using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fast.Logic.View
{
    public class LogicView : MonoBehaviour
    {
        private LogicHub logic_hub = null;
        private readonly List<LogicSubView> sub_views = new List<LogicSubView>();

        public LogicHub LogicHub => logic_hub;

        private void Awake()
        {
            GatherSubPresentations();
        }

        public void SetLogicHub(LogicHub logic_hub)
        {
            this.logic_hub = logic_hub;
        }

        private void GatherSubPresentations()
        {
            LogicSubView[] curr_sub_views = gameObject.GetComponents<LogicSubView>();
            sub_views.AddRange(curr_sub_views);

            curr_sub_views = gameObject.GetComponentsInChildren<LogicSubView>();
            sub_views.AddRange(curr_sub_views);

            for (int i = 0; i < sub_views.Count; ++i)
            {
                sub_views[i].SetLogicParent(this);
            }
        }

        public void OnCommandReceived(Commands.ILogicOutputCommand effect)
        {
            for(int i = 0; i < sub_views.Count; ++i)
            {
                sub_views[i].OnCommandReceived(effect);
            }
        }
    }
}
