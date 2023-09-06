using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "EnemySettings", menuName = "ScriptableObjects/EnemySettings")]

public class EnemySettings : ScriptableObject
{
    public Vector3[] patrolPoints;
}
