using System.Collections.Generic;
using UnityEngine;
using antoinegleisberg.Types;

namespace antoinegleisberg.HOA.Core
{
    public static class BuildingsPlacer
    {
        public static List<Vector2Int> GetOccupiedTiles(Vector2 interpolatedCellPosition, Vector2Int size)
        {
            List<Vector2Int> occupiedTiles = new List<Vector2Int>();

            Pair<int, int> rangeX = GetOccupationRange(interpolatedCellPosition.x, size.x);
            Pair<int, int> rangeY = GetOccupationRange(interpolatedCellPosition.y, size.y);

            for (int x = rangeX.First; x < rangeX.Second; x++)
            {
                for (int y = rangeY.First; y < rangeY.Second; y++)
                {
                    occupiedTiles.Add(new Vector2Int(x, y));
                }
            }

            return occupiedTiles;
        }

        public static Vector2 GetBuildingCenter(Vector2 worldPosition, Vector2Int size)
        {
            float x = size.x % 2 == 0 ? Mathf.FloorToInt(worldPosition.x) : Mathf.FloorToInt(worldPosition.x) + 0.5f;
            float y = size.y % 2 == 0 ? Mathf.FloorToInt(worldPosition.y) : Mathf.FloorToInt(worldPosition.y) + 0.5f;
            return new Vector2(x, y);
        }

        public static Vector2 GetBuildingCenterWorldCoordinates(Vector2 interpolatedCellPosition, Vector2Int size)
        {
            Pair<int, int> rangeX = GetOccupationRange(interpolatedCellPosition.x, size.x);
            Pair<int, int> rangeY = GetOccupationRange(interpolatedCellPosition.y, size.y);
            int centerCellX = Mathf.FloorToInt((rangeX.First + rangeX.Second) / 2f);
            int centerCellY = Mathf.FloorToInt((rangeY.First + rangeY.Second) / 2f);
            
            Vector3 cellCoordinatesInWorldCoordinateSpace = GridManager.Instance.CellToWorldPosition(new Vector3Int(centerCellX, centerCellY));

            if (size.x % 2 == 1)
            {
                cellCoordinatesInWorldCoordinateSpace += new Vector3(0.25f, 0.125f);
            }

            if (size.y % 2 == 1)
            {
                cellCoordinatesInWorldCoordinateSpace += new Vector3(-0.25f, 0.125f);
            }
            
            return new Vector2(cellCoordinatesInWorldCoordinateSpace.x, cellCoordinatesInWorldCoordinateSpace.y);
        }

        public static Pair<Pair<int, int>, Pair<int, int>> GetOccupationRange(Vector2 interpolatedCellPosition, Vector2Int size)
        {
            Pair<int, int> xRange = GetOccupationRange(interpolatedCellPosition.x, size.x);
            Pair<int, int> yRange = GetOccupationRange(interpolatedCellPosition.y, size.y);
            return new Pair<Pair<int, int>, Pair<int, int>>(xRange, yRange);
        }

        private static Pair<int, int> GetOccupationRange(float interpolatedCellPosition, int size)
        {
            int min = Mathf.RoundToInt(interpolatedCellPosition - size / 2f);
            int max = Mathf.RoundToInt(interpolatedCellPosition + size / 2f);

            return new Pair<int, int>(min, max);
        }
    }
}
