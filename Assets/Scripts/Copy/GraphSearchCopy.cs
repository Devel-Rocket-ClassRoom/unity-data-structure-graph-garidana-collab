using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;


public class GraphSearch1
{
    private Graph1 _graph;
    public List<GraphNode1> path = new ();

    public void Init (Graph1 graph)
    {
        this._graph = graph;
    }

    public void DFS (GraphNode1 node1)
    {
        path.Clear();

        var visited = new HashSet<GraphNode1>();
        var stack = new Stack<GraphNode1>();

        stack.Push(node1);
        visited.Add(node1);

        while (stack.Count > 0)
        {
            var currentNode = stack.Pop();
            path.Add(currentNode);

            foreach (var adjacent in currentNode.adjacents)
            {
                if (!adjacent.CanVisit || visited.Contains(adjacent))
                {
                    continue;
                }

                visited.Add(adjacent);
                stack.Push(adjacent);
            }
        }
    }

    public void BFS (GraphNode1 node1)
    {
        path.Clear();

        var visited = new HashSet<GraphNode1>();
        var q= new Queue<GraphNode1>();

        q.Enqueue(node1);
        visited.Add(node1);

        while (q.Count > 0)
        {
            var currentNode = q.Dequeue();
            path.Add(currentNode);

            foreach (var adjacent in currentNode.adjacents)
            {
                if (!adjacent.CanVisit || visited.Contains(adjacent))
                {
                    continue;
                }

                visited.Add(adjacent);
                q.Enqueue(adjacent);
            }
        }       
    }

    public void RecursiveDFS (GraphNode1 startNode)
    {
        path.Clear();

        var visited = new HashSet<GraphNode1>();
        RecursiveDFSCore(startNode, visited);
    }

    private void RecursiveDFSCore (GraphNode1 node, HashSet<GraphNode1> visited)
    {
        path.Add(node);
        visited.Add(node);

        foreach (var adjacent in node.adjacents)
        {
            if (!adjacent.CanVisit || visited.Contains(adjacent))
            {
                continue;
            }

            RecursiveDFSCore(adjacent, visited);
        }
    }

    public bool PathFindingBFS (GraphNode1 startNode, GraphNode1 endNode)
    {
        path.Clear();
        _graph.ResetNodePrevious();

        var visited = new HashSet<GraphNode1>();
        var q = new Queue<GraphNode1>();

        q.Enqueue(startNode);
        visited.Add(startNode);

        bool success = false;

        while (q.Count > 0)
        {
            var currentNode = q.Dequeue();

            if (currentNode == endNode)
            {
                success = true;
                break;
            }

            foreach (var adjacent in currentNode.adjacents)
            {
                if (!adjacent.CanVisit || visited.Contains(adjacent))
                {
                    continue;
                }

                adjacent.previous = currentNode;
                visited.Add(adjacent);
                q.Enqueue(adjacent);
            }
        }

        if (!success)
        {
            return false;
        }

        GraphNode1 step = endNode;

        while (step != null)
        {
            path.Add(step);
            step = step.previous;
        }

        path.Reverse();
        return true;
    }
}