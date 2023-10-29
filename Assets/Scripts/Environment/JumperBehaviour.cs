using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumperBehaviour : MonoBehaviour
{
    [SerializeField] private float appliedJumpForce;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerMovement>().JumpSecond();
        }
    }
}
