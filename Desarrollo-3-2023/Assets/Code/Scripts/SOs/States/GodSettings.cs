using UnityEngine;

namespace Code.SOs.States
{
    /// <summary>
    /// Settings for the god state
    /// </summary>
    [CreateAssetMenu(fileName = "GodSettingsSO", menuName = "ScriptableObjects/StateSettings/GodSettings")]
    public class GodSettings : ScriptableObject
    {
        public MoveSettings moveSettings;
    }
}