using UnityEngine;
using System.Collections;
using System;

using System.Linq;

public class FSM : MonoBehaviour {

	[Serializable]
	public enum FSMState {
        Chase,
        Flee,
        SelfDestruct,
    }

	[Serializable]
	public struct FSMProbability {
		public FSMState state;
		public int weight;
    }

	public FSMProbability[] states;

    FSMState selectState() {
        // Sum the weights of every state.
        var weightSum = states.Sum(state => state.weight);
        var randomNumber = UnityEngine.Random.Range(0, weightSum);
        var i = 0;
        while (randomNumber >= 0) {
            var state = states[i];
            randomNumber -= state.weight;
            if (randomNumber <= 0) {
                return state.state;
            }
            i++;
        }
        // It should not be possible to reach this point!
        throw new Exception("Something is wrong in the selectState algorithm!");
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FSMState randomState = selectState();
            Debug.Log(randomState.ToString());
        }
    }
}
