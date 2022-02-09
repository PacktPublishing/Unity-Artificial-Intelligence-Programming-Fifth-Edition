using UnityEngine;

public class VehicleAvoidance : MonoBehaviour {
    public float vehicleRadius = 1.2f;
    public float speed = 10.0f;

    public float force = 50.0f;
    public float minimumDistToAvoid = 10.0f;
    public float targetReachedRadius = 3.0f;

    //Actual speed of the vehicle 
    private float curSpeed;
    private Vector3 targetPoint;

    // Use this for initialization
    void Start() {
        targetPoint = Vector3.zero;
    }

    void OnGUI() {
        GUILayout.Label("Click anywhere to move the vehicle to the clicked point");
    }

    // Update is called once per frame
    void Update() {
        //Vehicle move by mouse click
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out var hit, 100.0f)) {
            targetPoint = hit.point;
        }

        //Directional vector to the target position
        Vector3 dir = (targetPoint - transform.position);
        dir.Normalize();

        //Apply obstacle avoidance
        AvoidObstacles(ref dir);

        //Don't move the vehicle when the target point is reached
        if (Vector3.Distance(targetPoint, transform.position) < targetReachedRadius)
            return;

        //Assign the speed with delta time
        curSpeed = speed * Time.deltaTime;

        //Rotate the vehicle to its target directional vector
        var rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, 5.0f * Time.deltaTime);

        //Move the vehicle towards
        transform.position += transform.forward * curSpeed;
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }

    //Calculate the new directional vector to avoid the obstacle
    public void AvoidObstacles(ref Vector3 dir) {
        //Only detect layer 8 (Obstacles)
        int layerMask = 1 << 8;

        //Check that the vehicle hit with the obstacles within it's minimum distance to avoid
        if (Physics.SphereCast(transform.position, vehicleRadius, transform.forward, out var hit, minimumDistToAvoid, layerMask)) {
            //Get the normal of the hit point to calculate the new direction
            Vector3 hitNormal = hit.normal;
            hitNormal.y = 0.0f; //Don't want to move in Y-Space

            //Get the new directional vector by adding force to vehicle's current forward vector
            dir = transform.forward + hitNormal * force;
        }

    }
}