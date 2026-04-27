using UnityEngine;


public enum Sdies
{
    // B R L T
    Bottom,  // 3
    Right,  // 2
    Left,  // 1
    Top  // 0
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
                // 1000  0
                // 0100  1
                // 0010  2
                // 0001  3
                autoTileId |= 1 << adjacents.Length - 1 - i;
            }
        }
    }

}