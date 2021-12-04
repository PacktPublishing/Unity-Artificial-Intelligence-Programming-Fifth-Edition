using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Tasks;     // TaskStatus

    /// <summary>
    /// DoneShootOnce is a action inherited from DoneShootOnce and Periodically clones a 'bullet' and
    /// shoots it throught the Forward axis with the specified velocity. This action never ends.
    /// </summary>
    [Action("Chapter09/Shoot")]
    [Help("Periodically clones a 'bullet' and shoots it through the Forward axis " +
          "with the specified velocity. This action never ends.")]
    public class Shoot : ShootOnce
    {
        ///<value>Input delay Parameter, 30 by default.</value>
        // Define the input parameter delay, with the waited game loops between shoots.
        [InParam("delay", DefaultValue = 30)]
        public int delay;

        // Game loops since the last shoot.
        private int elapsed = 0;


        /// <summary>Update method of DoneShoot.</summary>
        /// <returns>Return Running task.</returns>
        // Main class method, invoked by the execution engine.
        public override TaskStatus OnUpdate()
        {
            if (delay > 0)
            {
                ++elapsed;
                elapsed %= delay;
                if (elapsed != 0)
                    return TaskStatus.RUNNING;
            }

            base.OnUpdate();
            return TaskStatus.RUNNING;

        } 

    } 