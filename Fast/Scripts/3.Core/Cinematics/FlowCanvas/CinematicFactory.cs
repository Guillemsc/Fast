using FlowCanvas;
using NodeCanvas.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Cinematics
{
    public class CinematicFactory
    {
        public static Cinematic FlowScriptToCinematic(FlowScript script)
        {
            Cinematic ret = null;

            List<CinematicTimeline> timelines = new List<CinematicTimeline>();

            List<Node> root_nodes = script.GetRootNodes();

            for (int i = 0; i < root_nodes.Count; ++i)
            {
                Node curr_node = root_nodes[i];

                if (curr_node.GetType() == typeof(NewTimeline))
                {
                    CinematicTimeline timeline = CalculateTimeline(curr_node);

                    timelines.Add(timeline);
                }
            }

            ret = new Cinematic(timelines);

            return ret;
        }

        private static CinematicTimeline CalculateTimeline(Node node)
        {
            CinematicTimeline ret = null;

            List<CinematicFlowPoint> flow_points = new List<CinematicFlowPoint>();

            Queue<Node> flow_points_to_check = new Queue<Node>();

            Node[] child_nodes = node.GetChildNodes();

            if(child_nodes.Length == 1)
            {
                Node curr_child_node = child_nodes[0];

                if (curr_child_node.GetType() == typeof(NewFlowPoint))
                {
                    flow_points_to_check.Enqueue(curr_child_node);
                }
            }

            while(flow_points_to_check.Count > 0)
            {
                Node curr_flow_point_node = flow_points_to_check.Dequeue();

                Node next_node_to_check = null;
                CinematicFlowPoint flow_point = CreateFlowPoint(curr_flow_point_node, out next_node_to_check);

                if(next_node_to_check != null)
                {
                    flow_points_to_check.Enqueue(next_node_to_check);
                }

                flow_points.Add(flow_point);
            }

            ret = new CinematicTimeline(flow_points);

            return ret;
        }

        private static CinematicFlowPoint CreateFlowPoint(Node curr_flow_point_node, out Node next_to_check)
        {
            CinematicFlowPoint ret = null;

            next_to_check = null;

            List<CinematicAction> actions = new List<CinematicAction>();

            Node[] child_nodes = curr_flow_point_node.GetChildNodes();

            for (int i = 0; i < child_nodes.Length; ++i)
            {
                Node curr_child_node = child_nodes[i];

                if (curr_child_node.GetType() == typeof(NewFlowPoint))
                {
                    next_to_check = curr_child_node;
                }
                else 
                {
                    NewAction action = curr_child_node as NewAction;

                    if(action != null)
                    {
                        CinematicAction cinematic_action = action.GetCinematicAction();

                        if(cinematic_action != null)
                        {
                            actions.Add(cinematic_action);
                        }
                    }
                }
            }

            ret = new CinematicFlowPoint(actions);

            return ret;
        }
    }
}
