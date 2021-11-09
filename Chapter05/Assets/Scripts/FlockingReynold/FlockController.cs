using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This script is the modification of the implementation of the Flocks behaviors from http://www.unifycommunity.com/wiki/index.php?title=Flocking
/// </summary>

public class FlockController : MonoBehaviour
{
	public float minVelocity = 1;       //Min Velocity
	public float maxVelocity = 8;       //Max Flock speed
	public int flockSize = 20;          //Number of flocks in the group
	public float centerWeight = 1;      //How far the flocks should stick to the center ( the more weight stick closer to the center)
	public float velocityWeight = 1;    //Alignment behaviors
	public float separationWeight = 1;  //Determine how far flock should be seperated within the group
	public float followWeight = 1;      //How close the flock should follow to the leader(the more weight make the closer follow)
	public float randomizeWeight = 1;   //Additional Random Noise
	
	public Flock prefab;
	public Transform target;
	
	internal Vector3 flockCenter;       //Center position of the flock in the group
	internal Vector3 flockVelocity;     //Average Velocity of the flock

    public ArrayList flockList = new ArrayList();

	void Start()
	{		
		for (int i = 0; i < flockSize; i++)
		{
			Flock flock = Instantiate(prefab, transform.position, transform.rotation) as Flock;
            flock.transform.parent = transform;
            flock.controller = this;
            flockList.Add(flock);
		}
	}

	void Update()
	{
        //Calculate the Center and Velocity of the whole flock group
		Vector3 center = Vector3.zero;
		Vector3 velocity = Vector3.zero;
		
        foreach (Flock flock in flockList)
		{
            center += flock.transform.localPosition;
            velocity += flock.GetComponent<Rigidbody>().velocity;
		}

		flockCenter = center / flockSize;
		flockVelocity = velocity / flockSize;
	}
}