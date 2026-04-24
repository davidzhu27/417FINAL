using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    public InputActionReference jumpAction;
    public float jumpForce = 5f;
    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        jumpAction.action.Enable();

        jumpAction.action.performed += (ctx) =>
        {
            Jump();
        };
    }

    void Jump()
    {
        if (characterController == null)
            return;

        // Keep behavior lightweight: a single upward move on jump press.
        // This avoids Rigidbody physics while still "jumping" the XR rig.
        characterController.Move(Vector3.up * jumpForce * Time.deltaTime);
    }
}