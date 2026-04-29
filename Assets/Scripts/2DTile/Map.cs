using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public enum TileTypes
{
    Empty = -1,
    // 0~14
    Grass = 15,
    Tree,
    Hills,
    Mountains,
    Towns,
    Castle,
    Monster,
}

// 맵 생성및 관리 클래스
public class Map
{
    public int rows = 0;
    public int cols = 0;

    public Tile[] tiles;

    public Tile[] CoastTiles => tiles.Where(t => t.autoTileId >= 0 && t.autoTileId < (int)TileTypes.Grass).ToArray();
    public Tile[] LandTiles => tiles.Where(t => t.autoTileId == (int)TileTypes.Grass).ToArray();

    public Tile startTile;
    public Tile castleTile;

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
                var index = r * cols + c;
                var adjacents = tiles[index].adjacents;
                if ((r - 1) >= 0)
                {
                    adjacents[(int)Sides.Top] = tiles[index - cols];
                }
                if ((c + 1) < cols)
                {
                    adjacents[(int)Sides.Right] = tiles[index + 1];
                }
                if ((c - 1) >= 0)
                {
                    adjacents[(int)Sides.Left] = tiles[index - 1];
                }
                if ((r + 1) < rows)
                {
                    adjacents[(int)Sides.Bottom] = tiles[index + cols];
                }
            }
        }

        for (int i = 0; i < tiles.Length; ++i)
        {
            tiles[i].UpdateAutoTileId();
            tiles[i].UpdateFowTileId();
        }
    }

    public void ShuffleTiles(Tile[] tiles)
    {
        for (int i = tiles.Length - 1; i > 0; --i)
        {
            int rand = Random.Range(0, i + 1);
            (tiles[rand], tiles[i]) = (tiles[i], tiles[rand]);
        }
    }

    public void DecorateTiles(Tile[] tiles, float percent, TileTypes tileType)
    {
        ShuffleTiles(tiles);
        int total = Mathf.FloorToInt(tiles.Length * percent); 
        for (int i = 0; i < total; ++i)
        {
            if (tileType == TileTypes.Empty)
            {
                tiles[i].ClearAdjacents();
            }
            tiles[i].autoTileId = (int)tileType;
        }
    }

    public bool CreateIsland(
        float erodePercent,
        int erodeIterations,
        float lakePercent,
        float treePercent,
        float hillPercent,
        float mountainPercent,
        float townPercent,
        float monsterPercent)
    {
        DecorateTiles(LandTiles, lakePercent, TileTypes.Empty);

        for (int i = 0; i < erodeIterations; ++i)
        {
            DecorateTiles(CoastTiles, erodePercent, TileTypes.Empty);
        }

        DecorateTiles(LandTiles, treePercent, TileTypes.Tree);
        DecorateTiles(LandTiles, hillPercent, TileTypes.Hills);
        DecorateTiles(LandTiles, mountainPercent, TileTypes.Mountains);
        DecorateTiles(LandTiles, townPercent, TileTypes.Towns);
        DecorateTiles(LandTiles, monsterPercent, TileTypes.Monster);

        var towns = tiles.Where(x => x.autoTileId == (int)TileTypes.Towns).ToArray();
        ShuffleTiles(towns);
        startTile = towns[0]; 
        castleTile = towns[1];
        castleTile.autoTileId = (int)TileTypes.Castle;

        var path = PathFindingAStar(startTile, castleTile);
        return path.Count > 0;

    }

    private int Heuristic(Tile a, Tile b)
    {
        int ax = a.id % cols;
        int ay = a.id / cols;

        int bx = b.id % cols;
        int by = b.id / cols;

        return Mathf.Abs(ax - bx) + Mathf.Abs(ay - by);
    }

    public List<Tile> PathFindingAStar (int startTile, int goalTile)
    {
        return PathFindingAStar(tiles[startTile], tiles[goalTile]);
    }

    public List<Tile> PathFindingAStar(Tile startTile, Tile goalTile)
    {
        List<Tile> path = new();
         // 빈 경로 초기화
        path.Clear();
        foreach (Tile tile in tiles)
        {
            tile.ClearPreviousTile();
        }

        // 방문한 노드 추적을 위한 hashset 추가
        var visited = new HashSet<Tile>();
        // 우선순위 큐 생성 (노드 <T>와 우선순위<int>를 담고있는 큐)
        var priorityQueue = new PriorityQueue<Tile, int>();
        var distances = new int[tiles.Length];

        for (int i = 0; i < distances.Length; ++i)
        {
            distances[i] = int.MaxValue;
        }

        distances[startTile.id] = 0;
        priorityQueue.Enqueue(startTile, distances[startTile.id] + Heuristic(startTile, goalTile));
        // visited.Add(startNode);

        bool success = false;
        // 우선순위의 큐가 빌때까지 반복
        while (priorityQueue.Count > 0)
        {
            var currentNode = priorityQueue.Dequeue();
            if (visited.Contains(currentNode))
            {
                continue;
            }

            if (currentNode == goalTile)
            {
                success = true;
                break;
            }

            visited.Add(currentNode);

            foreach (var adjacent in currentNode.adjacents)
            {
                if ( adjacent == null || !adjacent.CanMove || visited.Contains(adjacent))
                {
                    continue;
                }

                var newDistance = distances[currentNode.id] + adjacent.Weight;
                if (newDistance < distances[adjacent.id])
                {
                    distances[adjacent.id] = newDistance;
                    priorityQueue.Enqueue(adjacent, distances[adjacent.id] + Heuristic(adjacent, goalTile));
                    adjacent.previousTile = currentNode;
                }
                // visited.Add(adjacent);
            }
        }

        if (success)
        {
            Tile step = goalTile;
            while (step != null)
            {
                path.Add(step);
                step = step.previousTile;
            }
            path.Reverse();
        }
        return path;
    }
}