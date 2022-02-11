using UnityEngine;

using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Tasks;     // TaskStatus

/// <summary>
/// DoneShootOnce is a action inherited from DoneShootOnce and Periodically clones a 'bullet' and
/// shoots it throught the Forward axis with the specified velocity. This action never ends.
/// </summary>
[Action("Chapter09/Shoot")]
[Help("Periodically clones a 'bullet' and shoots it through the Forward axis " +
      "with the specified velocity. This action never ends.")]
public class Shoot : ShootOnce {
    ///<value>Input delay Parameter in seconds, 30 by default.</value>
    // Define the input parameter delay, with the waited time between shoots.
    [InParam("delay", DefaultValue = 1.0f)]
    public float delay;

    // Time since the last shoot.
    private float elapsedTime = 0;


    /// <summary>Update method of DoneShoot.</summary>
    /// <returns>Return Running task.</returns>
    // Main class method, invoked by the execution engine.
    public override TaskStatus OnUpdate() {
        if (delay > 0) {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= delay) {
                elapsedTime = 0;
                return TaskStatus.RUNNING;
            }

        }

        base.OnUpdate();
        return TaskStatus.RUNNING;

    }

}