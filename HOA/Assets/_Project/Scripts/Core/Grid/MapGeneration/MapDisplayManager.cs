using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

using Random = UnityEngine.Random;

namespace antoinegleisberg.HOA.Core
{
    /// <summary>
    /// Handles the display of the tiles of the map
    /// </summary>
    public class MapDisplayManager : MonoBehaviour
    {
        private static Dictionary<Tuple<Tile, Tile, Color, Color>, Tile> _mergedTiles;
        
        [SerializeField] private Tile _leftTip;
        [SerializeField] private Tile _leftCorner;
        [SerializeField] private Tile _rightTip;
        [SerializeField] private Tile _rightCorner;
        [SerializeField] private Tile _topTip;
        [SerializeField] private Tile _topCorner;
        [SerializeField] private Tile _bottomTip;
        [SerializeField] private Tile _bottomCorner;
        [SerializeField] private Tile _bottomLeftSide;
        [SerializeField] private Tile _bottomRightSide;
        [SerializeField] private Tile _topLeftSide;
        [SerializeField] private Tile _topRightSide;

        [SerializeField] private TileData _groundTile;
        [SerializeField] private TileData _waterTile;

        [SerializeField] private Tilemap _tilemap;

        private void Awake()
        {
            _mergedTiles = new Dictionary<Tuple<Tile, Tile, Color, Color>, Tile>();
            _mergedTiles.Clear();
        }

        public void ResetDisplay()
        {
            _tilemap.ClearAllTiles();
        }

        public void UpdateDisplay(List<Vector3Int> positions)
        {
            foreach (Vector3Int position in positions)
            {
                UpdateWaterTileDisplay(position);
            }
        }

        public void UpdateTileDisplay(Vector3Int position, TileData tileData)
        {
            // replace this with the correct tile later
            TileBase selectedTileSprite = tileData.TileList[Random.Range(0, tileData.TileList.Count)];
            _tilemap.SetTile(position, selectedTileSprite);

            // Set the correct color
            Color tileColor = tileData.TileColor;
            _tilemap.SetTileFlags(position, TileFlags.None);
            _tilemap.SetColor(position, tileColor);
            _tilemap.SetTileFlags(position, TileFlags.LockColor);
        }

