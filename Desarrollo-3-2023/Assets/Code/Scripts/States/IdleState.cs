using Patterns.FSM;
using UnityEngine;

namespace Code.Scripts.States
{
    public class IdleState<T> : BaseState<T>
    {
        public IdleState(T id) : base(id)
        {
        }

        public IdleState(T id, string name) : base(id, name)
        {
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
            
            Debug.Log("Entered Idle");
        }
    }
}
