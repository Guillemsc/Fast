using FlowCanvas;
using FlowCanvas.Nodes;
using NodeCanvas.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fast.Cinematics
{
    public class Cinematic 
    {
        private CinematicAsset cinematic_asset = null;

        private Graph running_graph = null;

        List<Timeline> timelines = new List<Timeline>();
        private int timelines_finished = 0;

        private Fast.Callback on_finish = new Callback();

        public Fast.Callback OnFinish => on_finish;

        public Cinematic(CinematicAsset cinematic_asset)
        {
            this.cinematic_asset = cinematic_asset;
        }

        public void Play()
        {
            if(cinematic_asset == null)
            {
                return;
            }

            if(cinematic_asset.NodeCanvasScript == null)
            {
                return;
            }

            timelines.Clear();
            timelines_finished = 0;

            running_graph = Graph.Clone<Graph>(cinematic_asset.NodeCanvasScript);

            running_graph.StartGraph(null, new Blackboard(), true, null);
  
            if (running_graph == null)
            {
                return;
            }

            List<Node> root_nodes = running_graph.GetRootNodes();

            for (int i = 0; i < root_nodes.Count; ++i)
            {
                Node curr_node = root_nodes[i];

                Timeline timeline_node = curr_node as Timeline;

                if (timeline_node == null)
                {
                    continue;
                }

                timelines.Add(timeline_node);
            }

            for (int i = 0; i < timelines.Count; ++i)
            {
                Timeline curr_timeline = timelines[i];

                curr_timeline.OnFinish.UnSubscribeAll();
                curr_timeline.OnFinish.Subscribe(OnTimelineFinishes);

                curr_timeline.StartFlow();
            }
        }

        private void OnTimelineFinishes()
        {
            ++timelines_finished;

            if (timelines.Count == timelines_finished)
            {
                on_finish.Invoke();
            }
        }

        public void ForceStop()
        {
            if (running_graph == null)
            {
                return;
            }

            List<Node> root_nodes = running_graph.GetRootNodes();

            for (int i = 0; i < root_nodes.Count; ++i)
            {
                Node curr_node = root_nodes[i];

                Timeline timeline_node = curr_node as Timeline;

                if (timeline_node == null)
                {
                    continue;
                }

                timeline_node.ForceStopFlow();
            }
        }
    }
}
