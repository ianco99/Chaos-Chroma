using Code.Scripts.Abstracts.Character;
using Code.Scripts.States;
using Code.SOs.Enemy;
using System;
using Code.Scripts.SOs.States;
using UnityEngine;

namespace Patterns.FSM
{
    /// <summary>
    /// Handler for state
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PatrolState<T> : MovementState<T>
    {
        private readonly PatrolSettings settings;
        private Transform groundCheckPoint;
        private Character patroller;

        public PatrolState(Rigidbody2D rb, T id, Transform groundCheckPoint, Character patroller, Transform transform,
            PatrolSettings settings) : base(id, settings.moveSettings,
            transform, rb)
        {
            this.settings = settings;
            this.groundCheckPoint = groundCheckPoint;
            this.patroller = patroller;

            SetDirection(1f);
        }

        public PatrolState(Rigidbody2D rb, T id, string name, Transform groundCheckPoint, Character patroller,
            Transform transform, PatrolSettings settings) : base(id, name, settings.moveSettings,
            transform, rb)
        {
            this.settings = settings;
            this.groundCheckPoint = groundCheckPoint;
            this.patroller = patroller;

            SetDirection(1f);
        }

        public override void OnUpdate()
        {
            CheckGround();
            CheckWall();
        }

        /// <summary>
        /// Checks if the character is grounded and if there is no ground within a certain distance.
        /// If the character is grounded and there is no ground, flips the character's orientation.
        /// </summary>
        private void CheckGround()
        {
            if (!IsGrounded())
                return;

            RaycastHit2D hit = Physics2D.Raycast(groundCheckPoint.position, -groundCheckPoint.up,
                settings.groundCheckDistance, LayerMask.GetMask("Static", "Platform"));

            if (!hit)
            {
                FlipDirection();
            }
        }

        /// <summary>
        /// Checks for a wall in the direction the character is facing.
        /// </summary>
        /// <remarks>
        /// If the character is grounded and detects a wall within a certain distance,
        /// the character's orientation is flipped to avoid collision.
        /// </remarks>
        private void CheckWall()
        {
            if (!IsGrounded())
                return;

            RaycastHit2D hit = Physics2D.Raycast(groundCheckPoint.position, groundCheckPoint.right * dir,
                settings.wallCheckDistance, LayerMask.GetMask("Static", "Platform"));

            if (hit && hit.transform.name != patroller.name)
            {
                FlipDirection();
            }
        }

        /// <summary>
        /// Sets the direction of the patrol state.
        /// </summary>
        /// <param name="newDirection">The direction to set the patrol state to.</param>
        /// <remarks>
        /// The direction is a float value where positive values indicate a right direction and
        /// negative values indicate a left direction. 0 indicates no direction.
        /// </remarks>
        public void SetDirection(float newDirection)
        {
            dir.x = newDirection;
        }

        /// <summary>
        /// Flips the direction of the patrol state.
        /// </summary>
        /// <remarks>
        /// This method is used to flip the direction of the character when there is no ground
        /// within a certain distance.
        /// </remarks>
        public void FlipDirection()
        {
            dir = -dir;
        }
    }
}