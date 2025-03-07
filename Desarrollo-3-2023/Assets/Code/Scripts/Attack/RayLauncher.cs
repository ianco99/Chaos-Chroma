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

            //if (!col.gameObject.TryGetComponent(out Damageable damageable)) return;
            //
            //if (!damageable.TakeDamage(settings.damage, transform.position))
            //{
            //    rb.velocity *= -1f;
            //    return;
            //}

        }
        else
        {
            Debug.Log("nope");
        }

        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, spawnPos.position);
        lineRenderer.SetPosition(1, direction*10);
        StopAllCoroutines();
        StartCoroutine(DisappearRay());
        //direction;
    }

    private IEnumerator DisappearRay()
    {
        yield return new WaitForSeconds(0.1f);
        lineRenderer.enabled = false;
    }
}
