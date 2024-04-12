using UnityEngine;
using antoinegleisberg.HOA.Input;
using System.Collections.Generic;

namespace antoinegleisberg.HOA
{
    public partial class PathfindingGraph : MonoBehaviour
    {
        [SerializeField] private Grid _grid;
        [SerializeField] private int _pathfindingGraphRange;
        // Maps the world positions to the nodes
        [SerializeField] private Dictionary<Vector2, Node> _nodes;

        private void Awake()
        {
            GenerateGraph();
        }

        private void Start()
        {
            InputManager.Instance.OnMouseClick += Instance_OnMouseClick;
        }

        private void Instance_OnMouseClick()
        {
            Vector3 mousePosition = InputManager.Instance.MouseScreenPosition;
            Vector3 interpolatedCellPosition = GridManager.Instance.MouseToInterpolatedCellPosition(mousePosition);
            Vector3 worldPosition = GridManager.Instance.MouseToWorldPosition(mousePosition);
            Vector3Int cellPosition = _grid.WorldToCell(worldPosition);
            Vector3 localPosition = _grid.CellToLocalInterpolated(cellPosition);
            Debug.Log("Interpolated Cell Position: " + interpolatedCellPosition);
            Debug.Log("Cell Position: " + cellPosition);
            Debug.Log("World Position: " + worldPosition);
            Debug.Log("Local Position: " + localPosition);
            Debug.Log("---------");
        }

        private void GenerateGraph()
        {
            _nodes = new Dictionary<Vector2, Node>();

            for (int x = -_pathfindingGraphRange; x <= _pathfindingGraphRange; ++x)
            {
                for (int y = -_pathfindingGraphRange; y <= _pathfindingGraphRange; y++)
                {
                    Vector2 cellPosition = new Vector2(x, y);
                    float offset = 0.5f;
                    Vector2 bottomCellCoords = cellPosition;
                    Vector2 centerCellCoords = cellPosition + offset * Vector2.right + offset * Vector2.up;
                    Vector2 rightCellCoords = cellPosition + offset * Vector2.right;
                    Vector2 leftCellCoords = cellPosition + offset * Vector2.up;

                    Node bottom = new Node(bottomCellCoords);
                    Node center = new Node(centerCellCoords);
                    Node right = new Node(rightCellCoords);
                    Node left = new Node(leftCellCoords);

                    _nodes.Add(_grid.CellToLocalInterpolated(bottomCellCoords), bottom);
                    _nodes.Add(_grid.CellToLocalInterpolated(centerCellCoords), center);
                    _nodes.Add(_grid.CellToLocalInterpolated(rightCellCoords), right);
                    _nodes.Add(_grid.CellToLocalInterpolated(leftCellCoords), left);
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (_nodes == null)
            {
                return;
            }

            foreach (Vector2 pos in _nodes.Keys)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(pos, 0.05f);
            }
        }
    }
}
