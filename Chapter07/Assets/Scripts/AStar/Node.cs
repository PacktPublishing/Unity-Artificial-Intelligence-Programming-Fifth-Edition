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
        estimatedCost = 0.0f;
        nodeTotalCost = 1.0f;
        bObstacle = false;
        parent = null;
    }

    /// <summary>
    //Constructor with adding position to the node creation
    /// </summary>
    public Node(Vector3 pos)
    {
        estimatedCost = 0.0f;
        nodeTotalCost = 1.0f;
        bObstacle = false;
        parent = null;
        position = pos;
    }

    /// <summary>
    //Make the node to be noted as an obstacle
    /// </summary>
    public void MarkAsObstacle()
    {
        bObstacle = true;
    }

    /// <summary>
    // This CompareTo methods affect on Sort method
    // It applies when calling the Sort method from ArrayList
    // Compare using the estimated total cost between two nodes
    /// </summary>
    public int CompareTo(object obj) {
        if (obj is not Node n || estimatedCost == n.estimatedCost) return 0;
        return estimatedCost > n.estimatedCost ? 1 : -1;
    }

    public override bool Equals(object obj) {
        return obj is Node node &&
               position.Equals(node.position);
    }

    public override int GetHashCode() {
        return HashCode.Combine(position);
    }
}


