using System;
using UnityEngine;

namespace Code.Scripts.SOs.Level
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Ship", fileName = "Ship", order = 0)]
    public class ShipVariants : ScriptableObject
    {
        [Serializable] public struct Range
        {
            [SerializeField] private float min;
            [SerializeField] private float max;

            public Range(float min, float max)
            {
                this.min = min;
                this.max = max;
            }
            
            public float Get() => UnityEngine.Random.Range(min, max);
        }
        
        public const float DestroyPos = 100f;
        
        public Sprite sprite;
        public Range position = new(0f, 100f);
        public Range speed = new(10f, 20f);
    }
}