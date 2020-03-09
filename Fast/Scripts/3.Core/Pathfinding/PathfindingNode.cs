using System;
using UnityEngine;

namespace Fast.Pathfinding
{
    public class PathfindingNode
    {
        private PathfindingNode parent_node = null;
        private Vector2Int grid_pos = Vector2Int.zero;

        public PathfindingNode(PathfindingNode parent, Vector2Int grid_pos)
        {
            this.parent_node = parent;
            this.grid_pos = grid_pos;
        }

        public PathfindingNode ParentNode
        {
            get { return parent_node; }
        }

        public Vector2Int GridPos
        {
            get { return grid_pos; }
        }
    }
}
