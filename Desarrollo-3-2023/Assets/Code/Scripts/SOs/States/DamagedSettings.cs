using UnityEngine;

namespace Code.SOs.States
{
    /// <summary>
    /// Settings for the damaged state
    /// </summary>
    [CreateAssetMenu(fileName = "DamagedSettingsSO", menuName = "ScriptableObjects/StateSettings/DamagedSettings")]
    public class DamagedSettings : ScriptableObject
    {
        public TimerSettings timerSettings;
        public float force;
    }
}
