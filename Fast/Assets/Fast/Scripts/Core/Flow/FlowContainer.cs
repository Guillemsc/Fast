using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Flow
{
    public class FlowContainer
    {
        private FlowController controller = null;

        private int identifier_id = 0;

        private List<FlowNode> all_nodes = new List<FlowNode>();

        private List<FlowNode> nodes_to_play = new List<FlowNode>();
        private List<FlowNode> nodes_playing = new List<FlowNode>();

        private bool next_starts_with_last = false;
        private bool next_starts_at_end_of_last = false;

        public FlowContainer(FlowController controller, int identifier_id)
        {
            this.controller = controller;
            this.identifier_id = identifier_id;
        }

        public FlowController Controller
        {
            get { return controller; }
        }

        public int IdentifierId
        {
            get { return identifier_id; }
        }

        public bool NextStartsWithLast
        {
            get { return next_starts_with_last; }
            set { next_starts_with_last = value; }
        }

        public bool NextStartsAtEndOfLast
        {
            get { return next_starts_at_end_of_last; }
            set { next_starts_at_end_of_last = value; }
        }

        public void AddFlowNode(FlowNode node)
        {
            if(node != null)
            {
                all_nodes.Add(node);

                if(next_starts_with_last)
                {
                    node.StartWithLast = true;
                }
                else if(next_starts_at_end_of_last)
                {
                    node.StartAtEndOfLast = true;
                }

                next_starts_with_last = false;
                next_starts_at_end_of_last = false;
            }
        }

        public List<FlowNode> AllNodes
        {
            get { return all_nodes; }
        }

        public List<FlowNode> NodesToPlay
        {
            get { return nodes_to_play; }
        }

        public List<FlowNode> NodesPlaying
        {
            get { return nodes_playing; }
        }

        public FlowNode LastNode
        {
            get
            {
                FlowNode ret = null;

                if (all_nodes.Count > 0)
                {
                    ret = all_nodes[all_nodes.Count - 1];
                }

                return ret;
            }
        }
    }
}
