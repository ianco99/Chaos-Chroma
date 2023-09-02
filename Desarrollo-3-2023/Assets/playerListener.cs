using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kuznickiEventChannel;

public class playerListener : MonoBehaviour
{
    [SerializeField] private FloatEventChannel onMoveChannel;
    [SerializeField] private VoidEventChannel onAttackChannel;
    [SerializeField] private VoidEventChannel onBlockChannel;
    [SerializeField] private VoidEventChannel onJumpChannel;

    private void Start()
    {
        onMoveChannel.Subscribe(OnMove);
        onAttackChannel.Subscribe(OnAttack);
        onBlockChannel.Subscribe(OnBlock);
        onJumpChannel.Subscribe(OnJump);
    }

    private void OnMove(float axis)
    {
        print("Eu topo, me quiero mover en axis: " + axis);
    }

    private void OnAttack()
    {
        print("Eu topo, quiero atacar");
    }

    private void OnBlock()
    {
        print("Eu topo, quiero bloquear");
    }

    private void OnJump()
    {
        print("Eu topo, quiero saltar");
    }
}
