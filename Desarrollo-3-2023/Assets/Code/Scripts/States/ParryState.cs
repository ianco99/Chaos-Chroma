using Code.SOs.States;
using UnityEngine;

namespace Patterns.FSM
{
    /// <summary>
    /// Handler for parry state
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ParryState<T> : BaseState<T>
    {
        private ParrySettings parrySettings;
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
