using UnityEngine;
using UnityEngine.Tilemaps;

namespace antoinegleisberg.HOA.Core
{
    public class MapDisplayManager : MonoBehaviour
    {
        public static MapDisplayManager Instance { get; private set; }

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

        [SerializeField] private Tile _tile1;
        [SerializeField] private Tile _tile2;

        [SerializeField] private TileData _groundTile;
        [SerializeField] private TileData _waterTile;

        [SerializeField] private Tilemap _tilemap;

        private void Awake()
        {
            Instance = this;
        }
        
        private void Start()
        {
            Tile tile = MergeTiles(_tile1, _tile2, _groundTile.TileColor, _waterTile.TileColor);
            _tilemap.SetTile(new Vector3Int(-2, -2, 0), tile);
        }

        public void UpdateWaterTilesDisplay(Vector3Int position)
        {
            // throw new NotImplementedException();
        }

        private static Tile MergeTiles(Tile tile1, Tile tile2, Color color1, Color color2)
        {
            Vector2Int dimensions = new Vector2Int((int)tile1.sprite.rect.width, (int)tile1.sprite.rect.height);

            Texture2D texture1 = tile1.sprite.texture;
            Texture2D texture2 = tile2.sprite.texture;

            Texture2D mergedTexture = new Texture2D(dimensions.x, dimensions.y);

            for (int x = 0; x < dimensions.x; x++)
            {
                for (int y = 0; y < dimensions.y; y++)
                {
                    float alpha1 = texture1.GetPixel((int)tile1.sprite.rect.x + x, (int)tile1.sprite.rect.y + y).a;
                    float alpha2 = texture2.GetPixel((int)tile2.sprite.rect.x + x, (int)tile2.sprite.rect.y + y).a;
                    Color mergedColor = color1 * alpha1 + color2 * alpha2;
                    mergedTexture.SetPixel(x, y, mergedColor);
                }
            }

            mergedTexture.Apply();

            Tile tile = ScriptableObject.CreateInstance<Tile>();
            tile.sprite = Sprite.Create(mergedTexture, new Rect(0, 0, mergedTexture.width, mergedTexture.height), new Vector2(0.5f, 0.5f), 256);
            return tile;
        }
    }
}
