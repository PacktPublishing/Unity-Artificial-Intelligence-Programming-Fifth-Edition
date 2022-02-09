using UnityEngine;

public class VehicleFollowing : MonoBehaviour {
    public Path path;
    public float speed = 10.0f;

    [Range(1.0f, 1000.0f)]
    public float steeringInertia = 100.0f;

    public bool isLooping = true;
    public float waypointRadius = 1.0f;

    //Actual speed of the vehicle 
    private float curSpeed;

    private int curPathIndex = 0;
    private float pathLength;
    private Vector3 targetPoint;

    Vector3 velocity;

    // Use this for initialization
    void Start() {
        pathLength = path.Length;

        //get the current velocity of the vehicle
        velocity = transform.forward;
    }

    // Update is called once per frame
    void Update() {
        //Unify the speed
        curSpeed = speed * Time.deltaTime;

        targetPoint = path.GetPoint(curPathIndex);

        //If reach the radius of the waypoint then move to next point in the path
        if (Vector3.Distance(transform.position, targetPoint) < waypointRadius) {
            //Don't move the vehicle if path is finished 
            if (curPathIndex < pathLength - 1)
                curPathIndex++;
            else if (isLooping)
                curPathIndex = 0;
            else
                return;
        }

        //Move the vehicle until the end point is reached in the path
        if (curPathIndex >= pathLength)
            return;

        //Calculate the next Velocity towards the path
        if (curPathIndex >= pathLength - 1 && !isLooping)
            velocity += Steer(targetPoint, true);
        else
            velocity += Steer(targetPoint);

        transform.position += velocity; //Move the vehicle according to the velocity
        transform.rotation = Quaternion.LookRotation(velocity); //Rotate the vehicle towards the desired Velocity
    }

    //Steering algorithm to steer the vector towards the target
    public Vector3 Steer(Vector3 target, bool bFinalPoint = false) {
        //Calculate the directional vector from the current position towards the target point
        Vector3 desiredVelocity = (target - transform.position);
        float dist = desiredVelocity.magnitude;

        //Normalize the desired Velocity
        desiredVelocity.Normalize();

        // 
        if (bFinalPoint && dist < waypointRadius)
            desiredVelocity *= curSpeed * (dist / waypointRadius);
        else
            desiredVelocity *= curSpeed;

        //Calculate the force Vector
        Vector3 steeringForce = desiredVelocity - velocity;

        return steeringForce / steeringInertia;
    }
}