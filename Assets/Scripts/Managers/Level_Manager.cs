using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Manager : MonoBehaviour
{
    //singleton
    public static Level_Manager _LEVEL_MANAGER;

    //variables
    private bool cappyOnWorld = false;  //stores if cappy is instantiated or not

    private void Awake()
    {
        if (_LEVEL_MANAGER != null && _LEVEL_MANAGER != this) Destroy(gameObject);
        else _LEVEL_MANAGER = this; //we destroy the level manager on load since every level has its own
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
}
