using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fast.Pathfinding
{
    public class PathfindingConfiguration
    {
        public virtual bool GridPositionIsWalkable(Vector2Int grid_pos)
        {
            return false;
        }

        public virtual float GridPositionsWorldDistance(Vector2Int grid_pos1, Vector2Int grid_pos2)
        {
            return 0.0f;
        }

        public virtual List<Vector2Int> GetNextGridPositionsToCheck(Vector2Int curr_pos)
        {
            //List<Vector2Int> ret = new List<Vector2Int>();

            //Vector2Int up_pos = curr_pos + new Vector2Int(0, 1);
            //Vector2Int down_pos = curr_pos + new Vector2Int(0, -1);
            //Vector2Int left_pos = curr_pos + new Vector2Int(-1, 0);
            //Vector2Int right_pos = curr_pos + new Vector2Int(1, 0);

            //ret.Add(up_pos);
            //ret.Add(down_pos);
            //ret.Add(left_pos);
            //ret.Add(right_pos);

            return new List<Vector2Int>();
        }
    }
}
