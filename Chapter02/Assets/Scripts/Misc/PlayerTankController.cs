using UnityEngine;
using System.Collections;

public class PlayerTankController : MonoBehaviour
{
    public GameObject Bullet;
	
    public GameObject Turret;
    public GameObject bulletSpawnPoint;
    public float curSpeed, targetSpeed;
    public float rotSpeed = 150.0f;
    public float turretRotSpeed = 10.0f;
    public float maxForwardSpeed = 300.0f;
    public float maxBackwardSpeed = -300.0f;
    public float shootRate = 0.5f;

    protected float elapsedTime;

    void Start()
    {

    }

    void OnEndGame()
    {
        // Don't allow any more control changes when the game ends
        this.enabled = false;
    }

    void Update()
    {
        UpdateControl();
        UpdateWeapon();
    }
    
    void UpdateControl()
    {
        //AIMING WITH THE MOUSE
        // Generate a plane that intersects the transform's position with an upwards normal.
        Plane playerPlane = new Plane(Vector3.up, transform.position + new Vector3(0, 0, 0));

        // Generate a ray from the cursor position
        Ray RayCast = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Determine the point where the cursor ray intersects the plane.
        float HitDist = 0;

        // If the ray is parallel to the plane, Raycast will return false.
        if (playerPlane.Raycast(RayCast, out HitDist))
        {
            // Get the point along the ray that hits the calculated distance.
            Vector3 RayHitPoint = RayCast.GetPoint(HitDist);

            Quaternion targetRotation = Quaternion.LookRotation(RayHitPoint - transform.position);
            Turret.transform.rotation = Quaternion.Slerp(Turret.transform.rotation, targetRotation, Time.deltaTime * turretRotSpeed);
        }

        if (Input.GetKey(KeyCode.W))
        {
            targetSpeed = maxForwardSpeed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            targetSpeed = maxBackwardSpeed;
        }
        else
        {
            targetSpeed = 0;
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0, -rotSpeed * Time.deltaTime, 0.0f);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, rotSpeed * Time.deltaTime, 0.0f);
        }

        //Determine current speed
        curSpeed = Mathf.Lerp(curSpeed, targetSpeed, 7.0f * Time.deltaTime);
        transform.Translate(Vector3.forward * Time.deltaTime * curSpeed);    
    }

    void UpdateWeapon()
    {
        elapsedTime += Time.deltaTime;
        if (Input.GetMouseButtonDown(0))
        {
            if (elapsedTime >= shootRate)
            {
                //Reset the time
                elapsedTime = 0.0f;

                //Also Instantiate over the PhotonNetwork
                Instantiate(Bullet, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
            }
        }
    }
}