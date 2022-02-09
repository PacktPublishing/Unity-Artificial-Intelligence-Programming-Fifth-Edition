using UnityEngine;

public class Path : MonoBehaviour {
    public bool isDebug = true;
    public Transform[] waypoints;

    public float Length {
        get {
            return waypoints.Length;
        }
    }

    public Vector3 GetPoint(int index) {
        return waypoints[index].position;
    }

    /// <summary>
    /// Show Debug Grids and obstacles inside the editor
    /// </summary>
    void OnDrawGizmos() {
        if (!isDebug)
            return;

        for (int i = 1; i < waypoints.Length; i++) {
            Debug.DrawLine(waypoints[i-1].position, waypoints[i].position, Color.red);
        }
    }
}