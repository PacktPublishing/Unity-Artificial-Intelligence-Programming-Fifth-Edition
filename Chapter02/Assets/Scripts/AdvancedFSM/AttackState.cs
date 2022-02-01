using UnityEngine;
using System.Collections;

public class AttackState : FSMState
{
    private Vector3 destPos;
    private Transform[] waypoints;
    private float curRotSpeed = 1.0f;
    private float curSpeed = 100.0f;

    public AttackState(Transform[] wp) 
    { 
        waypoints = wp;
        stateID = FSMStateID.Attacking;
        curRotSpeed = 1.0f;
        curSpeed = 100.0f;

        //find next Waypoint position
        FindNextPoint();
    }

    public override void CheckTransitionRules(Transform player, Transform npc)
    {
        //Check the distance with the player tank
        float dist = Vector3.Distance(npc.position, player.position);
        if (dist >= 200.0f && dist < 300.0f)
        {
            //Rotate to the target point
            Quaternion targetRotation = Quaternion.LookRotation(destPos - npc.position);
            npc.rotation = Quaternion.Slerp(npc.rotation, targetRotation, Time.deltaTime * curRotSpeed);

            //Go Forward
            npc.Translate(Vector3.forward * Time.deltaTime * curSpeed);

            Debug.Log("Switch to Chase State");
            npc.GetComponent<NPCTankController>().SetTransition(Transition.SawPlayer);
        }
        //Transition to patrol is the tank become too far
        else if (dist >= 300.0f)
        {
            Debug.Log("Switch to Patrol State");
            npc.GetComponent<NPCTankController>().SetTransition(Transition.LostPlayer);
        }  
    }

    public override void RunState(Transform player, Transform npc)
    {
        //Set the target position as the player position
        destPos = player.position;

        //Always Turn the turret towards the player
        Transform turret = npc.GetComponent<NPCTankController>().turret;
        Quaternion turretRotation = Quaternion.LookRotation(destPos - turret.position);
        turret.rotation = Quaternion.Slerp(turret.rotation, turretRotation, Time.deltaTime * curRotSpeed);

        //Shoot bullet towards the player
        npc.GetComponent<NPCTankController>().ShootBullet();
    }

    /// <summary>
    /// Find the next semi-random patrol point
    /// </summary>
    public void FindNextPoint() {
        //Debug.Log("Finding next point");
        int rndIndex = Random.Range(0, waypoints.Length);
        Vector3 rndPosition = Vector3.zero;
        destPos = waypoints[rndIndex].position + rndPosition;
    }
}
