using System.Collections;
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
        private readonly HitsManager hitsManager;

        private bool isAttacking;
        private readonly MonoBehaviour mb;

        public AttackEndState(T id, string name, GameObject hit, MonoBehaviour mb, AK.Wwise.Event playEspada = null) : base(id, name)
        {
            this.hit = hit;
            this.playEspada = playEspada;
            this.mb = mb;
            hitsManager = this.hit.GetComponent<HitsManager>();
        }

        public override void OnEnter()
        {
            base.OnEnter();

            hit.SetActive(true);

            playEspada?.Post(hit);

            isAttacking = true;
        }

        public override void OnExit()
        {
            base.OnExit();

            isAttacking = false;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (!hit.activeSelf)
            {
                Exit();
            }
        }

        /// <summary>
        /// Interrupt attack state
        /// </summary>
        public void Stop()
        {
            if (hitsManager)
            {
                hitsManager.Stop();
            }

            hit.SetActive(false);
            Exit();
        }

        /// <summary>
        /// Sets the direction for the attack if the character is not currently attacking.
        /// </summary>
        /// <param name="direction">The direction vector to set for the attack.</param>
        public void SetDir(Vector2 direction)
        {
            if (isAttacking)
                mb.StartCoroutine(WaitAndSetDir(direction));

            hitsManager.SetDir(direction);
        }

        private IEnumerator WaitAndSetDir(Vector2 direction)
        {
            yield return new WaitUntil(() =>!isAttacking);
        }

        public int Dir => hitsManager.DirAsInt;
    }
}