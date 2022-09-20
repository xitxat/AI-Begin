using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph 
{ 

    List<Edge> edges = new List<Edge>();
    List<Node> nodes = new List<Node>();
    public List<Node> pathList = new List<Node>();

    public Graph() { }

    public void AddNode(GameObject id)
    {
        Node node = new Node(id);
        nodes.Add(node);
    }

    //  Go fromNode looks thru Node List to get from
    public void AddEdge(GameObject fromNode, GameObject toNode)
    {
        Node from = FindNode(fromNode);
        Node to = FindNode(toNode);

        // check if these 2 nodes exist and add to ?list
        if(from != null && to != null)
        {
            Edge e = new Edge(from, to);
            edges.Add(e);
            from.edgeList.Add(e);
        }
    }

    //  get the nodes we need using the ID of the GO  Node
    Node FindNode(GameObject id)
    {
        foreach (Node n in nodes)
        {
            if (n.getId() == id)
                //getId() == id)
                return n;
        }
        return null;
    }

    //  Implement A*
    public bool AStar(GameObject startId, GameObject endId)
    {
        Node start = FindNode(startId);
        Node end = FindNode(endId);

        // check
        if (start == null || end == null)
        {
            return false;
        }

        // A*
        List<Node> open = new List<Node>();
        List<Node> closed = new List<Node>();
        float tentative_g_score = 0;
        bool tenative_is_better;

        start.g = 0;     // dist. from Start
        start.h = distance(start, end);    // dist. to End
        start.f = start.h;

        open.Add(start);
        while(open.Count > 0)
        {
            int i = lowestF(open);
            Node thisNode = open[i];
            if (thisNode.getId() == endId)
            {
                // reconstructing the path start to end
                ReconstructPath(start, end);
                return true;
            }

            open.RemoveAt(i);
            closed.Add(thisNode);
            Node neighbour;
            foreach (Edge e in thisNode.edgeList)
            {
                neighbour = e.endNode;

                if (closed.IndexOf(neighbour) > -1)
                    continue;

                tentative_g_score = thisNode.g + distance(thisNode, neighbour);
                if (open.IndexOf(neighbour) == -1)
                {
                    open.Add(neighbour);
                    tenative_is_better = true;
                }
                else if (tentative_g_score < neighbour.g)
                {
                    tenative_is_better = true;
                }
                else
                    tenative_is_better = false;

                if (tenative_is_better)
                {
                    //  nodes constructed from this
                    neighbour.cameFrom = thisNode;
                    neighbour.g = tentative_g_score;
                    neighbour.h = distance(thisNode, end);
                    neighbour.f = neighbour.g + neighbour.h;
                }
            }

        }
        return false;  

    }

    //   setting path list var
    public void ReconstructPath(Node startId, Node endId)
    {
        pathList.Clear();
        pathList.Add(endId);

        var p = endId.cameFrom;
        while (p != startId && p != null)
        {
            pathList.Insert(0, p);
            p= p.cameFrom;
        }
        pathList.Insert(0, startId);
    }


    //  best guess to goal heuristic
    // get NOde with Lowest F value.
    float distance(Node a, Node b)
    {
        return(Vector3.SqrMagnitude(a.getId().transform.position - b.getId().transform.position));
    }

    int lowestF(List<Node> l)
    {
        float lowestf = 0;
        int count = 0;
        int iteratorCount = 0;

        lowestf = l[0].f;

        for(int i = 1; i < l.Count; i++)
        {
            if (l[i].f < lowestf)
            {
                lowestf = l[i].f;
                iteratorCount = count;
            }
            count++;
        }
        return iteratorCount;

    }

}
