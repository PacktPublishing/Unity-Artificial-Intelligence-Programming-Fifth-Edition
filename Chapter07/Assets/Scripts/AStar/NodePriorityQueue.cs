using System.Collections.Generic;
using System.Linq;

public class NodePriorityQueue {
    private List<Node> nodes = new();

    public int Length {
        get { return nodes.Count; }
    }

    public bool Contains(Node node) {
        return nodes.Contains(node);
    }

    public Node Dequeue() {
        if (nodes.Count > 0) {
            var result = nodes[0];
            nodes.RemoveAt(0);
            return result;
        }
        return null;
    }

    public void Enqueue(Node node) {
        if (nodes.Contains(node)) {
            var oldNode = nodes.First(n => n.Equals(node));
            if (oldNode.fScore <= node.fScore) {
                return;
            } else {
                nodes.Remove(oldNode);
            }
        }
        nodes.Add(node);
        nodes = nodes.OrderBy(n => n.fScore).ToList();
    }

}
