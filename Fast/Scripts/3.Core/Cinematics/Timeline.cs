using FlowCanvas;
using FlowCanvas.Nodes;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System;
using System.Collections.Generic;

namespace Fast.Cinematics
{
    [Name("Timeline")]
    [Category("Fast/NewCinematics")]
    [Description("Start timeline")]
    [Color("0384fc")]
    public class Timeline : EventNode
    {
        private FlowOutput main_flow_out = null;

        private FlowCanvas.Flow flow = default;

        private FlowPoint curr_flow_point = null;

        protected override void RegisterPorts()
        {
            main_flow_out = AddFlowOutput("MainFlow");

            TimelineRegisterPorts();
        }

        public void StartFlow()
        {
            curr_flow_point = null;

            StartNextFlowPoint();
        }

        public void ForceStopFlow()
        {
            if(curr_flow_point == null)
            {
                return;
            }

            curr_flow_point.OnFinish.UnSubscribeAll();
            curr_flow_point.ForceStopFlowPoint();
            curr_flow_point.Finish(false);
        }

        private void StartNextFlowPoint()
        {
            if(curr_flow_point == null)
            {
                BinderConnection[] connections = main_flow_out.GetPortConnections();

                for(int i = 0; i < connections.Length; ++i)
                {
                    BinderConnection curr_connection = connections[i];

                    curr_flow_point = curr_connection.targetNode as FlowPoint;

                    if(curr_flow_point != null)
                    {
                        break;
                    }
                }

                if(curr_flow_point != null)
                {
                    curr_flow_point.OnFinish.UnSubscribeAll();
                    curr_flow_point.OnFinish.Subscribe(StartNextFlowPoint);

                    flow = new FlowCanvas.Flow();

                    main_flow_out.Call(flow);
                }
            }
            else
            {
                if(curr_flow_point != null)
                {
                    FlowPoint next_flow_point = curr_flow_point.GetNextFlowPoint();

                    if(next_flow_point == null)
                    {
                        return;
                    }

                    curr_flow_point.StartNextFlowPoint(flow);

                    curr_flow_point = next_flow_point;

                    curr_flow_point.OnFinish.UnSubscribeAll();
                    curr_flow_point.OnFinish.Subscribe(StartNextFlowPoint);
                }
            }
        }

        protected virtual void TimelineRegisterPorts()
        {

        }
    }
}
