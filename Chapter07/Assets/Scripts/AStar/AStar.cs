using UnityEngine;
using System.Collections.Generic;

public class AStar {

    /// <summary>
    /// Calculate the final path in the path finding
    /// </summary>
    private List<Node> CalculatePath(Node node) {
        List<Node> list = new();
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
        return (curNode.position - goalNode.position).magnitude;
    }

    /// <summary>
    /// Find the path between start node and goal node using AStar Algorithm
    /// </summary>
    public List<Node> FindPath(Node start, Node goal) {

        //Start Finding the path
        NodePriorityQueue openList = new NodePriorityQueue();
        openList.Enqueue(start);
        start.costSoFar = 0.0f;
        start.fScore = HeuristicEstimateCost(start, goal);

        HashSet<Node> closedList = new();
        Node node = null;

        while (openList.Length != 0) {
            node = openList.Dequeue();

            if (node.position == goal.position) {
                return CalculatePath(node);
            }

            var neighbours = GridManager.instance.GetNeighbours(node);

            //Get the Neighbours
            foreach (Node neighbourNode in neighbours) {
                if (!closedList.Contains(neighbourNode)) {
                    //Total Cost So Far from start to this neighbour node
                    float totalCost = node.costSoFar + GridManager.instance.StepCost;

                    //Estimated cost for neighbour node to the goal
                    float heuristicValue = HeuristicEstimateCost(neighbourNode, goal);

                    //Assign neighbour node properties
                    neighbourNode.costSoFar = totalCost;
                    neighbourNode.parent = node;
                    neighbourNode.fScore = totalCost + heuristicValue;

                    //Add the neighbour node to the queue
                    if (!closedList.Contains(neighbourNode)) {
                        openList.Enqueue(neighbourNode);
                    }
                    
                }
            }

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
