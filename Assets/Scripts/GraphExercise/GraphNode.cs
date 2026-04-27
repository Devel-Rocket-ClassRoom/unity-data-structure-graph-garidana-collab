using System.Collections.Generic;
using UnityEngine;

public class GraphNode 
{
    public int id;
    // 노드 해당칸의 가중치 (-1 이면 이동 불가)
    public int weight;
    // 길을 찾을 때 사용할 용도 (이전 노드)
    public GraphNode previous = null;
    // 이웃 노드 리스트
    public List<GraphNode> adjacents = new();
    // weight == -1 이면 벽(이동불가)
    public bool CanVisit => adjacents.Count > 0 && weight >= 0;
}
