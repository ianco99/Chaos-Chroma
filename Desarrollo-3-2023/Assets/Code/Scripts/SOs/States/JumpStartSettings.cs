using UnityEngine;
using UnityEngine.Serialization;

namespace Code.SOs.States
{
    /// <summary>
    /// Settings for the jump start state
    /// </summary>
    [CreateAssetMenu(fileName = "JumpStartSettingsSO", menuName = "ScriptableObjects/StateSettings/JumpStartSettings")]
    public class JumpStartSettings : ScriptableObject
    {
        [FormerlySerializedAs("jumpForce")] public float force;
        public MoveSettings moveSettings;
    }
}
