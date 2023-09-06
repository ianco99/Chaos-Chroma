using Code.Scripts.Input;
using Patterns.FSM;
using UnityEngine;

namespace Code.Scripts.States
{
    public class MovementState<T> : BaseState<T>
    {
        private readonly Transform transform;
        private readonly float speed;
        private float dir;
        
        public MovementState(T id, string name, float speed, Transform transform) : base(id, name)
        {
            this.speed = speed;
            this.transform = transform;
        }


        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log("Entered Move");
            
            InputManager.onMove += UpdateDir;
        }

        public override void OnExit()
        {
            base.OnExit();
            
            InputManager.onMove -= UpdateDir;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            
            MoveInDirection(dir);
        }

        /// <summary>
        /// Moves current object in given direction
        /// </summary>
        /// <param name="direction"></param>
        private void MoveInDirection(float direction)
        {
            transform.Translate(Vector3.right * (direction * speed * Time.deltaTime));
        }

        private void UpdateDir(float direction)
        {
            dir = direction;
        }
    }
}
