using Code.Scripts.States.Settings.Interfaces;
using UnityEngine;

namespace Code.Scripts.States.Settings
{
    /// <summary>
    /// Settings for the knockback block
    /// </summary>
    [CreateAssetMenu(fileName = "KnockbackBlockSettingsSO", menuName = "ScriptableObjects/StateSettings/KnockbackBlockSettings")]
    public class KnockbackBlockSettings : ScriptableObject, IKnockbackBlockSettings
    {
        public float knockbackForce;
    }
}
