using System.Collections.Generic;
using UnityEngine;

public class GraphTest : MonoBehaviour
<<<<<<< HEAD
// 씬을 초기화하고 탐색을 실행 해보는 Test 코드
=======
>>>>>>> 6547247e2ee2336baffba2de6d00bbb748536d52
{
    public enum Algorithm
    {
        // 깊이 우선 탐색 (스택형 자료탐색)
        DFS,
        // 너비 우선 탐색 (큐형 자료탐색)
        BFS,
        // 재귀함수 버전 DFS
        RecursiveDFS,
        // BFS 길찾기 버전
        PathFindingBFS,
        // Dijkstra 알고리즘
        Dijkstra,
        // A* 알고리즘
        AStar
    }

    public Transform uiNodeRoot;
    public Algorithm algorithm;
    public int startId;
    public int endId;

    public UIGraphNode nodePrefab;
    private List<UIGraphNode> uiNodes = new();
    private Graph graph;

    

    private void Start()
    {
<<<<<<< HEAD
        // 맵 데이터 (하드코딩으로 테스트)
        int[,] map = new int[5, 5]
        {
            { 1,-1,10,1,1},
=======
        int[,] map = new int[5, 5]
        {
            { 1,-1,100,1,1},
>>>>>>> 6547247e2ee2336baffba2de6d00bbb748536d52
            { 1,1,1,-1,1},
            { 1,1,-1,1,1},
            { 1,-1,1,1,1},
            { 1,1,1,1,1},
        };

        graph = new Graph();
        graph.Init(map);
        InitUiNodes(graph);
    }

    private void InitUiNodes(Graph graph)
    {
        foreach (var node in graph.nodes)
        {
            var uiNode = Instantiate(nodePrefab, uiNodeRoot);
            uiNode.SetNode(node);
            uiNode.Reset();
            uiNodes.Add(uiNode);
        }
    }

    private void ResetUiNodes()
    {
        foreach (var uiNode in uiNodes)
        {
            uiNode.Reset();
        }
    }

<<<<<<< HEAD

    // [ContextMenu]는 Unity 에디터에서 우클릭으로 Search()함수를 실행하게 해줌
=======
>>>>>>> 6547247e2ee2336baffba2de6d00bbb748536d52
    [ContextMenu("Search")]
    public void Search()
    {
        var search = new GraphSearch();
        search.Init(graph);
    
        switch (algorithm)
        {
            case Algorithm.DFS:
                search.DFS(graph.nodes[startId]);
                break;

            case Algorithm.BFS:
                search.BFS(graph.nodes[startId]);
                break;
            case Algorithm.RecursiveDFS:
                search.RecursiveDFS(graph.nodes[startId]);
                break;
            case Algorithm.PathFindingBFS:
                search.PathFindingBFS(graph.nodes[startId], graph.nodes[endId]);
                break;
            case Algorithm.Dijkstra:
                search.Dijkstra(graph.nodes[startId], graph.nodes[endId]);
                break;
            case Algorithm.AStar:
                search.AStar(graph.nodes[startId], graph.nodes[endId]);
                break;  
        }

        ResetUiNodes();

        if (search.path.Count <= 1)
        {
            if (search.path.Count == 1)
            {
                var only = search.path[0];
                uiNodes[only.id].SetColor(Color.red);
            }   
            return;   
        }

        for (int i = 0; i < search.path.Count; ++i)
        {
            var node = search.path[i];
            var color = Color.Lerp(Color.green, Color.gold, (float)i / (search.path.Count - 1));
            uiNodes[node.id].SetColor(color);
            uiNodes[node.id].SetText($"ID: {node.id}\nWeight: {node.weight}\nPath: {i}");  
        }
    }

}
