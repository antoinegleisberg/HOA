using UnityEngine;
using System.Collections.Generic;
using antoinegleisberg.Pathfinding;

namespace antoinegleisberg.HOA
{
    public partial class PathfindingGraph : MonoBehaviour
    {
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
            GenerateGraph();
            _pathfinder = Pathfinder<Node>.GetAStarPathfinder(
                (Node n1, Node n2) => n1.HeuristicDistance(n2), (Node n) => n.GetDistancesToNeighbours());
        }

        private void Start()
        {

        }

        private Vector2Int WorldToClosestNodeCoordinates(Vector3 worldPosition)
        {
            Vector3 interpolatedCellPosition = GridManager.Instance.WorldToInterpolatedCellPosition(worldPosition);
            Vector3 scaledInterpolatedCellPosition = _nodesPerUnit * interpolatedCellPosition;
            int nodeX = Mathf.RoundToInt(scaledInterpolatedCellPosition.x);
            int nodeY = Mathf.RoundToInt(scaledInterpolatedCellPosition.y);
            Vector2Int nodeCoords = new Vector2Int(nodeX, nodeY);
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
