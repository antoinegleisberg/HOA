using UnityEngine;
using antoinegleisberg.Types;
using System.Collections.Generic;
using System.Collections;

namespace antoinegleisberg.HOA.Core
{
    public partial class PathfindingGraph
    {
        private class Node
        {
            public Vector2Int Coordinates { get; private set; }
            private List<Node> _neighbours;

            public Node(Vector2Int coordinates)
            {
                Coordinates = coordinates;
                _neighbours = new List<Node>();
            }

            public void AddNeighbour(Node neighbour)
            {
                _neighbours.Add(neighbour);
            }

            public void RemoveNeighbour(Node neighbour)
            {
                _neighbours.Remove(neighbour);
            }

            public IReadOnlyList<Node> Neighbours()
            {
                return _neighbours;
            }

            public List<Pair<Node, int>> GetDistancesToNeighbours()
            {
                List<Pair<Node, int>> distances = new List<Pair<Node, int>>();

                foreach (Node neighbour in _neighbours)
                {
                    Vector2Int difference = Coordinates - neighbour.Coordinates;
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
                Vector2Int difference = Coordinates - other.Coordinates;
                int diffX = difference.x;
                int diffY = difference.y;
                int minDiff = Mathf.Min(diffX, diffY);
                int maxDiff = Mathf.Max(diffX, diffY);
                return 10 * (maxDiff - minDiff) + 14 * minDiff;
            }
        }
    }
}
