using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouseLook : EntityDeath
{
    [SerializeField]
    protected float mouseSensitivity = 100f;

    [SerializeField]
    protected Transform playerBody;

    float xRotation = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (!dead)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -80f, 80f);
            transform.localRotation = Quaternion.Euler(xRotation, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);

            playerBody.Rotate(Vector3.up * mouseX);
        }
        
    }
}
