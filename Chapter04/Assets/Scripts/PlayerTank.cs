using UnityEngine;
using UnityEngine.Serialization;

public class PlayerTank : MonoBehaviour 
{
    public Transform targetTransform;

    [SerializeField]
    private float movementSpeed = 10.0f;

    [SerializeField]
    private float rotSpeed = 2.0f;

    [SerializeField]
    private float targetReactionRadius = 5.0f;
	
	// Update is called once per frame
	void Update () 
    {
        if(Vector3.Distance(transform.position, targetTransform.position) < targetReactionRadius)
            return;

        Vector3 tarPos = targetTransform.position;
        tarPos.y = transform.position.y;
        Vector3 dirRot = tarPos - transform.position;

        Quaternion tarRot = Quaternion.LookRotation(dirRot);
        transform.rotation = Quaternion.Slerp(transform.rotation, tarRot, rotSpeed * Time.deltaTime);

        transform.Translate(new Vector3(0, 0, movementSpeed * Time.deltaTime));
	}
}
