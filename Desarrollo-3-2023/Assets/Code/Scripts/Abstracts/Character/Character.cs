using System;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Serializable]
    struct Flippable
    {
        public Transform trans;
        public SpriteRenderer sprite;
    }


    [Header("Character:")]
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected SpriteRenderer sprite;
    [SerializeField] private List<Flippable> flippables;

    protected bool facingRight;

    protected void Flip()
    {
        foreach (var flipped in flippables)
        {
            Vector2 pos = flipped.trans.position;

            pos.x *= -1;
            flipped.trans.position = pos;
            flipped.sprite.flipX = !flipped.sprite.flipX;
        }

        sprite.flipX = !sprite.flipX;
    }
}