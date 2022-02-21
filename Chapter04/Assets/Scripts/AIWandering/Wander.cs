using UnityEngine;
using System.Collections;

public class Wander : MonoBehaviour 
{
    private Vector3 tarPos;

    [SerializeField]
    private float movementSpeed = 5.0f;

    [SerializeField]
    private float rotSpeed = 2.0f;

    [SerializeField]
    private float minX = -45.0f;

    [SerializeField]
    private float maxX = 45.0f;

    [SerializeField]
    private float minZ = -45.0f;

    [SerializeField]
    private float maxZ = -45.0f;

    [SerializeField]
    private float targetReactionRadius = 5.0f;

    [SerializeField]
    private float targetVerticalOffset = 0.5f;

	// Use this for initialization
	void Start () 
    {
        //Get Wander Position
        GetNextPosition();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(Vector3.Distance(tarPos, transform.position) <= targetReactionRadius)
            GetNextPosition();

        Quaternion tarRot = Quaternion.LookRotation(tarPos - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, tarRot, rotSpeed * Time.deltaTime);

        transform.Translate(new Vector3(0f, 0f, movementSpeed * Time.deltaTime));
	}

    void GetNextPosition()
    {
        tarPos = new Vector3(Random.Range(minX, maxX), targetVerticalOffset, Random.Range(minZ, maxZ));
    }
}
