using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

public class SimpleFSM : FSM {
    public enum FSMState {
        None,
        Patrol,
        Chase,
        Attack,
        Dead,
    }

    //Current state that the NPC is reaching
    public FSMState curState = FSMState.Patrol;

    //Speed of the tank
    private float curSpeed = 150.0f;

    //Tank Rotation Speed
    private float curRotSpeed = 2.0f;

    //Bullet
    public GameObject bullet;

    //Whether the NPC is destroyed or not
    private bool bDead = false;
    private int health = 100;

    // We overwrite the deprecated built-in `rigidbody` variable.
    new private Rigidbody rigidbody;

    //Player Transform
    private Transform playerTransform;

    //Next destination position of the NPC Tank
    private Vector3 destPos;

    //List of points for patrolling
    private GameObject[] pointList;

    //Bullet shooting rate
    public float shootRate = 3.0f;
    public float elapsedTime = 0.0f;
    public float maxFireAimError = 0.001f;

    // Patrolling Radius
    public float patrollingRadius = 100.0f;
    public float attackRadius = 200.0f;
    public float playerNearRadius = 300.0f;

    //Tank Turret
    public Transform turret;
    public Transform bulletSpawnPoint;


    //Initialize the Finite state machine for the NPC tank
    protected override void Initialize() {

        //Get the list of points
        pointList = GameObject.FindGameObjectsWithTag("WandarPoint");

        //Set Random destination point first
        FindNextPoint();

        //Get the target enemy(Player)
        GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");
        playerTransform = objPlayer.transform;

        // Get the rigidbody
        rigidbody = GetComponent<Rigidbody>();

        if (!playerTransform)
            print("Player doesn't exist. Please add one with Tag named 'Player'");

    }

    //Update each frame
    protected override void FSMUpdate() {
        switch (curState) {
            case FSMState.Patrol:
                UpdatePatrolState();
                break;
            case FSMState.Chase:
                UpdateChaseState();
                break;
            case FSMState.Attack:
                UpdateAttackState();
                break;
            case FSMState.Dead:
                UpdateDeadState();
                break;
        }

        //Update the time
        elapsedTime += Time.deltaTime;

        //Go to dead state is no health left
        if (health <= 0)
            curState = FSMState.Dead;
    }

    /// <summary>
    /// Patrol state
    /// </summary>
    protected void UpdatePatrolState() {
        //Find another random patrol point if the current point is reached
        if (Vector3.Distance(transform.position, destPos) <= patrollingRadius) {
            print("Reached the destination point\ncalculating the next point");
            FindNextPoint();
        }
        //Check the distance with player tank
        //When the distance is near, transition to chase state
        else if (Vector3.Distance(transform.position, playerTransform.position) <= playerNearRadius) {
            print("Switch to Chase Position");
            curState = FSMState.Chase;
        }

        //Rotate to the target point
        Quaternion targetRotation = Quaternion.LookRotation(destPos - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * curRotSpeed);

        //Go Forward
        transform.Translate(Vector3.forward * Time.deltaTime * curSpeed);
    }

    /// <summary>
    /// Chase state
    /// </summary>
    protected void UpdateChaseState() {
        //Set the target position as the player position
        destPos = playerTransform.position;

        //Check the distance with player tank
        //When the distance is near, transition to attack state
        float dist = Vector3.Distance(transform.position, playerTransform.position);
        if (dist <= attackRadius) {
            curState = FSMState.Attack;
        }
        //Go back to patrol is it become too far
        else if (dist >= playerNearRadius) {
            curState = FSMState.Patrol;
        }

        //Go Forward
        transform.Translate(Vector3.forward * Time.deltaTime * curSpeed);
    }

    /// <summary>
    /// Attack state
    /// </summary>
    protected void UpdateAttackState() {
        //Set the target position as the player position
        destPos = playerTransform.position;

        Vector3 frontVector = Vector3.forward;

        //Check the distance with the player tank
        float dist = Vector3.Distance(transform.position, playerTransform.position);
        if (dist >= attackRadius && dist < playerNearRadius) {
            // Rotate target point
            // The rotation is only around the vertical axis of the tank.
            Quaternion targetRotation = Quaternion.FromToRotation(frontVector, destPos - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * curRotSpeed);

            //Go Forward
            transform.Translate(Vector3.forward * Time.deltaTime * curSpeed);

            curState = FSMState.Attack;
        }
        //Transition to patrol is the tank become too far
        else if (dist >= playerNearRadius) {
            curState = FSMState.Patrol;
        }

        //Always Turn the turret towards the player
        Quaternion turretRotation = Quaternion.FromToRotation(frontVector, destPos - transform.position);
        turret.rotation = Quaternion.Slerp(turret.rotation, turretRotation, Time.deltaTime * curRotSpeed);

        //Shoot the bullets
        if (Mathf.Abs(Quaternion.Dot(turretRotation, turret.rotation)) > 1.0f - maxFireAimError) {
            ShootBullet();
        }
    }

    /// <summary>
    /// Dead state
    /// </summary>
    protected void UpdateDeadState() {
        //Show the dead animation with some physics effects
        if (!bDead) {
            bDead = true;
            Explode();
        }
    }

    /// <summary>
    /// Shoot the bullet from the turret
    /// </summary>
    private void ShootBullet() {
        if (elapsedTime >= shootRate) {
            //Shoot the bullet
            Instantiate(bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            elapsedTime = 0.0f;
        }
    }

    /// <summary>
    /// Check the collision with the bullet
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision) {
        //Reduce health
        if (collision.gameObject.tag == "Bullet")
            health -= collision.gameObject.GetComponent<Bullet>().damage;
    }

    /// <summary>
    /// Find the next semi-random patrol point
    /// </summary>
    protected void FindNextPoint() {
        print("Finding next point");
        int rndIndex = Random.Range(0, pointList.Length);
        float rndRadius = 10.0f;

        Vector3 rndPosition = Vector3.zero;
        destPos = pointList[rndIndex].transform.position + rndPosition;

        //Check Range
        //Prevent to decide the random point as the same as before
        if (IsInCurrentRange(destPos)) {
            rndPosition = new Vector3(Random.Range(-rndRadius, rndRadius), 0.0f, Random.Range(-rndRadius, rndRadius));
            destPos = pointList[rndIndex].transform.position + rndPosition;
        }
    }

    /// <summary>
    /// Check whether the next random position is the same as current tank position
    /// </summary>
    /// <param name="pos">position to check</param>
    protected bool IsInCurrentRange(Vector3 pos) {
        float xPos = Mathf.Abs(pos.x - transform.position.x);
        float zPos = Mathf.Abs(pos.z - transform.position.z);

        if (xPos <= 50f && zPos <= 50f)
            return true;

        return false;
    }

    protected void Explode() {
        float rndX = Random.Range(10.0f, 30.0f);
        float rndZ = Random.Range(10.0f, 30.0f);
        for (int i = 0; i < 3; i++) {
            rigidbody.AddExplosionForce(10000.0f, transform.position - new Vector3(rndX, 10.0f, rndZ), 40.0f, 10.0f);
            rigidbody.velocity = transform.TransformDirection(new Vector3(rndX, 20.0f, rndZ));
        }

        Destroy(gameObject, 1.5f);
    }

}
