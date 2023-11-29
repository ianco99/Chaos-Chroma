using Code.SOs.States;
using UnityEngine;

namespace Patterns.FSM
{
    /// <summary>
    /// Handler for attack start state
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AttackStartState<T> : BaseState<T>
    {
        private AttackStartSettings attackStartSettings;
        private float timeOnHold;
        private bool released;
        private float t;
        private readonly SpriteRenderer characterOutline;

        public AttackStartState(T id, string name, AttackStartSettings settings, SpriteRenderer characterOutline) : base(id, name)
        {
            this.characterOutline = characterOutline;
            attackStartSettings = settings;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            
            Enter();

            released = false;
            timeOnHold = 0;
            t = 0;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            timeOnHold += Time.deltaTime;
            UpdateCharacterOutlineColor();
            
            if (released && timeOnHold >= attackStartSettings.minTimeOnHold)
                Exit();
        }
        
        public override void OnExit()
        {
            base.OnExit();
            
            characterOutline.color = Color.clear;
        }

        /// <summary>
        /// Stop holding attack and exit state
        /// </summary>
        public void Release()
        {
            released = true;
            
            if (timeOnHold < attackStartSettings.minTimeOnHold)
                return;
            
            Exit();
        }
        
        /// <summary>
        /// Sets the color of the character outline to the current state of the attack
        /// </summary>
        private void UpdateCharacterOutlineColor()
        {
            t += Time.deltaTime / attackStartSettings.minTimeOnHold;

            Color color = Vector4.Lerp(Color.white, attackStartSettings.objectiveColor, t);
            
            characterOutline.color = color;
        }
    }
}
