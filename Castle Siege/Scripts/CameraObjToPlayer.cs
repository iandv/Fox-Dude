using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraObjToPlayer : MonoBehaviour
{
    public bool started;

    [SerializeField]
    float maxSpeed, distanceForMaxSpeed, startTime;
    [SerializeField]
    Transform pTransform;

    private bool isMoving;

    void Start()
    {
        started = true;
        StartCoroutine(AwakeCoroutine());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isMoving) return;

        float distance = Vector3.Distance(pTransform.position, transform.position);
        if (distance > 0.1f)
        {
            Vector3 dir = (pTransform.position - transform.position).normalized;

            if (distance > distanceForMaxSpeed)
            {
                transform.position += Time.deltaTime * maxSpeed * dir * distance / distanceForMaxSpeed;
            }
            else
            {
                transform.position += dir * maxSpeed * Time.deltaTime;
            }
        }

        if (distance <= 0.1f)
        {
            transform.position = pTransform.position;
            isMoving = false;
            started = false;
        }
    }

    IEnumerator AwakeCoroutine()
    {
        yield return new WaitForSeconds(startTime);
        isMoving = true;
    }
}
