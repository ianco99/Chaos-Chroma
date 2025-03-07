using UnityEngine;

namespace Code.SOs.States
{
    /// <summary>
    /// Settings for the parry state
    /// </summary>
    [CreateAssetMenu(fileName = "ParrySettingsSO", menuName = "ScriptableObjects/StateSettings/ParrySettings")]
    public class ParrySettings : ScriptableObject
    {
        public float duration;
    }
}
