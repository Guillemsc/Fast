using FlowCanvas;
using FlowCanvas.Nodes;
using ParadoxNotion.Design;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fast.Cinematics
{
    [Name("Flow Point")]
    [Category("Fast/Cinematics")]
    [Description("Start timeline")]
    [Color("fc8c03")]
    public class FlowPoint : Finishable
    {
        [SerializeField] private int output_ports_count = 1;

        private FlowInput main_flow_in = null;
        private FlowOutput main_flow_out = null;

        private List<FlowOutput> finishables_outputs = new List<FlowOutput>();
        private int valid_finishable_outputs = 0;
        private int finishables_outputs_finished = 0;

        protected override void OnNodeGUI()
        {
            base.OnNodeGUI();

            DrawPortModifiersGUI();
        }

        protected override void RegisterPorts()
        {
            main_flow_in = AddFlowInput("MainFlow", OnFlowInput);
            main_flow_out = AddFlowOutput("MainFlow");

            RegisterCustomPorts();
        }

        private void DrawPortModifiersGUI()
        {
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("+", GUILayout.MaxWidth(50)))
                {
                    ++output_ports_count;

                    base.GatherPorts();
                }

                if (GUILayout.Button("-", GUILayout.MaxWidth(50)))
                {
                    --output_ports_count;

                    if (output_ports_count < 0)
                    {
                        output_ports_count = 0;
                    }

                    base.GatherPorts();
                }
            }
            GUILayout.EndHorizontal();
        }

        protected void RegisterCustomPorts()
        {
            finishables_outputs.Clear();

            for (int i = 0; i < output_ports_count; ++i)
            {
                finishables_outputs.Add(null);

                finishables_outputs[finishables_outputs.Count - 1] = AddFlowOutput($"Action flow: {i}");
            }
        }

        private void OnFlowInput(FlowCanvas.Flow flow)
        {
            StartFinishablesOutputs(flow);
        }

        private void StartFinishablesOutputs(FlowCanvas.Flow flow)
        {
            finishables_outputs_finished = 0;
            valid_finishable_outputs = 0;

            for (int i = 0; i < finishables_outputs.Count; ++i)
            {
                FlowOutput curr_flow_output = finishables_outputs[i];

                Finishable finishable = Utils.GetFinishableFromFlowOutput(curr_flow_output);

                if(finishable == null)
                {
                    continue;
                }

                ++valid_finishable_outputs;

                finishable.OnFinish.UnSubscribeAll();
                finishable.OnFinish.Subscribe(OnFinishablesOutputsFinished);

                curr_flow_output.Call(flow);
            }
        }

        private void OnFinishablesOutputsFinished()
        {
            ++finishables_outputs_finished;

            if(finishables_outputs_finished == valid_finishable_outputs)
            {
                Finish();
            }
        }

        public FlowPoint GetNextFlowPoint()
        {
            BinderConnection[] connections = main_flow_out.GetPortConnections();

            for (int i = 0; i < connections.Length; ++i)
            {
                BinderConnection curr_connection = connections[i];

                FlowPoint curr_flow_point = curr_connection.targetNode as FlowPoint;

                if (curr_flow_point != null)
                {
                    return curr_flow_point;
                }
            }

            return null;
        }

        public void StartNextFlowPoint(FlowCanvas.Flow flow)
        {
            main_flow_out.Call(flow);
        }

        public void ForceStopFlowPoint()
        {
            ForceStopFinishableOutputs();
        }

        private void ForceStopFinishableOutputs()
        {
            for (int i = 0; i < finishables_outputs.Count; ++i)
            {
                FlowOutput curr_flow_output = finishables_outputs[i];

                Finishable finishable = Utils.GetFinishableFromFlowOutput(curr_flow_output);

                if (finishable == null)
                {
                    continue;
                }

                finishable.OnFinish.UnSubscribeAll();
                finishable.Finish(false);
            }
        }
    }
}
