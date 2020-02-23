using System.Collections.Generic;
using UnityEngine;
using Fast.Containers;

namespace Fast.Pathfinding
{
    class CalculatePathAStarTimeSliced : Fast.TimeSliced.TimeSlicedTask
    {
        private PathfindingConfiguration config = null;

        private Vector2Int grid_pos_origin = Vector2Int.zero;
        private Vector2Int grid_pos_dest = Vector2Int.zero;

        private PriorityQueue<PathfindingNode> queue_to_check = new PriorityQueue<PathfindingNode>();
        private List<Vector2Int> already_visited_list = new List<Vector2Int>();
        private PriorityQueue<PathfindingNode> already_visited_priority_queue = new PriorityQueue<PathfindingNode>();

        private PathfindingPath final_path = new PathfindingPath();

        public CalculatePathAStarTimeSliced(PathfindingConfiguration config, Vector2Int grid_pos_origin, Vector2Int grid_pos_dest)
        {
            this.config = config;
            this.grid_pos_origin = grid_pos_origin;
            this.grid_pos_dest = grid_pos_dest;
        }

        public override void OnStartInternal()
        {
            final_path.SetState(false, false);

            bool origin_walkable = config.GridPositionIsWalkable(grid_pos_origin);
            bool destination_walkable = config.GridPositionIsWalkable(grid_pos_dest);

            if (origin_walkable && destination_walkable)
            {
                PathfindingNode curr_node = new PathfindingNode(null, grid_pos_origin);

                float curr_distance = config.GridPositionsWorldDistance(grid_pos_origin, grid_pos_dest);

                queue_to_check.Add(curr_node, curr_distance);
            }
            else
            {
                Finish();
            }
        }

        public override void OnUpdateInternal()
        {
            if (queue_to_check.Count > 0)
            {
                PathfindingNode to_check = queue_to_check.PopFront();

                if (to_check.GridPos != grid_pos_dest)
                {
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

                                float curr_distance = config.GridPositionsWorldDistance(curr_pos, grid_pos_dest);

                                already_visited_list.Add(curr_node.GridPos);
                                already_visited_priority_queue.Add(curr_node, curr_distance);

                                queue_to_check.Add(curr_node, curr_distance);
                            }
                        }
                    }
                }
                else
                {
                    BacktrackPathfindingNode(to_check, ref final_path);

                    final_path.SetState(false, true);

                    Finish();
                }
            }
            else
            {
                PathfindingNode to_check = already_visited_priority_queue.PopFront();

                BacktrackPathfindingNode(to_check, ref final_path);

                final_path.SetState(false, false);

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

        private void BacktrackPathfindingNode(PathfindingNode node, ref PathfindingPath ret)
        {
            while (node.ParentNode != null)
            {
                ret.Nodes.Insert(0, node);

                node = node.ParentNode;
            }
        }

        public PathfindingPath FinalPath
        {
            get { return final_path; }
        }
    }
}
