
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph {

    // holds all edgeds going out from a node
    private Dictionary<Node, List<Edge>> data;
    public Graph() {
        data = new Dictionary<Node, List<Edge>>();
    }

    public void AddEdge(Edge e) {
        AddNode(e.from);
        AddNode(e.to);
        if (!data[e.from].Contains(e))
        {
            data[e.from].Add(e);
            e = new Edge(e.to, e.from);
            if (!data[e.from].Contains(e)) data[e.to].Add(e);
        }
    }

    public void RemoveNodeConnections(Node n)
    {
        if (data.ContainsKey(n))
        {
            foreach (Edge e in getConnections(n))
            {
                foreach (Edge f in getConnections(e.to))
                {
                    if (f.to == n)
                    {
                        if (data[e.to].Contains(f)) data[e.to].Remove(f);
                    }
                }
                if (data[n].Contains(e)) data[n].Remove(e);
            }
            //data.Remove(n);
        }
    }
    

    public Edge EdgeTo (Node from, Node to)
    {
        foreach (Edge e in data[from])
            if (e.to == to) return e;
        return null;
    } 
	// used only by AddEdge 
	public void AddNode(Node n) {
		if (!data.ContainsKey (n))
			data.Add (n, new List<Edge> ());
	}

	// returns the list of edged exiting from a node
	public Edge[] getConnections(Node n) {
		if (!data.ContainsKey (n)) return new Edge[0];
		return data [n].ToArray ();
	}

    public void setConnections(Node n, Edge[] edges)
    {
        if (!data.ContainsKey(n)) return;
        data[n] = new List<Edge>(edges);
    }

    public void setConnections()
    {
        foreach(Node n in getNodes())
        {
            if (getNode("" + n.x + "" + (n.y + 1)) != null) AddEdge(new Edge(n, getNode("" + n.x + "" + (n.y + 1))));
            if (getNode("" + n.x + "" + (n.y - 1)) != null) AddEdge(new Edge(n, getNode("" + n.x + "" + (n.y - 1))));
            if (getNode("" + (n.x + 1) + "" + n.y) != null) AddEdge(new Edge(n, getNode("" + (n.x + 1) + "" + n.y)));
            if (getNode("" + (n.x - 1) + "" + n.y) != null) AddEdge(new Edge(n, getNode("" + (n.x - 1) + "" + n.y)));
        }
    }

    public void changeWeight(String n, float newWeight)
    {
        foreach (Node node in getNodes())
            foreach (Edge e in data[node])
                if (e.to.description == n|| e.from.description == n)
                    e.weight = newWeight;
        return;
    }

    public Node getNode(String Description)
    {
        foreach(Node n in data.Keys)
        {
            if (n.description == Description) return n;
        }
        return null;
    }

	public Node[] getNodes() {
		return data.Keys.ToArray ();
	}

    public Node FindNear(float x, float y, float height, float xSize, float ySize, int xend, int yend)
    {
        float min = 2;
        Node candidate = null;
        foreach (Node n in getNodes())
        {
            if ((xend!=n.x||yend!=n.y) && Vector2.Distance(new Vector2(x,y), new Vector2(n.x * xSize, n.y * ySize)) < min)
            {
                candidate = n;
                min = Vector2.Distance(new Vector2(x, y), new Vector2(n.x*xSize, n.y*ySize));
            }
        }
        return candidate;
    }
}