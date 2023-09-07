using UnityEngine;

namespace Patterns.FSM
{
    public class ParryState<T> : BaseState<T>
    {
        private readonly Damageable parrier;
        private readonly float parryDuration;
        
        public ParryState(T id, string name, Damageable damageable, float parryDuration) : base(id, name)
        {
            parrier = damageable;
            this.parryDuration = parryDuration;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            
            Debug.Log("Entered Parry");
            
            parrier.StartParry(parryDuration);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            
            if (!parrier.parry)
                Exit();
        }
    }
}
