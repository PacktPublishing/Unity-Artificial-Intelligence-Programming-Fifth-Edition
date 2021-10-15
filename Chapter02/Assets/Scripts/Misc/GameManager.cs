using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
    public float gameSpeed = 1.0f;

	// Use this for initialization
	void Start () 
    {
	    //Set the gravity Settings
        Physics.gravity = new Vector3(0, -500.0f, 0);
	}
	
	// Update is called once per frame
	void Update () 
    {
        Time.timeScale = gameSpeed;
	}
}
