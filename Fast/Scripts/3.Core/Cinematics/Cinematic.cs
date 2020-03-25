using FlowCanvas;
using FlowCanvas.Nodes;
using NodeCanvas.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fast.Cinematics
{
    public class Cinematic : MonoBehaviour
    {
        [SerializeField] private FlowScript script = null;

        private Graph running_graph = null;

        public void Update()
        {
            if(Input.GetKeyDown("a"))
            {
                ForceStop();
                Play();
            }
        }

        public void Play()
        {
            Blackboard bb = gameObject.GetComponent<Blackboard>();

            running_graph = Graph.Clone<Graph>(script);

            running_graph.StartGraph(this, bb, true, null);
  
            if (running_graph == null)
            {
                return;
            }

            List<Node> root_nodes = running_graph.GetRootNodes();

            for (int i = 0; i < root_nodes.Count; ++i)
            {
                Node curr_node = root_nodes[i];

                Timeline timeline_node = curr_node as Timeline;
                
                if(timeline_node == null)
                {
                    continue;
                }

                timeline_node.StartFlow();
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
