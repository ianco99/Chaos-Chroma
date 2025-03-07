using Code.Scripts.Abstracts.Character;
using UnityEngine;

namespace Code.Scripts.Pickup
{
    /// <summary>
    /// Base class for pickups
    /// </summary>
    public abstract class BasePickup : MonoBehaviour, IPickup
    {
        public abstract void Pickup(Character character);
    }
}
