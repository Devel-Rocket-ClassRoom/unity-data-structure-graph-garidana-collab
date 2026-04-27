using System.Collections.Generic;
using UnityEngine;

<<<<<<< HEAD
public class GraphNode // 그래프의 기본단위 : 노드
{
    // 하나의 칸 (노드)를 담는 클래스

    // 노드 id (배열 인덱스 개념)
    public int id;
    // 노드 해당칸의 가중치 (-1 이면 이동 불가)
    public int weight;
    // 길을 찾을 때 사용할 용도 (이전 노드) 역 추적용
=======
public class GraphNode 
{
    public int id;
    // 노드 해당칸의 가중치 (-1 이면 이동 불가)
    public int weight;
    // 길을 찾을 때 사용할 용도 (이전 노드)
>>>>>>> 6547247e2ee2336baffba2de6d00bbb748536d52
    public GraphNode previous = null;
    // 이웃 노드 리스트
    public List<GraphNode> adjacents = new();
    // weight == -1 이면 벽(이동불가)
    public bool CanVisit => adjacents.Count > 0 && weight >= 0;
}
