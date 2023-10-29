using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CappyBehaviour : MonoBehaviour
{
    [SerializeField] private float maxDistance;
    [SerializeField] private float velocity;
    [SerializeField] private float lifeSpan;
    private Vector3 target;
    private bool moving = true;
    [SerializeField] private float appliedJumpForce;

    private void Awake()
    {
        target = transform.position + transform.forward * maxDistance;
        Destroy(gameObject, lifeSpan);
    }

    private void Update()
    {
        if (moving) transform.position = Vector3.Lerp(transform.position, target, velocity * Time.deltaTime);
    }

    private void OnDestroy()
    {
        Level_Manager._LEVEL_MANAGER.setCappySituation(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) other.gameObject.GetComponent<PlayerMovement>().JumpSecond();
    }
}
