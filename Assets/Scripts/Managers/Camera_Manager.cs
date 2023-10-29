using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Manager : MonoBehaviour
{
    [SerializeField] private float distanceToTarget;
    private Vector2 AxisValue;
    [SerializeField] private GameObject target;
    [SerializeField] private float cameraLerp;

    private void LateUpdate()
    {
        AxisValue.x += Input_Manager._INPUT_MANAGER.GetRightAxisValue().y;
        AxisValue.y += Input_Manager._INPUT_MANAGER.GetRightAxisValue().x;
        AxisValue.x = Mathf.Clamp(AxisValue.x, -10f, 50f);

        transform.eulerAngles = new Vector3(AxisValue.x, AxisValue.y, 0);
        transform.position = Vector3.Lerp(transform.position, target.transform.position - transform.forward * distanceToTarget, cameraLerp * Time.deltaTime);
    }
}
