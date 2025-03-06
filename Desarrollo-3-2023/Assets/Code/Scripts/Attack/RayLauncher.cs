using Code.Scripts.Abstracts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayLauncher : MonoBehaviour, IShooter
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform spawnPos;

    private Vector2 direction;
    public Transform GetTransform()
    {
        return spawnPos.transform;
    }

    public void SetAim(Vector2 aim)
    {
        direction = aim;
    }

    public void Shoot()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Mathf.Infinity);

        if (hit)
        {
            Debug.Log(hit.rigidbody.name);
        }
        else
        {
            Debug.Log("nope");
        }
        //direction;
    }
}
