using UnityEngine;
using System;

public class Node {
    public float costSoFar;         //Total cost so far for the node
    public float fScore;         //Estimated cost from this node to the goal node
    public bool isObstacle;              //Does the node is an obstacle or not
    public Node parent;                 //Parent of the node in the linked list
    public Vector3 position;            //Position of the node

    /// <summary>
    //Constructor with adding position to the node creation
    /// </summary>
    public Node(Vector3 pos) {
        fScore = 0.0f;
        costSoFar = 0.0f;
        isObstacle = false;
        parent = null;
        position = pos;
    }

    /// <summary>
    //Make the node to be noted as an obstacle
    /// </summary>
    public void MarkAsObstacle() {
        isObstacle = true;
    }

    public override bool Equals(object obj) {
        return obj is Node node &&
               position.Equals(node.position);
    }

    public override int GetHashCode() {
        return HashCode.Combine(position);
    }
}
