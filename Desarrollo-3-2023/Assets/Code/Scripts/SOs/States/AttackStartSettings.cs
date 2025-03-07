using UnityEngine;

namespace Code.SOs.States
{
    /// <summary>
    /// Settings for the attack start state
    /// </summary>
    [CreateAssetMenu(fileName = "AttackStartSettingsSO", menuName = "ScriptableObjects/StateSettings/AttackStartSettings")]
    public class AttackStartSettings : ScriptableObject
    {
        public float minTimeOnHold;
        public Color objectiveColor;
    }
}
