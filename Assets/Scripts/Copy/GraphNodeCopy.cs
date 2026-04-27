using System.Collections.Generic;
using System.Collections;
using System.Linq;


public class GraphNode1
{
    public int id;
    public int weight;
    public GraphNode1 previous = null;

    public List<GraphNode1> adjacents = new();

    public bool CanVisit => adjacents.Count > 0 && weight >= 0;
}