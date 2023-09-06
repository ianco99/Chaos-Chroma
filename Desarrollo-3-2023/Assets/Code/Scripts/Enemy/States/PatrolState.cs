using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.FSM;

public class PatrolState<T> : BaseState<T>
{
    private EnemySettings settings;
    private Rigidbody2D rb;
    private Vector3 currentDirection;
    public PatrolState(Rigidbody2D rb, EnemySettings settings, T id, string name) : base(id, name)
    {
        this.rb = rb;
        this.settings = settings;
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        currentDirection = rb.transform.position - settings.patrolPoints[0];
        Vector3 newForce = Vector3.Scale(currentDirection.normalized, settings.patrolSpeed);
        Debug.LogError("Current direction: " + newForce);
        rb.AddForce(newForce * Time.deltaTime, ForceMode2D.Impulse);
    }
}