        private void UpdateWaterTileDisplay(Vector3Int position)
        {
            if (MapManager.Instance.GetTileAt(position) != _waterTile)
            {
                return;
            }

            bool waterRight = MapManager.Instance.GetTileAt(position + Vector3Int.right) == _waterTile;
            bool waterLeft = MapManager.Instance.GetTileAt(position + Vector3Int.left) == _waterTile;
            bool waterUp = MapManager.Instance.GetTileAt(position + Vector3Int.up) == _waterTile;
            bool waterDown = MapManager.Instance.GetTileAt(position + Vector3Int.down) == _waterTile;
            bool waterUpRight = MapManager.Instance.GetTileAt(position + Vector3Int.right + Vector3Int.up) == _waterTile;
            bool waterUpLeft = MapManager.Instance.GetTileAt(position + Vector3Int.left + Vector3Int.up) == _waterTile;
            bool waterDownRight = MapManager.Instance.GetTileAt(position + Vector3Int.right + Vector3Int.down) == _waterTile;
            bool waterDownLeft = MapManager.Instance.GetTileAt(position + Vector3Int.left + Vector3Int.down) == _waterTile;

            Tile tile = (Tile)_waterTile.TileList[0];
            if (!waterRight && !waterUp)
            {
                tile = GetMergedTile(_topCorner, _bottomTip, _groundTile.TileColor, _waterTile.TileColor);
            }
            else if (!waterRight && !waterDown)
            {
                tile = GetMergedTile(_rightCorner, _leftTip, _groundTile.TileColor, _waterTile.TileColor);
            }
            else if (!waterLeft && !waterUp)
            {
                tile = GetMergedTile(_leftCorner, _rightTip, _groundTile.TileColor, _waterTile.TileColor);
            }
            else if (!waterLeft && !waterDown)
            {
                tile = GetMergedTile(_bottomCorner, _topTip, _groundTile.TileColor, _waterTile.TileColor);
            }
            else if (!waterUp)
            {
                tile = GetMergedTile(_topLeftSide, _bottomRightSide, _groundTile.TileColor, _waterTile.TileColor);
            }
            else if (!waterDown)
            {
                tile = GetMergedTile(_bottomRightSide, _topLeftSide, _groundTile.TileColor, _waterTile.TileColor);
            }
            else if (!waterRight)
            {
                tile = GetMergedTile(_topRightSide, _bottomLeftSide, _groundTile.TileColor, _waterTile.TileColor);
            }
            else if (!waterLeft)
            {
                tile = GetMergedTile(_bottomLeftSide, _topRightSide, _groundTile.TileColor, _waterTile.TileColor);
            }
            else if (!waterDownLeft)
            {
                tile = GetMergedTile(_bottomTip, _topCorner, _groundTile.TileColor, _waterTile.TileColor);
            }
            else if (!waterDownRight)
            {
                tile = GetMergedTile(_rightTip, _leftCorner, _groundTile.TileColor, _waterTile.TileColor);
            }
            else if (!waterUpLeft)
            {
                tile = GetMergedTile(_leftTip, _rightCorner, _groundTile.TileColor, _waterTile.TileColor);
            }
            else if (!waterUpRight)
            {
                tile = GetMergedTile(_topTip, _bottomCorner, _groundTile.TileColor, _waterTile.TileColor);
            }

            _tilemap.SetTile(position, tile);
        }

        private static Tile GetMergedTile(Tile tile1, Tile tile2, Color color1, Color color2)
        {
            if (_mergedTiles == null)
            {
                _mergedTiles = new Dictionary<Tuple<Tile, Tile, Color, Color>, Tile>();
            }

            Tuple<Tile, Tile, Color, Color> key = new Tuple<Tile, Tile, Color, Color>(tile1, tile2, color1, color2);
            if (!_mergedTiles.ContainsKey(key))
            {
                _mergedTiles[key] = MergeTiles(tile1, tile2, color1, color2);
            }
            return _mergedTiles[key];
        }

        private static Tile MergeTiles(Tile tile1, Tile tile2, Color color1, Color color2)
        {
            Vector2Int dimensions = new Vector2Int((int)tile1.sprite.rect.width, (int)tile1.sprite.rect.height);

            Texture2D texture1 = tile1.sprite.texture;
            Texture2D texture2 = tile2.sprite.texture;

            Texture2D mergedTexture = new Texture2D(dimensions.x, dimensions.y, TextureFormat.RGBA32, false);

            for (int x = 0; x < dimensions.x; x++)
            {
                for (int y = 0; y < dimensions.y; y++)
                {
                    float alpha1 = texture1.GetPixel((int)tile1.sprite.rect.x + x, (int)tile1.sprite.rect.y + y).a;
                    float alpha2 = texture2.GetPixel((int)tile2.sprite.rect.x + x, (int)tile2.sprite.rect.y + y).a;
                    Color mergedColor = color1 * alpha1 + color2 * alpha2;
                    if (Mathf.Approximately(alpha1 + alpha2, 2))
                    {
                        mergedColor /= 2;
                    }
                    mergedTexture.SetPixel(x, y, mergedColor);
                }
            }

            mergedTexture.filterMode = FilterMode.Point;

            mergedTexture.Apply();

            Tile tile = ScriptableObject.CreateInstance<Tile>();
            tile.sprite = Sprite.Create(mergedTexture, new Rect(0, 0, mergedTexture.width, mergedTexture.height), new Vector2(0.5f, 0.5f), 256);
            return tile;
        }
    }
}
