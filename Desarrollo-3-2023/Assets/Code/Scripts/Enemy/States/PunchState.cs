using Code.Scripts.Attack;

namespace Patterns.FSM
{
    /// <summary>
    /// Handler for punch state
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PunchState<T> : BaseState<T>
    {
        private readonly FirePunch leftPunch;
        private readonly FirePunch rightPunch;
        private float distance;

        public bool Ended { get; private set; }

        public PunchState(T id, FirePunch leftPunch, FirePunch rightPunch) : base(id)
        {
            this.leftPunch = leftPunch;
            this.rightPunch = rightPunch;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            Punch();
        }

        /// <summary>
        /// Initiate the punch
        /// </summary>
        private void Punch()
        {
            Ended = false;

            if (distance > 0)
            {
                leftPunch.gameObject.SetActive(true);
                leftPunch.Punch(distance);
            }
            else
            {
                rightPunch.gameObject.SetActive(true);
                rightPunch.Punch(distance);
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (!leftPunch.Move && !rightPunch.Move)
                Ended = true;
        }

        /// <summary>
        /// Set the distance to throw the punch to
        /// </summary>
        /// <param name="distance">Distance to throw</param>
        public void SetTargetDistance(float distance)
        {
            this.distance = distance;
        }
    }
}