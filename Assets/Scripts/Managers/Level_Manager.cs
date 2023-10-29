using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Manager : MonoBehaviour
{
    //singleton
    public static Level_Manager _LEVEL_MANAGER;

    //variables
    private bool cappyOnWorld = false;  //stores if cappy is instantiated or not
    private float numberOfCoins = 0;
    private bool coinsInitialized = false;

    private void Awake()
    {
        if (_LEVEL_MANAGER != null && _LEVEL_MANAGER != this) Destroy(gameObject);
        else _LEVEL_MANAGER = this; //we destroy the level manager on load since every level has its own
    }

    private void Update()
    {
        if (coinsInitialized && numberOfCoins == 0)
        {
            Debug.Log("molt be");
        }
    }

    //getters & setters
    public bool getCappySituation()
    {
        return this.cappyOnWorld;
    }

    public void setCappySituation(bool value)
    {
        cappyOnWorld = value;
    }

    public void coinTaken()
    {
        numberOfCoins--;
    }

    public void coinSpawned()
    {
        numberOfCoins++;
        coinsInitialized = true;
    }
}
