using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Manager : MonoBehaviour
{
    [SerializeField] private float distanceToTarget;
    private float axisY;
    private float axisX;
    [SerializeField] private GameObject target;
    [SerializeField] private float cameraLerp;
    [SerializeField] private bool useRaycast;

    private void LateUpdate()
    {
        axisX += Input_Manager._INPUT_MANAGER.GetRightAxisValue().y;
        axisY += Input_Manager._INPUT_MANAGER.GetRightAxisValue().x;
        axisX = Mathf.Clamp(axisX, -10f, 50f);

        transform.eulerAngles = new Vector3(axisX, axisY, 0);
        Vector3 finalPosition = Vector3.Lerp(transform.position, (target.transform.position + target.transform.up * 0.7f) - transform.forward * distanceToTarget, cameraLerp * Time.deltaTime);

        if (useRaycast)
        {
            RaycastHit hit;
            if (Physics.Linecast(target.transform.position, finalPosition, out hit) && hit.collider.tag != "Player") finalPosition = hit.point;
        }

        transform.position = finalPosition;
    }
}
