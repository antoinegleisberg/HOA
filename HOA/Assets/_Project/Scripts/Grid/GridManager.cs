using UnityEngine;

namespace antoinegleisberg.HOA
{
    public class GridManager : MonoBehaviour
    {
        public static GridManager Instance { get; private set; }

        [field:SerializeField] public Grid Grid { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public bool TileIsOccupied(Vector2Int gridPos)
        {
            return BuildingsDB.Instance.TileIsOccupied(gridPos);
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
