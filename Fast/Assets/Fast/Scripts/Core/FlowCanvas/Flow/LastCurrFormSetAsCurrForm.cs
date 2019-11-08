﻿using System.Collections;
using FlowCanvas;
using FlowCanvas.Nodes;
using ParadoxNotion.Design;
using ParadoxNotion.Services;
using UnityEngine;

namespace Fast.FastFlowCanvas.Flow
{
    [Name("Last Curr Form Set As Curr Form")]
    [Category("Fast/Flow")]
    [Description("LastCurrFormSetAsCurrForm")]
    public class LastCurrFormSetAsCurrForm : EventNode
    {
        private FlowInput enter;
        private FlowOutput exit;

        protected override void RegisterPorts()
        {
            enter = AddFlowInput("Enter", Enter);
            exit = AddFlowOutput("Exit");
        }

        private void Enter(FlowCanvas.Flow flow)
        {
            FastInstance.Instance.MFlow.CurrFlowContainer.FlowLastCurrFormSetAsCurrForm();

            exit.Call(flow);
        }
    }
}