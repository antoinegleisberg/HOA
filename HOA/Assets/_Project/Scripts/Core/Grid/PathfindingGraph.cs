using UnityEngine;
using System.Collections.Generic;
using antoinegleisberg.Pathfinding;
using antoinegleisberg.Types;

namespace antoinegleisberg.HOA
{
    public partial class PathfindingGraph : MonoBehaviour
    {
        public static PathfindingGraph Instance { get; private set; }

        // Temporary area of pathfinding graph generation
        // Will be replaced once map area loading or smthg is implemented
        [SerializeField] private int _pathfindingGraphRange;
        
        // Maps the node coordinates to the nodes
        [SerializeField] private Dictionary<Vector2Int, Node> _nodes;

        // The number of nodes per unit of length
        // For 2 nodes/unit, we have 4 nodes / cell => 9 in total if counting neighbouring cells too
        [SerializeField] private int _nodesPerUnit = 2;

        [Header("Gizmos testing")]
        [SerializeField] private Vector2Int _startPos;
        [SerializeField] private Vector2Int _destPos;

        private Pathfinder<Node> _pathfinder;

        private void Awake()
        {
            Instance = this;
            _pathfinder = Pathfinder<Node>.GetAStarPathfinder(
                (Node n1, Node n2) => n1.HeuristicDistance(n2), (Node n) => n.GetDistancesToNeighbours());
            GenerateGraph();
        }

        public List<Vector3> GetPath(Vector3 startCoords, Vector3 destinationCoords)
        {
            Vector2Int startNodeCoords = WorldToClosestNodeCoordinates(startCoords);
            Vector2Int destinationNodeCoords = WorldToClosestNodeCoordinates(destinationCoords);
            Node startNode = _nodes[startNodeCoords];
            Node destinationNode = _nodes[destinationNodeCoords];
            List<Node> path = _pathfinder.FindPath(startNode, destinationNode);

            List<Vector3> result = new List<Vector3>(2 + path.Count);
            result.Add(startCoords);
            for (int i = 0; i < path.Count; i++)
            {
                result.Add(NodeCoordinatesToWorld(path[i].Coordinates));
            }
            result.Add(destinationCoords);

            return result;
        }

        public void RemoveNodeRange(Pair<Pair<int, int>, Pair<int, int>> range)
        {
            // Coordinate range to remove
            int xMin = range.First.First * _nodesPerUnit + 1;
            int xMax = range.First.Second * _nodesPerUnit;
            int yMin = range.Second.First * _nodesPerUnit + 1;
            int yMax = range.Second.Second * _nodesPerUnit;

            for (int x = xMin; x < xMax; ++x)
            {
                for (int y = yMin; y < yMax; ++y)
                {
                    Vector2Int coords = new Vector2Int(x, y);
                    Node node = _nodes[coords];
                    foreach (Node neighbour in node.Neighbours())
                    {
                        neighbour.RemoveNeighbour(node);
                    }
                    _nodes.Remove(coords);
                }
            }
        }

        private Node GetClosestExistingNode(Vector3 scaledInterpolatedCellPosition)
        {
            float radius = 1f;
            Node closestNode = null;

            while (closestNode == null)
            {
                List<Vector2Int> nodesInRadius = GetPotentialNodesInRadius(scaledInterpolatedCellPosition, radius);
                closestNode = GetClosestExistingNodeTo(scaledInterpolatedCellPosition, nodesInRadius);
                radius *= 2;
            }

            return closestNode;
        }

        private List<Vector2Int> GetPotentialNodesInRadius(Vector3 originInScaledCellCoordinates, float radius)
        {
            int minX = Mathf.FloorToInt(originInScaledCellCoordinates.x - radius);
            int maxX = Mathf.FloorToInt(originInScaledCellCoordinates.x + radius);
            int minY = Mathf.FloorToInt(originInScaledCellCoordinates.y - radius);
            int maxY = Mathf.FloorToInt(originInScaledCellCoordinates.y + radius);
            List<Vector2Int> potentialCoords = new List<Vector2Int>();

            for (int x = minX; x <= maxX; ++x)
            {
                for (int y = minY; y <= maxY; ++y)
                {
                    if (IsInCircle(new Vector2(x, y), originInScaledCellCoordinates, radius))
                    {
                        potentialCoords.Add(new Vector2Int(x, y));
                    }
                }
            }

            return potentialCoords;
        }

