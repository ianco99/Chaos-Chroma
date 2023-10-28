using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="AlertSettingsSO", menuName = "ScriptableObjects/StateSettings/AlertSettings")]
public class AlertSettings : ScriptableObject, IAlertSettings
{
    public float alertSpeed;
    public float alertAcceleration;
    public float alertAttackDistance;

    [Header("GroundCheck")]
    public float groundCheckDistance;
    public LayerMask groundLayer;
}
