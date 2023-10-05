using System;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Serializable]
    enum TransformFlip
    {
        None,
        X,
        Y,
        Z
    }
    
    [Serializable]
    struct Flippable
    {
        public Transform trans;
        public SpriteRenderer sprite;
        public TransformFlip transformFlip;
    }
    
    [Header("Character:")]
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected SpriteRenderer sprite;
    [SerializeField] protected bool facingRight = true;
    [SerializeField] private List<Flippable> flippables;
    
    protected virtual void Flip()
    {
        foreach (var flipped in flippables)
        {
            Vector2 pos = flipped.trans.localPosition;

            pos.x *= -1;
            flipped.trans.localPosition = pos;
            
            if (flipped.sprite)
                flipped.sprite.flipX = !flipped.sprite.flipX;
            
            switch (flipped.transformFlip)
            {
                case TransformFlip.None:
                    break;
                
                case TransformFlip.X:
                    flipped.trans.Rotate(transform.right, 180);
                    break;
                
                case TransformFlip.Y:
                    flipped.trans.Rotate(transform.up, 180);
                    break;
                
                case TransformFlip.Z:
                    flipped.trans.Rotate(transform.forward, 180);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        facingRight = !facingRight;
        sprite.flipX = !sprite.flipX;
    }
}