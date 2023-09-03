using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.FSM;

public class MovementState<T> : BaseState<T>
{
    public MovementState(T id) : base(id)
    {

    }

    public MovementState(T id, string name) : base(id, name)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.LogError("Entered Move");
    }
}
