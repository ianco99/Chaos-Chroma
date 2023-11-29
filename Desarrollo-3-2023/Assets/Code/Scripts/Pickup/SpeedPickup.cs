using Code.Scripts.Abstracts.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPickup : BasePickup
{
    [SerializeField] private float speedBump = 100;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character character;

        collision.TryGetComponent(out character);
        
        if (character != null)
            Pickup(character);
    }

    public override void Pickup(Character character)
    {
        base.Pickup(character);

        character.speedPickup?.Invoke(speedBump);

        Destroy(gameObject);
    }
}
