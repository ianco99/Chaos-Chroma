using Code.Scripts.Attack;
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
            hit.SetActive(true);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (!hit.activeSelf)
                Exit();
        }

        /// <summary>
        /// Interrupt attack state
        /// </summary>
        public void Stop()
        {
            if (hit.TryGetComponent(out HitsManager hitManager))
                hitManager.Stop();

            hit.SetActive(false);
            Exit();
        }
    }
}