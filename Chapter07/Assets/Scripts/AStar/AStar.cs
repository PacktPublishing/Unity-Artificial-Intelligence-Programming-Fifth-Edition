using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStar {
    #region List fields

    public NodePriorityQueue openList;
    public HashSet<Node> closedList;

    #endregion

    /// <summary>
    /// Calculate the final path in the path finding
    /// </summary>
    private List<Node> CalculatePath(Node node) {
        List<Node> list = new List<Node>();
        while (node != null) {
            list.Add(node);
            node = node.parent;
        }
        list.Reverse();
        return list;
    }

    /// <summary>
    /// Calculate the estimated Heuristic cost to the goal
    /// </summary>
    private float HeuristicEstimateCost(Node curNode, Node goalNode) {
        Vector3 vecCost = curNode.position - goalNode.position;
        return vecCost.magnitude;
    }

    /// <summary>
    /// Find the path between start node and goal node using AStar Algorithm
    /// </summary>
    public List<Node> FindPath(Node start, Node goal) {
        //Start Finding the path
        openList = new NodePriorityQueue();
        openList.Enqueue(start);
        start.nodeTotalCost = 0.0f;
        start.estimatedCost = HeuristicEstimateCost(start, goal);

        closedList = new HashSet<Node>();
        Node node = null;

        while (openList.Length != 0) {
            node = openList.Dequeue();

            if (node.position == goal.position) {
                return CalculatePath(node);
            }

            var neighbours = GridManager.instance.GetNeighbours(node);

            #region CheckNeighbours

            //Get the Neighbours
            for (int i = 0; i < neighbours.Count; i++) {
                //Cost between neighbour nodes
                Node neighbourNode = neighbours[i];

                if (!closedList.Contains(neighbourNode)) {
                    //Cost from current node to this neighbour node
                    float cost = HeuristicEstimateCost(node, neighbourNode);

                    //Total Cost So Far from start to this neighbour node
                    float totalCost = node.nodeTotalCost + cost;

                    //Estimated cost for neighbour node to the goal
                    float neighbourNodeEstCost = HeuristicEstimateCost(neighbourNode, goal);

                    //Assign neighbour node properties
                    neighbourNode.nodeTotalCost = totalCost;
                    neighbourNode.parent = node;
                    neighbourNode.estimatedCost = totalCost + neighbourNodeEstCost;

                    //Add the neighbour node to the queue
                    openList.Enqueue(neighbourNode);
                }
            }

            #endregion

            closedList.Add(node);
        }

        //If finished looping and cannot find the goal then return null
        if (node.position != goal.position) {
            Debug.LogError("Goal Not Found");
            return null;
        }

        //Calculate the path based on the final node
        return CalculatePath(node);
    }
}
