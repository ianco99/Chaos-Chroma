using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kuznickiEventChannel;

public class InputEventSystem : MonoBehaviourSingleton<InputEventSystem>
{
    [SerializeField] private FloatEventChannel onMoveChannel;
    [SerializeField] private VoidEventChannel onAttackChannel;
    [SerializeField] private VoidEventChannel onBlockChannel;
    [SerializeField] private VoidEventChannel onJumpChannel;

    public FloatEventChannel OnMoveChannel { get => onMoveChannel; private set => onMoveChannel = value; }
    public VoidEventChannel OnAttackChannel { get => onAttackChannel; private set => onAttackChannel = value; } 
    public VoidEventChannel OnBlockChannel { get => onBlockChannel; private set => onBlockChannel = value; }
    public VoidEventChannel OnJumpChannel { get => onJumpChannel; private set => onJumpChannel = value; }


    private void Start()
    {
        InputManager.onMove += MoveCharacters;
        InputManager.onAttack += AttackAttackers;
        InputManager.onBlock += BlockBlockers;
        InputManager.onJump += JumpJumpers;
    }

    private void MoveCharacters(float axis)
    {
        onMoveChannel.RaiseEvent(axis);
    }

    private void AttackAttackers()
    {
        onAttackChannel.RaiseEvent();
    }
    private void BlockBlockers()
    {
        onBlockChannel.RaiseEvent();
    }

    private void JumpJumpers()
    {
        onJumpChannel.RaiseEvent();
    }
}
