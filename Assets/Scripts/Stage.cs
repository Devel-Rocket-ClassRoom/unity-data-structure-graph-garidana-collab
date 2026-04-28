using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public GameObject tilePrefabs;
    private GameObject[] tileObjects;
    public int mapWidth = 20;
    public int mapHeight = 20;

    [Range(0f, 0.9f)]
    public float erodePercent = 0.5f;
    public int erodeIterations = 2;

    public Vector2 tileSize = new Vector2(16,16);
    public Sprite[] islandSprites;

    private Map _map;

    // Map 겟 프로퍼티
    public Map Map => _map;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ResetStage();
        }
    }


    private void ResetStage()
    {
        _map = new Map();
        _map.Init(mapHeight, mapWidth);
        _map.CreateIsland(erodePercent, erodeIterations);
        CreateGrid();
    }

    private void CreateGrid()
    {
        if (tileObjects != null)
        {
            foreach (var tile in tileObjects)
            {
                Destroy(tile.gameObject);
            }
        }

        tileObjects = new GameObject[mapWidth * mapHeight];

        var position = Vector3.zero;

        for (int i = 0; i < mapHeight; ++i)
        {
            for (int j = 0; j < mapWidth; ++j)
            {
                var tileId = i * mapWidth + j;
                var newGo = Instantiate(tilePrefabs, transform);
                newGo.transform.position = position;
                position.x += tileSize.x;

                tileObjects[tileId] = newGo;
                DecorateTile(tileId);
            }

            position.x = 0;
            position.y += tileSize.y;
        }
    }

    public void DecorateTile(int tileId)
    {
        var tile = _map.tiles[tileId];
        var tileGo = tileObjects[tileId];
        var ren = tileGo.GetComponent<SpriteRenderer>();
        
        if (tile.autoTileId != (int)TileTypes.Empty)
        {
            ren.sprite = islandSprites[tile.autoTileId];
        }
        else
        {
            ren.sprite = null;
        }
    }
}
