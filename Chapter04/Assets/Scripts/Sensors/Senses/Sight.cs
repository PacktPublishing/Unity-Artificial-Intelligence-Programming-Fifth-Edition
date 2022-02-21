using UnityEngine;
using System.Collections;

public class Sight : Sense
{
    public int FieldOfView = 45;
    public int ViewDistance = 100;

    private Transform playerTrans;
    private Vector3 rayDirection;

    protected override void Initialize() 
    {
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
    }

	// Update is called once per frame
    protected override void UpdateSense() 
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= detectionRate) {
            DetectAspect();
            elapsedTime = 0.0f;
        }
	}

    //Detect perspective field of view for the AI Character
    void DetectAspect()
    {
        rayDirection = (playerTrans.position - transform.position).normalized;

        if ((Vector3.Angle(rayDirection, transform.forward)) < FieldOfView)
        {
            RaycastHit hit;
            // Detect if player is within the field of view
            if (Physics.Raycast(transform.position, rayDirection, out hit, ViewDistance))
            {
                Aspect aspect = hit.collider.GetComponent<Aspect>();
                if (aspect != null)
                {
                    //Check the aspect
                    if (aspect.affiliation == targetAffiliation)
                    {
                        print("Enemy Detected");
                    }
                }
            }
        }
    }

    /// <summary>
    /// Show Debug Grids and obstacles inside the editor
    /// </summary>
    void OnDrawGizmos()
    {
        if (!Application.isEditor || playerTrans == null)
            return;

        Debug.DrawLine(transform.position, playerTrans.position, Color.red);

        Vector3 frontRayPoint = transform.position + (transform.forward * ViewDistance);

        //Approximate perspective visualization
        Vector3 leftRayPoint = Quaternion.Euler(0f ,FieldOfView * 0.5f, 0f) * frontRayPoint;

        Vector3 rightRayPoint = Quaternion.Euler(0f, - FieldOfView*0.5f, 0f) *  frontRayPoint;

        Debug.DrawLine(transform.position, frontRayPoint, Color.green);
        Debug.DrawLine(transform.position, leftRayPoint, Color.green);
        Debug.DrawLine(transform.position, rightRayPoint, Color.green);
    }
}
