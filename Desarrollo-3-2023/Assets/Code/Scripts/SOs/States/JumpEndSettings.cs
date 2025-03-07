using UnityEngine;

namespace Code.SOs.States
{
    /// <summary>
    /// Settings for the jump end state
    /// </summary>
    [CreateAssetMenu(fileName = "JumpEndSettingsSO", menuName = "ScriptableObjects/StateSettings/JumpEndSettings")]
    public class JumpEndSettings : ScriptableObject
    {
        public float gravMultiplier;
        public MoveSettings moveSettings;
    }
}
