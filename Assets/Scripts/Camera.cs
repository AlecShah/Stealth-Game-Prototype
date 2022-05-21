using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{

    [SerializeField] private float mouseSensitivity;

    private Transform parent;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        parent = transform.parent;
    }
    void Update()
    {
        Rotate();
    }
    private void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

        parent.Rotate(Vector3.up, mouseX);
    }
}
