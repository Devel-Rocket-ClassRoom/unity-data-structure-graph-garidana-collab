using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Collections;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public GameObject tilePrefabs;
    private GameObject[] tileObjects;
    public int mapWidth = 20;
    public int mapHeight = 20;

    private Camera mainCamera;

    [Range(0f, 0.9f)]
    public float erodePercent = 0.5f;
    public int erodeIterations = 2;
    public float lakePercent = 0.1f;
    public float treePercent = 0.1f;
    public float hillPercent = 0.1f;
    public float mountainPercent = 0.1f;
    public float townPercent = 0.1f;
    public float monsterPercent = 0.1f;
    public float castlePercent = 0.1f;

    public Vector2 tileSize = new Vector2(16,16);
    public Sprite[] islandSprites;
    //public Sprite[] fowSprites;
    //public Color fowColor = Color.black;

    //private GameObject[] fowObjects;

    private Map _map;

    public PlayerMovement playerPrefab;

    private PlayerMovement player;

    // Map 겟 프로퍼티
    public Map Map => _map;

    private Vector3 FirstTilePos
    {
        get
        {
            var pos = transform.position;
            pos.x -= mapWidth * tileSize.x * 0.5f;
            pos.y += mapHeight * tileSize.y * 0.5f;

            pos.x += tileSize.x * 0.5f;
            pos.y -= tileSize.y * 0.5f;
            return pos;
        }
    }

    private int prevTileId = -1;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ResetStage();
        }

        if (tileObjects != null)
        {
            int currentTileId = ScreenPosToTileId(Input.mousePosition);
            if (prevTileId != currentTileId)
            {
                tileObjects[currentTileId].GetComponent<SpriteRenderer>().color = Color.green;
                if (prevTileId >= 0 && prevTileId < tileObjects.Length)
                {
                    tileObjects[prevTileId].GetComponent<SpriteRenderer>().color = Color.white;
                }
                prevTileId = currentTileId;
            }
        }
    }


    private void ResetStage()
    {
        _map = new Map();
        _map.Init(mapHeight, mapWidth);
        _map.CreateIsland(
            erodePercent, 
            erodeIterations,
            lakePercent,
            treePercent,
            hillPercent,
            mountainPercent,
            townPercent,
            monsterPercent
            );
        CreateGrid();
        CreatePlayer();
    }


    private void CreatePlayer()
    {
        if (player != null)
        {
            Destroy (player.gameObject);
        }

        player = Instantiate(playerPrefab);
        player.MoveTo(Map.startTile.id);
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

        var position = FirstTilePos;

        // FOW 생성
        //fowObjects = new GameObject[mapWidth * mapHeight];

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

            position.x = FirstTilePos.x;
            position.y -= tileSize.y;
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

    public int ScreenPosToTileId(Vector3 screenPos)
    {
        screenPos.z = Mathf.Abs(transform.position.z - mainCamera.transform.position.z);
        return WorldPosToTileId(mainCamera.ScreenToWorldPoint(screenPos));
        //return (int)(screenPos.y / tileSize.y) * mapWidth + (int)(screenPos.x / tileSize.x);
    }

    public int WorldPosToTileId (Vector3 worldPos)
    {
        var first = FirstTilePos;
        int x = Mathf.FloorToInt((worldPos.x - first.x) / tileSize.x + 0.5f);
        int y = Mathf.FloorToInt((first.y - worldPos.y) / tileSize.y + 0.5f);
        x = Mathf.Clamp(x, 0, mapWidth - 1);
        y = Mathf.Clamp(y, 0, mapHeight - 1);
        return y * mapWidth + x;
    }

    public Vector3 GetTilePos(int y , int x)
    {
        return FirstTilePos + new Vector3 (x * tileSize.x, - y * tileSize.y);
    }

    public Vector3 GetTilePos (int tileId)
    {
        return GetTilePos(tileId / mapWidth, tileId % mapWidth);
    }

}
