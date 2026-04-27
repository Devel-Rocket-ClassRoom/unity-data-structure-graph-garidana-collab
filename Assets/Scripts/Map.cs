using System;
using UnityEngine;


public enum TileType
{
    Empty = -1,
    // 0 ~ 14 해안선 타일
    Grass = 15,
    Tree,
    Hills,
    Mountains,
    Towns,
    Castle,
    Monster
}
public class Map 
{
    public int rows = 0;
    public int cols = 0;

    public Tile[] tiles;

    public void Init(int rows, int cols)
    {
        this.rows = rows;
        this.cols = cols;

        tiles = new Tile[rows * cols];
        for (int i = 0; i < tiles.Length; ++i)
        {
            tiles[i] = new Tile();
            tiles[i].id = i;
        }

        for (int r = 0; r < rows; ++r)
        {
            for (int c = 0; c < cols; ++c)
            {
                int index = r * cols + c;
                var adjacents = tiles[index].adjacents;
                if ((r - 1) >= 0)
                {
                    adjacents[(int)Sdies.Top] = tiles[index - cols];
                }

                if ((c + 1) < cols)
                {
                    adjacents[(int)Sdies.Right] = tiles[index + 1];
                }

                if ((c - 1) >= 0)
                {
                    adjacents[(int)Sdies.Bottom] = tiles[index - 1];
                }

                if ((r + 1) < rows)
                {
                    adjacents[(int)Sdies.Left] = tiles[index + cols];
                }
            }
        }

        for (int i = 0; i < tiles.Length; ++i)
        {
            tiles[i].UpdateAutoTileId();
        }
    }
}
