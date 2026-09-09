using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    private Vector2 MovementInput;
    private Rigidbody2D rg;

    void Awake()
    {
        rg = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Move();
        
    }

    void Move()
    {
        rg.linearVelocity = MovementInput * moveSpeed * Time.fixedDeltaTime;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MovementInput = context.ReadValue<Vector2>();
    }
}