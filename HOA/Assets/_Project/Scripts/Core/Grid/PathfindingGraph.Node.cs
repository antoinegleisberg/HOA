using UnityEngine;
using antoinegleisberg.Types;
using System.Collections.Generic;

namespace antoinegleisberg.HOA
{
    public partial class PathfindingGraph
    {
        private class Node
        {
            public Vector2 InterpolatedCellPosition { get; private set; }
            private Vector2Int _adjustedInterpolatedCellPosition;
            private List<Node> _neighbours;

            public Node(Vector2 interpolatedCellPosition)
            {
                InterpolatedCellPosition = interpolatedCellPosition;
                _neighbours = new List<Node>();
                int adjustedX = Mathf.RoundToInt(2 * interpolatedCellPosition.x);
                int adjustedY = Mathf.RoundToInt(2 * interpolatedCellPosition.y);
                _adjustedInterpolatedCellPosition = new Vector2Int(adjustedX, adjustedY);
            }

            public void AddNeighbour(Node neighbour)
            {
                _neighbours.Add(neighbour);
            }

            public List<Pair<Node, int>> GetDistancesToNeighbours()
            {
                List<Pair<Node, int>> distances = new List<Pair<Node, int>>();

                foreach (Node neighbour in _neighbours)
                {
                    Vector2Int difference = _adjustedInterpolatedCellPosition - neighbour._adjustedInterpolatedCellPosition;
                    int distance = 14;
                    if (difference.x == 0 || difference.y == 0)
                    {
                        distance = 10;
                    }
                    distances.Add(new Pair<Node, int>(neighbour, distance));
                }

                return distances;
            }

            public int HeuristicDistance(Node other)
            {
                Vector2Int difference = _adjustedInterpolatedCellPosition - other._adjustedInterpolatedCellPosition;
                int diffX = difference.x;
                int diffY = difference.y;
                int minDiff = Mathf.Min(diffX, diffY);
                int maxDiff = Mathf.Max(diffX, diffY);
                return 10 * (maxDiff - minDiff) + 14 * minDiff;
            }
        }
    }
}
