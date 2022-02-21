using UnityEngine;

public class Target : MonoBehaviour
{

    [SerializeField]
    private float hOffset = 0.2f;

    void Update ()
    {
        int button = 0;

        //Get the point of the hit position when the mouse is being clicked
        if(Input.GetMouseButtonDown(button)) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray.origin, ray.direction, out var hitInfo)) 
            {
                Vector3 targetPosition = hitInfo.point;
                transform.position = targetPosition + new Vector3(0.0f, hOffset, 0.0f);
            }
        }
    }

}