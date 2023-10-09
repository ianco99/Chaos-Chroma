using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="AlertSettingsSO", menuName = "ScriptableObjects/StateSettings/AlertSettings")]
public class AlertSettings : ScriptableObject, IAlertSettings
{
    public float alertSpeed;
    public float alertAcceleration;
}
