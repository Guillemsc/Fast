using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fast.Logic.Presentation
{
    public class LogicPresentation : MonoBehaviour
    {
        private readonly List<LogicSubPresentation> sub_presentations = new List<LogicSubPresentation>();

        private void Awake()
        {
            GatherSubPresentations();
        }

        private void GatherSubPresentations()
        {
            LogicSubPresentation[] curr_sub_presentations = gameObject.GetComponents<LogicSubPresentation>();
            sub_presentations.AddRange(curr_sub_presentations);

            for(int i = 0; i < sub_presentations.Count; ++i)
            {
                sub_presentations[i].SetLogicParent(this);
            }

            curr_sub_presentations = gameObject.GetComponentsInChildren<LogicSubPresentation>();
            sub_presentations.AddRange(curr_sub_presentations);

            for (int i = 0; i < sub_presentations.Count; ++i)
            {
                sub_presentations[i].SetLogicParent(this);
            }
        }

        public void ReceiveEffect(Commands.ILogicCommandEffect effect)
        {
            for(int i = 0; i < sub_presentations.Count; ++i)
            {
                sub_presentations[i].ReceiveEffect(effect);
            }
        }
    }
}
