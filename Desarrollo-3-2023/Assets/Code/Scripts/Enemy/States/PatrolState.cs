using Code.Scripts.States;
using UnityEngine;

namespace Code.Scripts.Enemy.States
{
    public class PatrolState<T> : MovementState<T>
    {
        public PatrolState(Rigidbody2D rb, T id, string name, float speed, Transform transform) : base(id, name, speed, transform, rb)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log("Entered Patrol");
        }

        public override void OnUpdate()
        {
            MoveInDirection(-1);
        }
    }
}
