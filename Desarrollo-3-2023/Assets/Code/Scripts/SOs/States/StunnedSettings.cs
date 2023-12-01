using Code.SOs.States;
using UnityEngine;

namespace Code.Scripts.SOs.States
{
    [CreateAssetMenu(fileName = "StunnedSettingsSO", menuName = "ScriptableObjects/StateSettings/StunnedSettings")]
    public class StunnedSettings : ScriptableObject
    {
        public TimerSettings timerSettings;
    }
}
