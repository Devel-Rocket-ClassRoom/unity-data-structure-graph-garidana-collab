using UnityEngine;
using UnityEngine.Rendering;

public class Graph1
{
    public int row = 0;
    public int col = 0;

    public GraphNode1[] nodes;

    public void Init (int[,] grid)
    {
        row = grid.GetLength(0);
        col = grid.GetLength(1);

        nodes = new GraphNode1[grid.Length];

        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i] = new GraphNode1();
            nodes[i].id = i;
        }

        for (int r = 0; r < row; ++r)
        {
            for (int c = 0; c < col; c++)
            {
                int index = r * col + c;
                nodes[index].weight = grid[r,c];

                if (grid[r,c] == -1)
                {
                    continue;
                }
                if (r - 1 >= 0 && grid[r - 1, c] >= 0)
                {
                    nodes[index].adjacents.Add(nodes[index - col]);
                }
                if (c + 1 < col && grid[r, c + 1] >= 0)
                {
                    nodes[index].adjacents.Add(nodes[index + 1]);
                }
                if (r + 1 < row && grid[r + 1, c] >= 0)
                {
                    nodes[index].adjacents.Add(nodes[index + col]);
                }
                if (c - 1 >= 0 && grid[r, c - 1] >= 0)
                {
                    nodes[index].adjacents.Add(nodes[index - 1]);
                }
            }
        }
    }

    public void ResetNodePrevious()
    {
        foreach (var node in nodes)
        {
            node.previous = null;
        }
    }
}