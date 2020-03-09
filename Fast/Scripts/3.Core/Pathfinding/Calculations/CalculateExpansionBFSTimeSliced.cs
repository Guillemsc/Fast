using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fast.Pathfinding
{    
    class CalculateExpansionBFSTimeSliced : Fast.TimeSliced.TimeSlicedTask
    {
        private IPathfindingConfiguration config = default;

        private Vector2Int grid_pos_origin = Vector2Int.zero;
        private int range = 0;

        private List<PathfindingNode> list_to_check = new List<PathfindingNode>();
        private List<Vector2Int> already_visited_list = new List<Vector2Int>();

        private List<PathfindingNode> final_expansion_list = new List<PathfindingNode>();

        public CalculateExpansionBFSTimeSliced(IPathfindingConfiguration config, Vector2Int grid_pos_origin, int range)
        {
            this.grid_pos_origin = grid_pos_origin;
            this.range = range;
        }

        public override void OnStartInternal()
        {
            bool origin_walkable = config.GridPositionIsWalkable(grid_pos_origin);

            if (origin_walkable)
            {
                PathfindingNode curr_node = new PathfindingNode(null, grid_pos_origin);

                list_to_check.Add(curr_node);
            }
            else
            {
                Finish();
            }
        }

        public override void OnUpdateInternal()
        {
            if (list_to_check.Count > 0)
            {
                PathfindingNode to_check = list_to_check[0];

                list_to_check.RemoveAt(0);

                List<Vector2Int> to_add = config.GetNextGridPositionsToCheck(to_check.GridPos);

                for (int i = 0; i < to_add.Count; ++i)
                {
                    Vector2Int curr_pos = to_add[i];

                    bool walkable = config.GridPositionIsWalkable(curr_pos); 

                    if (walkable)
                    {
                        bool already_visited = GridPosWasAlreadyVisited(curr_pos);

                        if (!already_visited)
                        {
                            PathfindingNode curr_node = new PathfindingNode(to_check, curr_pos);

                            bool inside_range = GridPosIsInsideRange(curr_node.GridPos);

                            if (inside_range)
                            {                                    
                                list_to_check.Add(curr_node);

                                final_expansion_list.Add(curr_node);
                            }

                            already_visited_list.Add(curr_node.GridPos);
                        }
                    }
                }
            }
            else
            {
                Finish();
            }
        }

        private bool GridPosWasAlreadyVisited(Vector2Int grid_pos)
        {
            bool ret = false;

            for (int i = 0; i < already_visited_list.Count; ++i)
            {
                if (already_visited_list[i] == grid_pos)
                {
                    ret = true;

                    break;
                }
            }

            return ret;
        }

        private bool GridPosIsInsideRange(Vector2Int grid_pos)
        {
            bool ret = false;

            Vector2 distance_from_origin = grid_pos - grid_pos_origin;

            if (Mathf.Abs(distance_from_origin.x) < range)
            {
                if (Mathf.Abs(distance_from_origin.y) < range)
                {
                    ret = true;
                }
            }

            return ret;
        }

        public List<PathfindingNode> FinalExpansionList
        {
            get { return final_expansion_list; }
        }
    }
}
