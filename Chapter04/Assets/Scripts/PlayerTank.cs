using UnityEngine;

public class PlayerTank : MonoBehaviour 
{
    public Transform targetTransform;
    private float movementSpeed, rotSpeed;

	// Use this for initialization
	void Start () 
    {
        movementSpeed = 10.0f;
        rotSpeed = 2.0f;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(Vector3.Distance(transform.position, targetTransform.position) < 5.0f)
            return;

        Vector3 tarPos = targetTransform.position;
        tarPos.y = transform.position.y;
        Vector3 dirRot = tarPos - transform.position;

        Quaternion tarRot = Quaternion.LookRotation(dirRot);
        transform.rotation = Quaternion.Slerp(transform.rotation, tarRot, rotSpeed * Time.deltaTime);

        transform.Translate(new Vector3(0, 0, movementSpeed * Time.deltaTime));
	}
}
