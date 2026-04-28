using System.Collections.Generic;

public class GraphSearch 
{
    private Graph _graph;
    // 경로 담는곳
    public List<GraphNode> path = new();


    public void Init(Graph graph)
    {
        this._graph = graph;
    }

    // 순회
    public void DFS(GraphNode node) // 시작 노드 인자
    {
        path.Clear();
        
        var visited = new HashSet<GraphNode>();
        var stack = new Stack<GraphNode>();

        // 시작 노드 넣기
        stack.Push(node);
        visited.Add(node);
        
        while (stack.Count > 0)
        {
            // 마지막에 stack에 들어온 놈을 get
            var currentNode = stack.Pop();
            path.Add(currentNode);

            // 방문할 노드가 있을시 stack에 추가
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

    // 
    public void BFS (GraphNode node)
    {
        path.Clear();

        // 방문한 노드
        var visited = new HashSet<GraphNode>();
        // 앞으로 방문할 노드들 (큐에서 뺄 때 해당 노드 방문)
        var queue = new Queue<GraphNode>();

        queue.Enqueue(node);
        visited.Add(node);

        while (queue.Count > 0)
        {
            var currentNode = queue.Dequeue();
            path.Add(currentNode);
            
            foreach(var adjacent in currentNode.adjacents)
            {
                if (!adjacent.CanVisit || visited.Contains(adjacent))
                {
                    continue;
                }

                visited.Add(adjacent);
                queue.Enqueue(adjacent);
            }
        }
    }

    public void RecursiveDFS(GraphNode startNode)
    {
        path.Clear();

        var visited = new HashSet<GraphNode>();
        RecursiveDFSCore(startNode, visited);

    }

    private void RecursiveDFSCore(GraphNode node, HashSet<GraphNode> visited)
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

    public bool PathFindingBFS(GraphNode startNode, GraphNode endNode)
    {
        // 경로 초기화
        path.Clear();
        // previous 노드 초기화
        _graph.ResetNodePrevious();

        var visited = new HashSet<GraphNode>();
        var queue = new Queue<GraphNode>();

        queue.Enqueue(startNode);
        visited.Add(startNode);

        bool success = false;

        // 큐의 길이가 존재하면 
        while (queue.Count > 0)
        {
            var currentNode = queue.Dequeue();
            // 목적지 도달시 break
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
                queue.Enqueue(adjacent);
            }
        }

        if (!success)
        {
            return false;
        }

        GraphNode step = endNode;

        while (step != null)
        {
            path.Add(step);
            step = step.previous;
        }

        path.Reverse();
        return true;
        
        // // endNode 에서 startNode 역추적
        // var node = endNode;
        // while (node != null)
        // {
        //     path.Add(node);
        //     node = node.previous;
        // }

        // // startNode에서 endNode로 역추적
        // path.Reverse();
    }

    public bool Dijkstra(GraphNode startNode, GraphNode endNode)
    {
        // 빈 경로 초기화
        path.Clear();
        _graph.ResetNodePrevious();

        // 방문한 노드 추적을 위한 hashset 추가
        var visited = new HashSet<GraphNode>();
        // 우선순위 큐 생성 (노드 <T>와 우선순위<int>를 담고있는 큐)
        var priorityQueue = new PriorityQueue<GraphNode, int>();
        var distances = new int[_graph.nodes.Length];

        for (int i = 0; i < distances.Length; ++i)
        {
            distances[i] = int.MaxValue;
        }

        distances[startNode.id] = 0;
        priorityQueue.Enqueue(startNode, 0);
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

            if (currentNode == endNode)
            {
                success = true;
                break;
            }

            visited.Add(currentNode);

            foreach (var adjacent in currentNode.adjacents)
            {
                if (!adjacent.CanVisit || visited.Contains(adjacent))
                {
                    continue;
                }

                var newDistance = distances[currentNode.id] + adjacent.weight;
                if (newDistance < distances[adjacent.id])
                {
                    distances[adjacent.id] = newDistance;
                    priorityQueue.Enqueue(adjacent, distances[adjacent.id]);
                    adjacent.previous = currentNode;
                }
                // visited.Add(adjacent);
            }
        }

        if (!success)
        {   
            return false;
        }

        GraphNode step = endNode;
        while (step != null)
        {
            path.Add(step);
            step = step.previous;
        }
        path.Reverse();
        return true;
     
    }

    public bool AStar(GraphNode startNode, GraphNode endNode)
    {
         // 빈 경로 초기화
        path.Clear();
        _graph.ResetNodePrevious();

        // 방문한 노드 추적을 위한 hashset 추가
        var visited = new HashSet<GraphNode>();
        // 우선순위 큐 생성 (노드 <T>와 우선순위<int>를 담고있는 큐)
        var priorityQueue = new PriorityQueue<GraphNode, int>();
        var distances = new int[_graph.nodes.Length];

        for (int i = 0; i < distances.Length; ++i)
        {
            distances[i] = int.MaxValue;
        }

        distances[startNode.id] = 0;
        priorityQueue.Enqueue(startNode, 0);
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

            if (currentNode == endNode)
            {
                success = true;
                break;
            }

            visited.Add(currentNode);

            foreach (var adjacent in currentNode.adjacents)
            {
                if (!adjacent.CanVisit || visited.Contains(adjacent))
                {
                    continue;
                }

                var newDistance = distances[currentNode.id] + adjacent.weight;
                if (newDistance < distances[adjacent.id])
                {
                    distances[adjacent.id] = newDistance;
                    priorityQueue.Enqueue(adjacent, distances[adjacent.id]);
                    adjacent.previous = currentNode;
                }
                // visited.Add(adjacent);
            }
        }

        if (!success)
        {   
            return false;
        }
        
        GraphNode step = endNode;
        while (step != null)
        {
            path.Add(step);
            step = step.previous;
        }
        path.Reverse();
        return true;
    }
}
