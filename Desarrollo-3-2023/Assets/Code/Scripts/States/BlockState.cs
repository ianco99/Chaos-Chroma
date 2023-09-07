using UnityEngine;

namespace Patterns.FSM
{
    /// <summary>
    /// Handler for attack state
    /// </summary>
    public class BlockState<T> : BaseState<T>
    {
        private readonly Damageable blocker;
        
        public BlockState(T id, string name, Damageable damageable) : base(id, name)
        {
            blocker = damageable;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            
            Debug.Log("Entered Block");
            
            blocker.StartBlock();
        }

        public override void OnExit()
        {
            base.OnExit();
            
            blocker.StopBlock();
        }
    }
}
