using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent[] navAgents;
    public Transform targetMarker;
    public float verticalOffset = 10.0f;

    void Start ()
    {
	    navAgents = FindObjectsOfType(typeof(UnityEngine.AI.NavMeshAgent)) as UnityEngine.AI.NavMeshAgent[];
    }

    void UpdateTargets(Vector3 targetPosition) {
        foreach (UnityEngine.AI.NavMeshAgent agent in navAgents) {
            agent.destination = targetPosition;
        }
    }

    void Update() {

        //Get the point of the hit position when the mouse is being clicked
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray.origin, ray.direction, out var hitInfo)) {
                Vector3 targetPosition = hitInfo.point;
                UpdateTargets(targetPosition);
                targetMarker.position = targetPosition + new Vector3(0, verticalOffset, 0);
            }
        }
    }

}