using UnityEngine;
using Patterns.FSM;

public class JumpState<T> : BaseState<T>
{
    public JumpState(T id) : base(id)
    {

    }

    public JumpState(T id, string name) : base(id, name)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.LogError("Entered Jump");
    }
}
