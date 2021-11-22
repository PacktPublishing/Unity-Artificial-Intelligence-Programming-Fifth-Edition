using UnityEngine;
using System.Collections;

public class PriorityQueue 
{
    // node array to store the priority queue
    private ArrayList nodes = new ArrayList();

    /// <summary>
    /// Number of nodes in the priority queue
    /// </summary>
    public int Length
    {
        get { return this.nodes.Count; }
    }

    /// <summary>
    /// Check whether the node is already in the queue or not
    /// </summary>
    public bool Contains(object node)
    {
        return this.nodes.Contains(node);
    }

    /// <summary>
    /// Get the first node in the queue
    /// </summary>
    public Node First()
    {
        if (this.nodes.Count > 0)
        {
            return (Node)this.nodes[0];
        }
        return null;
    }

    /// <summary>
    /// Add the node to the priority queue and sort with the estimated total cost
    /// </summary>
    public void Push(Node node)
    {
        this.nodes.Add(node);
        this.nodes.Sort();
    }

    /// <summary>
    /// Add the node from the priority queue and sort the remaining with the estimated total cost
    /// </summary>
    public void Remove(Node node)
    {
        this.nodes.Remove(node);
        this.nodes.Sort();
    }

}


