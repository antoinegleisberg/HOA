using UnityEngine;

namespace antoinegleisberg.HOA.Core
{
    public class GridManager : MonoBehaviour
    {
        public static GridManager Instance { get; private set; }

        [SerializeField] private Grid _grid;

        private Camera _mainCamera;

        private void Awake()
        {
            Instance = this;
            _mainCamera = Camera.main;
        }

        public Vector3 MouseToWorldPosition(Vector3 mousePosition)
        {
            float zOffset = -_mainCamera.transform.position.z;
            return _mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, zOffset));
        }

        public Vector3 MouseToInterpolatedCellPosition(Vector3 mousePosition)
        {
            Vector3 worldPos = MouseToWorldPosition(mousePosition);
            return WorldToInterpolatedCellPosition(worldPos);
        }

        public Vector3 WorldToInterpolatedCellPosition(Vector3 worldPosition)
        {
            Vector3 localPos = _grid.WorldToLocal(worldPosition);
            Vector3 localCellInterpolated = _grid.LocalToCellInterpolated(localPos);
            return localCellInterpolated;
        }
        
        public Vector3 CellToWorldPosition(Vector3Int cellPosition)
        {
            return _grid.CellToWorld(cellPosition);
        }

        public Vector3 CellInterpolatedToWorldPosition(Vector3 interpolatedCellPosition)
        {
            Vector3 localInterpolatedPos = _grid.CellToLocalInterpolated(interpolatedCellPosition);
            return _grid.LocalToWorld(localInterpolatedPos);
        }

        public bool TileIsOccupied(Vector2Int gridPos)
        {
            Vector3Int position = new Vector3Int(gridPos.x, gridPos.y, 0);
            return BuildingsDB.Instance.TileIsOccupied(gridPos) || MapManager.Instance.GetResourceSiteAt(position) != null;
        }

        public Vector3 GetRandomWalkablePosition(Vector3 origin, float maxRange)
        {
            while (true)
            {
                float angle = Random.Range(0, 360);
                float distance = Random.Range(0, maxRange);
                Vector3 position = origin + distance * (Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right);

                // if position is walkable => ToDo later
                return position;
            }
        }
    }
}
