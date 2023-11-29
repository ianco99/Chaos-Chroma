using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KnockbackBlockSettingsSO", menuName = "ScriptableObjects/StateSettings/KnockbackBlockSettings")]
public class KnockbackBlockSettings : ScriptableObject, IKnockbackBlockSettings
{
    public float knockbackForce;
}
