using Patterns.FSM;
using UnityEngine;

namespace Code.Scripts.States
{
    /// <summary>
    /// Handler for attack state
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AttackState<T> : BaseState<T>
    {
        private readonly GameObject hit;

        public AttackState(T id, string name, GameObject hit) : base(id, name)
        {
            this.hit = hit;
        }

        public override void OnEnter()
        {
            base.OnEnter();
        
            Debug.Log("Entered Attack");
            hit.SetActive(true);
        }
    }
}
