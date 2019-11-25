using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Fast.Flow
{
    public class FlowController
    {
        private List<FlowContainer> all_containers = new List<FlowContainer>();

        private List<FlowContainer> containers_to_play = new List<FlowContainer>();
        private FlowContainer container_playing = null;

        private FlowState flow_state = new FlowState();

        public FlowState FlowState
        {
            get { return flow_state; }
        }

        public FlowContainer CreateContainer(int identifier_id)
        {
            FlowContainer ret = new FlowContainer(this, identifier_id);

            all_containers.Add(ret);

            return ret;
        }

        private FlowContainer GetContainer(int identifier_id)
        {
            FlowContainer ret = null;

            for (int i = 0; i < all_containers.Count; ++i)
            {
                FlowContainer curr_container = all_containers[i];

                if(curr_container.IdentifierId == identifier_id)
                {
                    ret = curr_container;

                    break;
                }
            }

            return ret;
        }

        public bool RunContainer(int identifier_id, out FlowContainer container)
        {
            bool ret = false;

            container = null;

            if (container_playing == null && containers_to_play.Count == 0)
            {
                FlowContainer to_run = GetContainer(identifier_id);

                if (to_run != null)
                {
                    ret = true;

                    container = to_run;

                    containers_to_play.Add(to_run);

                    Update();
                }
                else
                {
                    Debug.LogError("[Fast.Flow.FlowController] The container identifier_id could not be found");
                }
            }

            return ret;
        }

        public bool PushRunContainer(int identifier_id, out FlowContainer container)
        {
            bool ret = false;

            container = null;

            FlowContainer to_run = GetContainer(identifier_id);

            if (to_run != null)
            {
                ret = true;

                container = to_run;

                containers_to_play.Add(to_run);

                Update();
            }
            else
            {
                Debug.LogError("[Fast.Flow.FlowController] The container identifier_id could not be found");
            }

            return ret;
        }

        public void Update()
        {
            if(container_playing == null)
            {
                if(containers_to_play.Count > 0)
                {
                    container_playing = containers_to_play[0];

                    StartContainer(container_playing);

                    containers_to_play.RemoveAt(0);

                    UpdateContainer(container_playing);
                }
            }
            else
            {
                UpdateContainer(container_playing);
            }
        }

        private void StartContainer(FlowContainer container)
        {
            container.OnStart.Invoke();

            container.NodesToPlay.AddRange(container.AllNodes);
        }

        private void UpdateContainer(FlowContainer container)
        {
            List<FlowNode> nodes_to_start = new List<FlowNode>();

            if(container.NodesPlaying.Count > 0)
            {
                for(int i = 0; i < container.NodesPlaying.Count;)
                {
                    FlowNode curr_node = container.NodesPlaying[i];

                    if(curr_node.Finished)
                    {
                        container.NodesPlaying.RemoveAt(i);

                        if(curr_node.PushedAtEnd.Count > 0)
                        {
                            for(int y = 0; y < curr_node.PushedAtEnd.Count; ++y)
                            {
                                nodes_to_start.Add(curr_node.PushedAtEnd[y]);
                            }

                            curr_node.PushedAtEnd.Clear();
                        }
                    }
                    else
                    {
                        ++i;
                    }
                }
            }

            if(container.NodesToPlay.Count > 0 && container.NodesPlaying.Count == 0)
            {
                bool add_to_start = true;

                FlowNode last_node = null;

                while (add_to_start && container.NodesToPlay.Count > 0)
                {
                    add_to_start = false;

                    FlowNode node_to_check = container.NodesToPlay[0];

                    if(last_node != null)
                    {
                        if(node_to_check.StartWithLast)
                        {
                            add_to_start = true;
                        }
                        else if(node_to_check.StartAtEndOfLast)
                        {
                            last_node.AddToPushedAtEnd(node_to_check);
                        }
                    }
                    else
                    {
                        add_to_start = true;
                    }

                    if(add_to_start)
                    {
                        container.NodesToPlay.RemoveAt(0);

                        nodes_to_start.Add(node_to_check);
                    }

                    last_node = node_to_check;
                }
            }

            for (int i = 0; i < nodes_to_start.Count; ++i)
            {
                FlowNode node_to_start = nodes_to_start[i];

                node_to_start.Run();

                container.NodesPlaying.Add(node_to_start);
            }

            nodes_to_start.Clear();

            if (container.NodesToPlay.Count == 0 && container.NodesPlaying.Count == 0)
            {
                FinishContainer(container);
            }
        }

        private void FinishContainer(FlowContainer container)
        {
            container.OnFinish.Invoke();

            container_playing = null;
        }
    }
}
