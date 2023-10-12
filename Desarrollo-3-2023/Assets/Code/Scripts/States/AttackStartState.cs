using UnityEngine;

namespace Patterns.FSM
{
    public class AttackStartState<T> : BaseState<T>
    {
        private float timeOnHold;

        public AttackStartState(T id) : base(id)
        {
            timeOnHold = 0;
        }

        public AttackStartState(T id, string name) : base(id, name)
        {
            timeOnHold = 0;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            timeOnHold += Time.deltaTime;
        }

        public void Release()
        {
            Exit();
        }
    }
}
