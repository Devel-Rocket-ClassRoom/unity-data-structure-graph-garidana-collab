using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;


public enum Sdies
{
    // B R L T
    Top,  // 3
    Left,  // 2
    Right,  // 1
    Bottom  // 0
}

public class Tile
{
    public int id;
    public Tile[] adjacents = new Tile[4];
    public int autoTileId;

    // 전장의 안개 걷을지 말지 판단할 때 사용할 변수
    public bool isVisited = false;

    public void UpdateAutoTileId()
    {
        autoTileId = 0;
        for (int i = 0; i < adjacents.Length; ++i)
        {
            if (adjacents[i] != null)
            {
                // 1000  T
                // 0100  R
                // 0010  L
                // 0001  B
                autoTileId |= 1 << i;
            }
        }
    }

    public void RemoveAdjacents (Tile tile)
    {
        for (int i = 0; i < adjacents.Length; ++i)
        {
            if (adjacents[i] == null)
            {
                continue;
            }
            if (adjacents[i].id == tile.id)
            {
                adjacents[i] = null;
                UpdateAutoTileId();
                break;
            } 
        }
    }

    public void ClearAdjacents()
    {
        for ( int i = 0; i < adjacents.Length; ++i)
        {
            if (adjacents[i] == null)
                continue;
            adjacents[i].RemoveAdjacents(this);
            adjacents[i] = null;
        }
        UpdateAutoTileId();
    }
}