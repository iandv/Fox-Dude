    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRecoil : MonoBehaviour
{
    public float rotationSpeed = 6f;
    public float returnSpeed = 25f;

    public Vector3 recoilRotation = new Vector3(2f, 2f, 2f);

    private Vector3 currentRotation;
    private Vector3 rot;

    private void FixedUpdate()
    {
        currentRotation = Vector3.Lerp(currentRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        rot = Vector3.Slerp(rot, currentRotation, rotationSpeed * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(rot);
    }

    public void Recoil()
    {
        currentRotation += new Vector3(-recoilRotation.x, Random.Range(-recoilRotation.y, recoilRotation.y), Random.Range(-recoilRotation.z, recoilRotation.z));
    }
}
