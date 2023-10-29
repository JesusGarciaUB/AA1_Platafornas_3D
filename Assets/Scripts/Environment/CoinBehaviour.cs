using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
    [SerializeField] private float spinVelocity; //spin velocity in angles per second

    private void Awake()
    {
        Level_Manager._LEVEL_MANAGER.coinSpawned();
    }
    private void Update()
    {
        transform.Rotate(0, spinVelocity * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Level_Manager._LEVEL_MANAGER.coinTaken();
    }
}
