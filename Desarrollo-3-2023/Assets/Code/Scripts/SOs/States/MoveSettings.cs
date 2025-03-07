using UnityEngine;

namespace Code.SOs.States
{
    /// <summary>
    /// Settings for the move state
    /// </summary>
    [CreateAssetMenu(fileName = "MoveSettingsSO", menuName = "ScriptableObjects/StateSettings/MoveSettings")]
    public class MoveSettings : ScriptableObject
    {
        public float speed;
        public float accel;
        public float groundDistance;
    }
}