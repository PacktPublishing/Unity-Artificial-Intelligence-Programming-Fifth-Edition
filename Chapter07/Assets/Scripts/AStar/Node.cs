using UnityEngine;
using System.Collections;
using System;

public class Node : IComparable
{
    #region Fields
    public float nodeTotalCost;         //Total cost so far for the node
    public float estimatedCost;         //Estimated cost from this node to the goal node
    public bool bObstacle;              //Does the node is an obstacle or not
    public Node parent;                 //Parent of the node in the linked list
    public Vector3 position;            //Position of the node
    #endregion

    /// <summary>
    //Default Constructor
    /// </summary>
    public Node()
    {
        this.estimatedCost = 0.0f;
        this.nodeTotalCost = 1.0f;
        this.bObstacle = false;
        this.parent = null;
    }

    /// <summary>
    //Constructor with adding position to the node creation
    /// </summary>
    public Node(Vector3 pos)
    {
        this.estimatedCost = 0.0f;
        this.nodeTotalCost = 1.0f;
        this.bObstacle = false;
        this.parent = null;

        this.position = pos;
    }

    /// <summary>
    //Make the node to be noted as an obstacle
    /// </summary>
    public void MarkAsObstacle()
    {
        this.bObstacle = true;
    }

    /// <summary>
    // This CompareTo methods affect on Sort method
    // It applies when calling the Sort method from ArrayList
    // Compare using the estimated total cost between two nodes
    /// </summary>
    public int CompareTo(object obj)
    {
        Node node = (Node)obj;
        if (this.estimatedCost < node.estimatedCost)
            return -1;
        if (this.estimatedCost > node.estimatedCost)
            return 1;

        return 0;
    }
}


