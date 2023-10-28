using UnityEngine;

namespace Patterns.FSM
{
    public class AttackStartState<T> : BaseState<T>
    {
        private float timeOnHold;
        private readonly float minTimeOnHold;
        private bool released;

        public AttackStartState(T id, string name, float minTimeOnHold) : base(id, name)
        {
            this.minTimeOnHold = minTimeOnHold;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            released = false;
            timeOnHold = 0;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            timeOnHold += Time.deltaTime;
            
            if (released && timeOnHold >= minTimeOnHold)
                Exit();
        }

        public void Release()
        {
            released = true;
            
            if (timeOnHold < minTimeOnHold)
                return;
            
            Exit();
        }
    }
}
