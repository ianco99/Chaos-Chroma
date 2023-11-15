using Code.Scripts.Abstracts.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifePickup : BasePickup
{
    [SerializeField] private float lifeBump = 100;

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

        character.lifePickup?.Invoke(lifeBump);

        Destroy(gameObject);
    }
}
