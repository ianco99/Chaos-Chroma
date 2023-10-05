using Code.Scripts.States;
using Code.SOs.Enemy;
using System;
using UnityEngine;

namespace Patterns.FSM
{
    /// <summary>
    /// Handler for state
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DamagedState<T> : BaseState<T>
    {
        Rigidbody2D rb;
        public DamagedState( T id, string name, Transform transform, Rigidbody2D rb) : base(id, name)
        {
            this.rb = rb;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log("Damaged");
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

        }
    }
}