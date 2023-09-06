using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.FSM;

public class PatrolState<T> : BaseState<T>
{
    private EnemySettings settings;
    private Transform patroller;
    private Vector3 currentDirection;
    public PatrolState(Transform patroller, EnemySettings settings, T id, string name) : base(id, name)
    {
        this.patroller = patroller;
        this.settings = settings;
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        currentDirection =  settings.patrolPoints[0] - patroller.position;
        Vector3 newDirection = Vector3.Scale(currentDirection.normalized, settings.patrolSpeed);
        Debug.LogError("Current direction: " + newDirection);
        patroller.Translate(newDirection * Time.deltaTime);
        //rb.AddForce(newForce * Time.deltaTime, ForceMode2D.Impulse);
    }
}
