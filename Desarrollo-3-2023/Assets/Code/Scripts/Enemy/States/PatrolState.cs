using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.FSM;

public class PatrolState<T> : BaseState<T>
{
    private Rigidbody2D rb;
    public Vector2 currentVelocity;
    public PatrolState(Rigidbody2D rb, T id, string name) : base(id, name)
    {
        this.rb = rb;
    }

    public override void OnEnter()
    {
        base.OnEnter();

    }
    public override void OnUpdate()
    {
        base.OnUpdate();

        rb.AddForce(Vector2.right * 2);
        currentVelocity = rb.velocity;
    }
}
