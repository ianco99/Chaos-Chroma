using UnityEngine;

[CreateAssetMenu(fileName = "PatrolSettingsSO", menuName = "ScriptableObjects/StateSettings/PatrolSettings")]
public class PatrolSettings : ScriptableObject, IPatrolSettings
{
    public float patrolSpeed;
    public float patrolAcceleration;

    [Header("GroundCheck")]
    public float groundCheckDistance;
    public LayerMask groundLayer;

    [Header("WallCheck")]
    public LayerMask wallLayer;
    public float wallCheckDistance;
}
