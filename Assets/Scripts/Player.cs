using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] InputActionReference moveRef;
    [SerializeField] Rigidbody rigidbody;
    [SerializeField] float moveSpeed = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        moveRef.action.Enable();
    }
    void Start()
    {
        
    }

    void Update()
    {
        SetMove(moveRef.action.ReadValue<Vector2>());
    }

    void SetMove(Vector2 input)
    {
        Vector3 direction = new Vector3(input.x, 0, input.y).normalized;

        rigidbody.linearVelocity = direction * moveSpeed;
    }
}
