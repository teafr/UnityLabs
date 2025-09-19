using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 1f;
    [SerializeField]
    private float jumpForce = 1f;

    private Rigidbody rb;
    private InputUserActions inputActions;
    private Vector2 moveInput;
    private bool jumpPressed;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        inputActions = new InputUserActions();

        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        inputActions.Player.Jump.performed += ctx => jumpPressed = true;
    }

    void OnEnable() => inputActions.Enable();
    void OnDisable() => inputActions.Disable();

    void FixedUpdate()
    {
        Vector3 direction = new Vector3(moveInput.x, 0, moveInput.y);
        Vector3 velocity = direction * moveSpeed;
        Vector3 move = transform.position + velocity * Time.fixedDeltaTime;
        rb.MovePosition(move);

        if (jumpPressed && Mathf.Abs(rb.linearVelocity.y) < 0.01f)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        jumpPressed = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ColorZone"))
        {
            GetComponent<Renderer>().material.color = Random.ColorHSV();
        }
    }
}