        private bool IsInCircle(Vector2 point, Vector3 origin, float radius)
        {
            return Vector2.Distance(point, origin) <= radius;
        }

        private Node GetClosestExistingNodeTo(Vector3 originInScaledCellCoordinates, List<Vector2Int> potentialCoordinates)
        {
            float maxDist = float.MaxValue;
            Node closestNode = null;
            foreach (Vector2Int coords in potentialCoordinates)
            {
                if (!_nodes.ContainsKey(coords))
                {
                    continue;
                }

                if (Vector2.Distance(coords, originInScaledCellCoordinates) < maxDist)
                {
                    maxDist = Vector2.Distance(coords, originInScaledCellCoordinates);
                    closestNode = _nodes[coords];
                }
            }
            return closestNode;
        }

        private Vector2Int WorldToClosestNodeCoordinates(Vector3 worldPosition)
        {
            Vector3 interpolatedCellPosition = GridManager.Instance.WorldToInterpolatedCellPosition(worldPosition);
            Vector3 scaledInterpolatedCellPosition = _nodesPerUnit * interpolatedCellPosition;
            int nodeX = Mathf.RoundToInt(scaledInterpolatedCellPosition.x);
            int nodeY = Mathf.RoundToInt(scaledInterpolatedCellPosition.y);
            Vector2Int nodeCoords = new Vector2Int(nodeX, nodeY);
            if (!_nodes.ContainsKey(nodeCoords))
            {
                nodeCoords = GetClosestExistingNode(scaledInterpolatedCellPosition).Coordinates;
            }
            return nodeCoords;
        }

        private Vector3 NodeCoordinatesToWorld(Vector2Int nodeCoordinates)
        {
            float scaleFactor = 1.0f / _nodesPerUnit;
            Vector2 interpolatedCellCoords = scaleFactor * (Vector2)nodeCoordinates;
            return GridManager.Instance.CellInterpolatedToWorldPosition(interpolatedCellCoords);
        }

        private void GenerateGraph()
        {
            _nodes = new Dictionary<Vector2Int, Node>();

            for (int x = -_pathfindingGraphRange; x <= _pathfindingGraphRange; ++x)
            {
                for (int y = -_pathfindingGraphRange; y <= _pathfindingGraphRange; y++)
                {
                    for (int localX = 0; localX < _nodesPerUnit; localX++)
                    {
                        for (int localY = 0; localY < _nodesPerUnit; localY++)
                        {
                            // X corresponds to right and Y to up because the grid coordinate system is
                            // rotated 45 degrees relative to world coordinates
                            Vector2Int nodeCoords = _nodesPerUnit * new Vector2Int(x, y)
                                + localX * Vector2Int.right
                                + localY * Vector2Int.up;

                            Node node = new Node(nodeCoords);

                            _nodes.Add(nodeCoords, node);
                        }
                    }
                }
            }

            foreach (KeyValuePair<Vector2Int, Node> kvp in _nodes)
            {
                Vector2Int pos = kvp.Key;
                Node node = kvp.Value;

                for (int x = -1; x <= 1; ++x)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        if (x == 0 && y == 0)
                        {
                            continue;
                        }
                        
                        Vector2Int neighbourCoords = pos + new Vector2Int(x, y);
                        if (_nodes.ContainsKey(neighbourCoords))
                        {
                            node.AddNeighbour(_nodes[neighbourCoords]);
                        }
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (_nodes == null)
            {
                return;
            }

            Gizmos.color = Color.blue;

            foreach (Vector2Int nodePos in _nodes.Keys)
            {
                Vector3 worldPos = NodeCoordinatesToWorld(nodePos);
                Gizmos.DrawSphere(worldPos, 0.05f);
            }

            if (!_nodes.ContainsKey(_startPos) || !_nodes.ContainsKey(_destPos))
            {
                return;
            }

            Gizmos.color = Color.red;

            Vector3 startCoords = NodeCoordinatesToWorld(_startPos);
            Gizmos.DrawSphere(startCoords, 0.2f);
            Vector3 destCoords = NodeCoordinatesToWorld(_destPos);
            Gizmos.DrawSphere(destCoords, 0.2f);

            Gizmos.color = Color.green;

            List<Node> path = _pathfinder.FindPath(_nodes[_startPos], _nodes[_destPos]);

            foreach (Node node in path)
            {
                Vector3 nodePos = NodeCoordinatesToWorld(node.Coordinates);
                Gizmos.DrawSphere(nodePos, 0.1f);
            }
        }
    }
}
