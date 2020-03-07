using System;
using System.Collections.Generic;

namespace Fast.Pathfinding
{
    public class PathfindingPath
    {
        private bool error = false;
        private bool complete = false;

        private List<PathfindingNode> nodes = new List<PathfindingNode>();

        public PathfindingPath()
        {

        }

        public void SetState(bool error, bool complete)
        {
            this.error = error;
            this.complete = complete;
        }

        public bool Error
        {
            get { return error; }
        }

        public bool Complete
        {
            get { return complete; }
        }

        public List<PathfindingNode> Nodes
        {
            get { return nodes; }
        }
    }
}
