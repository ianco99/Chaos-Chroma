using Code.Scripts.Input;
using UnityEngine;

public class MoveState : MonoBehaviour
{
    [SerializeField] private float speed = 50f;
    
    private float movement;

    private void OnEnable()
    {
        InputManager.onMove += UpdateMovement;
    }

    private void OnDisable()
    {
        InputManager.onMove -= UpdateMovement;
    }

    private void Update()
    {
        MoveInDirection(movement);
    }

    private void UpdateMovement(float dir)
    {
        movement = dir;
    }

    private void MoveInDirection(float dir)
    {
        transform.Translate(Vector3.right * (dir * speed * Time.deltaTime));
    }
}