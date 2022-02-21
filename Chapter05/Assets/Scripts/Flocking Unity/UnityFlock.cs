using UnityEngine;
using System.Collections;
using Debug = System.Diagnostics.Debug;

public class UnityFlock : MonoBehaviour 
{
    public float minSpeed = 20.0f;         //movement speed of the flock
    public float turnSpeed = 20.0f;         //rotation speed of the flock
    public float randomFreq = 20.0f;        

    public float randomForce = 20.0f;       //Force strength in the unit sphere
    public float toOriginForce = 20.0f;     
    public float toOriginRange = 100.0f;

    public float gravity = 2.0f;            //Gravity of the flock

    public float avoidanceRadius = 50.0f;  //Minimum distance between flocks
    public float avoidanceForce = 20.0f;

    public float followVelocity = 4.0f;
    public float followRadius = 40.0f;      //Minimum Follow distance to the leader

    private Transform origin;               //Parent transform
    private Vector3 velocity;               //Velocity of the flock
    private Vector3 normalizedVelocity;
    private Vector3 randomPush;             //Random push value
    private Vector3 originPush;
    private Transform[] objects;            //Flock objects in the group
    private UnityFlock[] otherFlocks;       //Unity Flocks in the group
    private Transform transformComponent;   //My transform
    private float randomFreqInterval;

    void Start ()
    {
        randomFreqInterval = 1.0f / randomFreq;

        //Assign the parent as origin
	    origin = transform.parent;   
        
        //Flock transform           
	    transformComponent = transform;

        //Temporary components
        UnityFlock[] tempFlocks= null;

        //Get all the unity flock components from the parent transform in the group
        if (transform.parent)
        {
            tempFlocks = transform.parent.GetComponentsInChildren<UnityFlock>();
        }

        //Assign and store all the flock objects in this group
        Debug.Assert(tempFlocks != null, nameof(tempFlocks) + " != null");
        objects = new Transform[tempFlocks.Length];
        otherFlocks = new UnityFlock[tempFlocks.Length];

	    for(int i = 0;i<tempFlocks.Length;i++)
	    {
		    objects[i] = tempFlocks[i].transform;
		    otherFlocks[i] = tempFlocks[i];
	    }

        //Null Parent as the flock leader will be UnityFlockController object
        transform.parent = null;

        //Calculate random push depends on the random frequency provided
        StartCoroutine(UpdateRandom());
    }

    IEnumerator UpdateRandom ()
    {
	    while(true)
	    {
		    randomPush = Random.insideUnitSphere * randomForce;
            yield return new WaitForSeconds(randomFreqInterval + Random.Range(-randomFreqInterval / 2.0f, randomFreqInterval / 2.0f));
	    }
    }

    void Update() {
        //Internal variables
        float speed = velocity.magnitude;
        Vector3 avgVelocity = Vector3.zero;
        Vector3 avgPosition = Vector3.zero;
        int count = 0;

        Vector3 myPosition = transformComponent.position;
        Vector3 forceV;
        Vector3 toAvg;

        for (int i = 0; i < objects.Length; i++) {
            Transform boidTransform = objects[i];
            if (boidTransform != transformComponent) {
                Vector3 otherPosition = boidTransform.position;

                // Average position to calculate cohesion
                avgPosition += otherPosition;
                count++;

                //Directional vector from other flock to this flock
                forceV = myPosition - otherPosition;

                //Magnitude of that directional vector(Length)
                float directionMagnitude = forceV.magnitude;
                float forceMagnitude = 0.0f;

                //Add push value if the magnitude is less than follow radius to the leader
                if (directionMagnitude < followRadius) {
                    //calculate the velocity based on the avoidance distance between flocks 
                    //if the current magnitude is less than the specified avoidance radius
                    if (directionMagnitude < avoidanceRadius) {
                        forceMagnitude = 1.0f - (directionMagnitude / avoidanceRadius);

                        if (directionMagnitude > 0)
                            avgVelocity += (forceV / directionMagnitude) * forceMagnitude * avoidanceForce;
                    }

                    //just keep the current distance with the leader
                    forceMagnitude = directionMagnitude / followRadius;
                    UnityFlock tempOtherBoid = otherFlocks[i];
                    avgVelocity += followVelocity * forceMagnitude * tempOtherBoid.normalizedVelocity;
                }
            }
        }

        if (count > 0) {
            //Calculate the average flock velocity(Alignment)
            avgVelocity /= count;

            //Calculate Center value of the flock(Cohesion)
            toAvg = (avgPosition / count) - myPosition;
        } else {
            toAvg = Vector3.zero;
        }

        //Directional Vector to the leader
        forceV = origin.position - myPosition;
        float leaderDirectionMagnitude = forceV.magnitude;
        float leaderForceMagnitude = leaderDirectionMagnitude / toOriginRange;

        //Calculate the velocity of the flock to the leader
        if (leaderDirectionMagnitude > 0)
            originPush = leaderForceMagnitude * toOriginForce * (forceV / leaderDirectionMagnitude);

        if (speed < minSpeed && speed > 0) {
            velocity = (velocity / speed) * minSpeed;
        }

        Vector3 wantedVel = velocity;

        //Calculate final velocity
        wantedVel -= wantedVel * Time.deltaTime;
        wantedVel += randomPush * Time.deltaTime;
        wantedVel += originPush * Time.deltaTime;
        wantedVel += avgVelocity * Time.deltaTime;
        wantedVel += gravity * Time.deltaTime * toAvg.normalized;

        //Final Velocity to rotate the flock into
        velocity = Vector3.RotateTowards(velocity, wantedVel, turnSpeed * Time.deltaTime, 100.00f);
        transformComponent.rotation = Quaternion.LookRotation(velocity);

        //Move the flock based on the calculated velocity
        transformComponent.Translate(velocity * Time.deltaTime, Space.World);

        //normalise the velocity
        normalizedVelocity = velocity.normalized;
    }
}