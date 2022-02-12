using UnityEngine;
using System;
using UnityEngine.AI;

public class AITankController : FSM
{
    public Complete.TankShooting tankShooter;
    public Complete.TankHealth tankHealth;
    public float playerChaseRadius = 15.0f;
    public float platerAttackRadius = 10.0f;
    public float shootRate = 3.0f;
    public float targetReachedRadius = 5.0f;

    private bool isDead = false;
    private float elapsedTime = 0.0f;

    private GameObject player = null;
    private NavMeshAgent navMeshAgent;

    public enum FSMState
    {
        None,
        Patrol,
        Chase,
        Attack,
        Dead,
    }

    //Current state that the NPC is reaching
    public FSMState curState;

    //Initialize the Finite state machine for the NPC tank
    protected override void Initialize()
    {

        navMeshAgent = GetComponent<NavMeshAgent>();

        //Get the list of points
        pointList = GameObject.FindGameObjectsWithTag("PatrolPoint");

        int rndIndex = UnityEngine.Random.Range(0, pointList.Length);
        destPos = pointList[rndIndex].transform.position;

    }

    //Update each frame
    protected override void FSMUpdate()
    {
  
        switch (curState)
        {
            case FSMState.Patrol: UpdatePatrolState(); break;
            case FSMState.Chase: UpdateChaseState(); break;
            case FSMState.Attack: UpdateAttackState(); break;
            case FSMState.Dead: UpdateDeadState(); break;
        }

        elapsedTime += Time.deltaTime;

        //Go to dead state is no health left
        if (this.tankHealth.CurrentHealth <= 0)
            curState = FSMState.Dead;
    }

    private void UpdateDeadState()
    {
        if (!isDead)
        {
            Debug.Log("Dead");
        }
    }

    private void UpdateAttackState()
    {

        Collider[] players = Physics.OverlapSphere(transform.position, playerChaseRadius, LayerMask.GetMask("Players"));
        
        if (players.Length == 0)
        {
            curState = FSMState.Patrol;
            player = null;
            navMeshAgent.enabled = true;
            return;
        }

        player = players[0].gameObject;

        Vector3 _direction = (player.transform.position - transform.position).normalized;
        Quaternion _lookRotation = Quaternion.LookRotation(_direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * 3);

        if (elapsedTime > shootRate)
        {
            this.tankShooter.Fire();
            elapsedTime = 0;
        }
    }

    private void UpdateChaseState()
    {
        throw new NotImplementedException();
    }

    private void UpdatePatrolState()
    {
        Collider[] players = Physics.OverlapSphere(transform.position, platerAttackRadius, LayerMask.GetMask("Players"));

        if (players.Length > 0)
        {
            curState = FSMState.Attack;
            player = players[0].gameObject;
            navMeshAgent.enabled = false;
            return;
        }

        if (IsInCurrentRange(destPos))
        {
            Debug.Log("I'm in range.");
            int rndIndex = UnityEngine.Random.Range(0, pointList.Length);
            destPos = pointList[rndIndex].transform.position;
            Debug.Log("Moving to " + destPos.ToString());
        }

        navMeshAgent.destination = destPos;
    }

    /// <summary>
    /// Check whether the next random position is the same as current tank position
    /// </summary>
    /// <param name="pos">position to check</param>
    protected bool IsInCurrentRange(Vector3 pos)
    {
        float xPos = Mathf.Abs(pos.x - transform.position.x);
        float zPos = Mathf.Abs(pos.z - transform.position.z);

        if (xPos <= targetReachedRadius && zPos <= targetReachedRadius)
            return true;

        return false;
    }

}
