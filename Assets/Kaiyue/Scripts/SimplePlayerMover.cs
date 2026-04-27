using UnityEngine;
using UnityEngine.InputSystem;

public class SimplePlayerMover : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float mouseSensitivity = 0.12f;

    private float rotationY = 0f;

    void Update()
    {
        Vector3 move = Vector3.zero;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed) move += transform.forward;
            if (Keyboard.current.sKey.isPressed) move -= transform.forward;
            if (Keyboard.current.aKey.isPressed) move -= transform.right;
            if (Keyboard.current.dKey.isPressed) move += transform.right;
        }

        if (Mouse.current != null)
        {
            float mouseX = Mouse.current.delta.ReadValue().x * mouseSensitivity;
            rotationY += mouseX;
            transform.rotation = Quaternion.Euler(0f, rotationY, 0f);
        }

        transform.position += move * moveSpeed * Time.deltaTime;
    }
}