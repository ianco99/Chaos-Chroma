using Code.SOs.States;
using Patterns.FSM;
using UnityEngine;

namespace Code.Scripts.States
{
    /// <summary>
    /// Handler for movement state
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MovementState<T> : BaseState<T>
    {
        protected readonly Rigidbody2D rb;
        protected readonly Transform transform;
        protected MoveSettings settings;

        private AK.Wwise.Event playFootstep;
        private AK.Wwise.Event stopFootstep;

        private bool isPlayingSound = false;

        public Vector2 dir;

        public MovementState(T id, MoveSettings settings, Transform transform, Rigidbody2D rb, AK.Wwise.Event playFootstep = null, AK.Wwise.Event stopFootstep = null) : base(id)
        {
            this.settings = settings;
            this.transform = transform;
            this.rb = rb;

            this.playFootstep = playFootstep;
            this.stopFootstep = stopFootstep;
        }

        public MovementState(T id, string name, MoveSettings settings, Transform transform, Rigidbody2D rb, AK.Wwise.Event playFootstep = null, AK.Wwise.Event stopFootstep = null) : base(id, name)
        {
            this.settings = settings;
            this.transform = transform;
            this.rb = rb;

            this.playFootstep = playFootstep;
            this.stopFootstep = stopFootstep;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            CheckSound();

            if (Mathf.Approximately(dir.x, 0))
                Exit();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            Vector2 direction = transform.right * dir.x;

            MoveInDirection(direction);
            ClampSpeed();
        }

        public override void OnExit()
        {
            base.OnExit();

            if (isPlayingSound)
            {
                stopFootstep.Post(rb.gameObject);
                isPlayingSound = false;
            }
        }

        /// <summary>
        /// Moves current object in given direction
        /// </summary>
        /// <param name="direction"></param>
        private void MoveInDirection(Vector2 direction)
        {
            rb.AddForce(direction * (settings.accel * Time.fixedDeltaTime), ForceMode2D.Impulse);
        }

        /// <summary>
        /// Limit the speed of the moving object
        /// </summary>
        private void ClampSpeed()
        {
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -settings.speed * Time.fixedDeltaTime, settings.speed * Time.fixedDeltaTime), rb.velocity.y);
        }

        /// <summary>
        /// Check if the object is on the floor
        /// </summary>
        /// <returns></returns>
        public bool IsGrounded()
        {
            if (!transform)
                return false;

            Vector3 pos = transform.position + Vector3.down * 1f;
            Vector3 scale = transform.localScale;
            RaycastHit2D hit = Physics2D.Raycast(pos + Vector3.up * settings.groundDistance, Vector2.down, settings.groundDistance * 2f, LayerMask.GetMask("Static", "Platform", "Default"));

            Debug.DrawLine(pos + Vector3.up * settings.groundDistance, pos + Vector3.down * settings.groundDistance, Color.red);

            return hit.collider;
        }

        private void CheckSound()
        {
            if (playFootstep == null)
                return;
            if (stopFootstep == null)
                return;

            if (IsGrounded())
            {
                if (!isPlayingSound)
                {
                    isPlayingSound = true;
                    playFootstep.Post(rb.gameObject);
                }
            }
            else
            {
                if (isPlayingSound)
                {
                    isPlayingSound = false;
                    stopFootstep.Post(rb.gameObject);
                }
            }
        }
    }
}