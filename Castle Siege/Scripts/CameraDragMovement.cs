using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDragMovement : MonoBehaviour
{
    [Range(0.01f, 0.1f)]
    [SerializeField] float dragSpeed = 0.01f;
    [SerializeField] float touchZoomSpeed = 0.1f, mouseZoomSpeed = 15f;
    [SerializeField] float maxZoomDistance;
    [SerializeField] Transform initialPosition;
    [SerializeField] Transform[] boundaries;
    [SerializeField] VirtualJoystick js;
    [SerializeField] CameraObjToPlayer cotp;

    private void Start()
    {
        transform.position = initialPosition.position;
    }

    private void FixedUpdate()
    {
        if (js.gameObject.activeSelf || cotp.started) return;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (Input.touchCount == 1 && touch.phase == TouchPhase.Moved)
            {
                transform.position = new Vector3(
                    Mathf.Clamp(transform.position.x + -touch.deltaPosition.x * dragSpeed, boundaries[0].position.x, boundaries[1].position.x),
                    Mathf.Clamp(transform.position.y + -touch.deltaPosition.y * dragSpeed, boundaries[0].position.y, boundaries[1].position.y),
                    transform.position.z);
            }

            if (Input.touchCount == 2)
            {
                Touch tZero = Input.GetTouch(0);
                Touch tOne = Input.GetTouch(1);
                Vector2 tZeroPrevious = tZero.position - tZero.deltaPosition;
                Vector2 tOnePrevious = tOne.position - tOne.deltaPosition;

                float oldTouchDistance = Vector2.Distance(tZeroPrevious, tOnePrevious);
                float currentTouchDistance = Vector2.Distance(tZero.position, tOne.position);

                float deltaDistance = oldTouchDistance - currentTouchDistance;
                Zoom(deltaDistance, dragSpeed);
            }
        }

        else if (Application.isEditor)
        {
            Zoom(Input.GetAxis("Mouse ScrollWheel"), mouseZoomSpeed);
        }
    }

    void Zoom(float deltaMagnitideDiff, float speed)
    {
        transform.position = new Vector3(
            transform.position.x,
            transform.position.y,
            Mathf.Clamp(transform.position.z + deltaMagnitideDiff * speed, initialPosition.position.z, initialPosition.position.z + maxZoomDistance));
    }
}