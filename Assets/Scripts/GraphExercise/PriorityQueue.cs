using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PriorityQueue<TElement, TPriority>
{
    // TElement와 TPriority 타입을 튜플로 다루는 리스트
    private List<(TElement element, TPriority priority)> _heap;
    // TPriority 타입을 비교하는 Comparer
    private Comparer<TPriority> _comparer;

    // _heap 리스트의 길이를 구할 get 프로퍼티
    public int Count
    {
        get { return _heap.Count; }
    }

    // 리스트와 Comparer (Comparer<TPriority>.Default 캐싱 변수) 초기화 시켜줄 생성자
    public PriorityQueue()
    {
        // 원소와 우선순위 튜플을 갖는 리스트 생성
        _heap = new List<(TElement element, TPriority priority)>();
        // Comparer 캐싱
        _comparer = Comparer<TPriority>.Default;
    }

    public void Enqueue(TElement element, TPriority priority)
    {
        // _heap 리스트에 아이템 추가
        // 새 요소를 리스트 맨 끝에 먼저 추가
        _heap.Add((element, priority));

        // 인덱스는 길이의 -1 // 방금 추가한 원소의 인덱스 : 0 => based라 Count - 1을 해줘야 해당하는 원소의 인덱스가 됨. 인덱스는 0에서부터 시작하기 때문
        int index = _heap.Count - 1;

        // 인덱스 0 = 루트 (더 올라갈 노드가 없음)
        while (index > 0)
        {
            // 현재 인덱스의 부모 인덱스 계산 식 (parentIndex)
            int parentIndex = (index - 1) / 2;
            // Comparer 사용해서 우선순위 (priority)를 비교
            // Comparer => 왼쪽 인자가 클경우 음수, 두 인자가 같은 경우 0, 오른쪽 인자가 클 경우 양수 반환
            if (_comparer.Compare(_heap[index].priority, _heap[parentIndex].priority) < 0)
            {
                (_heap[index], _heap[parentIndex]) = (_heap[parentIndex], _heap[index]);
                index = parentIndex;
            }
            else
            {
                break;
            }
        }
    }

    public TElement Dequeue()
    {

        TElement result = _heap[0].element;

        // 인덱스로 사용할 변수 마지막 인덱스
        int last = _heap.Count - 1;
        _heap[0] = _heap[last];
        _heap.RemoveAt(last);

        // 루트 노드의 인덱스
        int index = 0;
        while (true)
        {
            // 자식 인덱스 Left and Right
            int left = 2 * index + 1;
            int right = 2 * index + 2;
            // 제일 작은 인덱스를 넣어줄 min 변수
            int min = index;

            // 왼쪽 노드와 현재 노드의 크기 비교
            if (left < _heap.Count && _comparer.Compare(_heap[left].priority, _heap[min].priority) < 0)
            {
                min = left;
            }
            // 오른쪽 노드와 현재 노드의 크기 비교
            if (right < _heap.Count && _comparer.Compare(_heap[right].priority, _heap[min].priority) < 0)
            {
                min = right;
            }
            // 
            if (min == index)
            {
                break;
            }

            (_heap[index], _heap[min]) = (_heap[min], _heap[index]);
            index = min;
        }
        return result;
    }

    public TElement Peek()
    {
        // 힙의 
        if (_heap.Count == 0)
        {
            throw new System.Exception("Queue is empty.");
        }
        return _heap[0].element;
    }


    public void Clear()
    {
        _heap.Clear();
    }
}