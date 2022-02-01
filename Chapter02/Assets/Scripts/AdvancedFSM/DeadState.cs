using UnityEngine;
using System.Collections;

public class DeadState : FSMState
{
    public DeadState() 
    {
        stateID = FSMStateID.Dead;
    }

    public override void CheckTransitionRules(Transform player, Transform npc)
    {

    }

    public override void RunState(Transform player, Transform npc)
    {
        //Do Nothing for the dead state
    }
}
