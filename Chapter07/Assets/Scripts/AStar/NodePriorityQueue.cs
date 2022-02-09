using System.Collections.Generic;

public class NodePriorityQueue {
    private SortedSet<Node> nodes = new SortedSet<Node>();

    public int Length {
        get { return nodes.Count; }
    }

    public bool Contains(Node node) {
        return nodes.Contains(node);
    }

    public Node Dequeue() {
        if (nodes.Count > 0) {
            var result = nodes.Min;
            nodes.Remove(result);
            return result;
        }
        return null;
    }

    public void Enqueue(Node node) {
        if (nodes.Contains(node)) {
            // We remove and re-add the node to update
            // the other nodes properties.
            nodes.Remove(node);
        }
        nodes.Add(node);
    }

}
