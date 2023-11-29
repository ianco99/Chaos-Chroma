using Code.Scripts.Attack;
using Patterns.FSM;
using UnityEngine;

namespace Code.Scripts.States
{
    /// <summary>
    /// Handler for attack state
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AttackEndState<T> : BaseState<T>
    {
        private readonly GameObject hit;
        private AK.Wwise.Event playEspada;
        public AttackEndState(T id, string name, GameObject hit, AK.Wwise.Event playEspada = null) : base(id, name)
        {
            this.hit = hit;
            this.playEspada = playEspada;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            hit.SetActive(true);
            
            if(playEspada != null)
            {
                playEspada.Post(hit);
            }
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