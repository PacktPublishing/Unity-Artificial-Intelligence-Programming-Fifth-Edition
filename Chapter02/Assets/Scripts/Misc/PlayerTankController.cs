using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

public class PlayerTankController : MonoBehaviour {
    public GameObject bullet;
    public GameObject turret;
    public GameObject bulletSpawnPoint;
    public float curSpeed, targetSpeed;
    public float rotSpeed = 150.0f;
    public float turretRotSpeed = 10.0f;
    public float maxForwardSpeed = 300.0f;
    public float maxBackwardSpeed = -300.0f;
    public float shootRate = 0.5f;

    private float elapsedTime;

    void OnEndGame() {
        // Don't allow any more control changes when the game ends
        this.enabled = false;
    }

    void Update() {
        UpdateControl();
        UpdateWeapon();
    }

    void UpdateControl() {
        //AIMING WITH THE MOUSE
        // Generate a plane that intersects the transform's position with an upwards normal.
        Plane playerPlane = new Plane(Vector3.up, transform.position + new Vector3(0f, 0f, 0f));

        // Generate a ray from the cursor position
        Ray rayCast = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Determine the point where the cursor ray intersects the plane.

        // If the ray is parallel to the plane, Raycast will return false.
        if (playerPlane.Raycast(rayCast, out var hitDist)) {
            // Get the point along the ray that hits the calculated distance.
            Vector3 rayHitPoint = rayCast.GetPoint(hitDist);

            Quaternion targetRotation = Quaternion.LookRotation(rayHitPoint - transform.position);
            turret.transform.rotation = Quaternion.Slerp(turret.transform.rotation, targetRotation, Time.deltaTime * turretRotSpeed);
        }

        if (Input.GetKey(KeyCode.W)) {
            targetSpeed = maxForwardSpeed;
        }
        else if (Input.GetKey(KeyCode.S)) {
            targetSpeed = maxBackwardSpeed;
        }
        else {
            targetSpeed = 0f;
        }

        if (Input.GetKey(KeyCode.A)) {
            transform.Rotate(0f, -rotSpeed * Time.deltaTime, 0f);
        }
        else if (Input.GetKey(KeyCode.D)) {
            transform.Rotate(0f, rotSpeed * Time.deltaTime, 0f);
        }

        //Determine current speed
        curSpeed = Mathf.Lerp(curSpeed, targetSpeed, 7.0f * Time.deltaTime);
        transform.Translate(Vector3.forward * Time.deltaTime * curSpeed);
    }

    void UpdateWeapon() {
        elapsedTime += Time.deltaTime;
        if (Input.GetMouseButtonDown(0)) {
            if (elapsedTime >= shootRate) {
                //Reset the time
                elapsedTime = 0.0f;

                //Also Instantiate over the PhotonNetwork
                Instantiate(bullet, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
            }
        }
    }
}