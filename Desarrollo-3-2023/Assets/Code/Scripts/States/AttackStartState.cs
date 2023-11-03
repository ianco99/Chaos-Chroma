using UnityEngine;

namespace Patterns.FSM
{
    /// <summary>
    /// Handler for attack start state
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AttackStartState<T> : BaseState<T>
    {
        private float timeOnHold;
        private readonly float minTimeOnHold;
        private bool released;
        private float t;
        private readonly SpriteRenderer characterOutline;
        private readonly Color objectiveColor;

        public AttackStartState(T id, string name, float minTimeOnHold, SpriteRenderer characterOutline, Color objectiveColor) : base(id, name)
        {
            this.minTimeOnHold = minTimeOnHold;
            this.characterOutline = characterOutline;
            this.objectiveColor = objectiveColor;
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
            
            if (released && timeOnHold >= minTimeOnHold)
                Exit();
        }

        /// <summary>
        /// Stop holding attack and exit state
        /// </summary>
        public void Release()
        {
            released = true;
            
            if (timeOnHold < minTimeOnHold)
                return;
            
            Exit();
        }
        
        /// <summary>
        /// Sets the color of the character outline to the current state of the attack
        /// </summary>
        private void UpdateCharacterOutlineColor()
        {
            t += Time.deltaTime / minTimeOnHold;

            Color color = Vector4.Lerp(Color.white, objectiveColor, t);
            
            characterOutline.color = color;
        }
    }
}
