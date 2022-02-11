using UnityEngine;

using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Tasks;     // TaskStatus
using BBUnity.Actions;        // GOAction

/// <summary>
/// DoneShootOnce is a action inherited from GOAction and Clone a 'bullet' and shoots 
/// it throught the Forward axis with the specified velocity.
/// </summary>
[Action("Chapter09/ShootOnce")]
[Help("Clone a 'bullet' and shoots it through the Forward axis with the " +
      "specified velocity.")]
public class ShootOnce : GOAction {

    ///<value>Input shootPoint Parameter.</value>
    // Define the input parameter "shootPoint".
    [InParam("shootPoint")]
    public Transform shootPoint;

    ///<value>Input bullet Parameter.</value>
    // Define the input parameter "bullet" (the prefab to be cloned).
    [InParam("bullet")]
    public GameObject bullet;

    ///<value>Input velocity Parameter, by deafult is 30f.</value>
    // Define the input parameter velocity, and provide a default
    // value of 30.0 when used as CONSTANT in the editor.
    [InParam("velocity", DefaultValue = 30f)]
    public float velocity;


    /// <summary>Initialization method of DoneShootOnce.</summary>
    /// <remarks>If the shootPoint is not established, we look for the shooting point.</remarks>

    // Initialization method. If not established, we look for the shooting point.
    public override void OnStart() {
        if (shootPoint == null) {
            shootPoint = gameObject.transform.Find("shootPoint");
            if (shootPoint == null) {
                Debug.LogWarning("Shoot point not specified. ShootOnce will not work " +
                                 "for " + gameObject.name);
            }
        }
        base.OnStart();
    } // OnStart


    /// <summary>Update method of DoneShootOnce.</summary>
    /// <remarks>Instantiate the bullet prefab, Search the RigitBody component in bullet instance. We add a rigitBody to bullet 
    /// if doesn´t exist, and then we give it a velocity.</remarks>
    /// <returns>Return FAILED if the shootPoint is null, and COMPLETE otherwise.</returns>
    // Main class method, invoked by the execution engine.
    public override TaskStatus OnUpdate() {
        if (shootPoint == null || bullet == null) {
            return TaskStatus.FAILED;
        }
        // Instantiate the bullet prefab.
        GameObject newBullet = Object.Instantiate(
                                    bullet, shootPoint.position,
                                    shootPoint.rotation * bullet.transform.rotation
                                );
        // Give it a velocity
        if (newBullet.GetComponent<Rigidbody>() == null)
            // Safeguard test, altough the rigid body should be provided by the
            // prefab to set its weight.
            newBullet.AddComponent<Rigidbody>();

        newBullet.GetComponent<Rigidbody>().velocity = velocity * shootPoint.forward;
        // The action is completed. We must inform the execution engine.
        return TaskStatus.COMPLETED;
    }

}
