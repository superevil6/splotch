using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class PlayerNumberManager : MonoBehaviour
{
    public PlayerManager PlayerManager;
    public PlayerNumber PlayerNumber;
    public string PlayerPrefix;
    // Start is called before the first frame update
    void Start()
    {
        switch(PlayerManager.PlayerNumber){
            case PlayerNumber.one:
            PlayerPrefix = "P1";
            break;
            case PlayerNumber.two:
            PlayerPrefix = "P2";
            break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
