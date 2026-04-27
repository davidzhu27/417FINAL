using UnityEngine;
using UnityEngine.InputSystem;

public class SimplePlayerMover : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float mouseSensitivity = 0.15f;

    private float rotationY = 0f;

    void Update()
    {
        Vector2 moveInput = Vector2.zero;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed) moveInput.y += 1;
            if (Keyboard.current.sKey.isPressed) moveInput.y -= 1;
            if (Keyboard.current.aKey.isPressed) moveInput.x -= 1;
            if (Keyboard.current.dKey.isPressed) moveInput.x += 1;
        }

        if (Mouse.current != null)
        {
            float mouseX = Mouse.current.delta.ReadValue().x * mouseSensitivity;
            rotationY += mouseX;
            transform.rotation = Quaternion.Euler(0f, rotationY, 0f);
        }

        Vector3 move =
            transform.forward * moveInput.y +
            transform.right * moveInput.x;

        transform.position += move * moveSpeed * Time.deltaTime;
    }
}