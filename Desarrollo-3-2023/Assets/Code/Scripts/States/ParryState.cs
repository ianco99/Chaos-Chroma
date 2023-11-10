using Code.SOs.States;

namespace Patterns.FSM
{
    public class ParryState<T> : BaseState<T>
    {
        private readonly ParrySettings parrySettings;
        private readonly Damageable parrier;
        
        public ParryState(T id, string name, Damageable damageable, ParrySettings settings) : base(id, name)
        {
            parrier = damageable;
            parrySettings = settings;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            
            parrier.StartParry(parrySettings.duration);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            
            if (!parrier.parry)
                Exit();
        }
    }
}
