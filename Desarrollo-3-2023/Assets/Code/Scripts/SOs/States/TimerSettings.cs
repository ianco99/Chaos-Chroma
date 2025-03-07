using UnityEngine;

namespace Code.SOs.States
{
    /// <summary>
    /// Settings for the timer
    /// </summary>
    [CreateAssetMenu(fileName = "TimerSettingsSO", menuName = "ScriptableObjects/StateSettings/TimerSettings")]
    public class TimerSettings : ScriptableObject
    {
        public float maxTime;
    }
}
