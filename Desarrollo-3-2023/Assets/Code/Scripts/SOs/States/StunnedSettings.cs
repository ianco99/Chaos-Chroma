using Code.SOs.States;
using UnityEngine;

namespace Code.Scripts.SOs.States
{
    /// <summary>
    /// Settings for the stunned state
    /// </summary>
    [CreateAssetMenu(fileName = "StunnedSettingsSO", menuName = "ScriptableObjects/StateSettings/StunnedSettings")]
    public class StunnedSettings : ScriptableObject
    {
        public TimerSettings timerSettings;
    }
}
