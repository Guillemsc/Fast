using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fast.Logic.View
{
    public class LogicView : MonoBehaviour
    {
        private readonly List<LogicSubView> sub_views = new List<LogicSubView>();

        private void Awake()
        {
            GatherSubPresentations();
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

        public void ReceiveEffect(Commands.ILogicCommandEffect effect)
        {
            for(int i = 0; i < sub_views.Count; ++i)
            {
                sub_views[i].ReceiveEffect(effect);
            }
        }
    }
}
