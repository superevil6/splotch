using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameBoard gameBoard;
    public ObjectPooler objectPooler;
    public DetectorManager detectorManager;
    public PlayerColorManager playerColorManager;
    public PlayerNumber playerNumber;
    public float DropSpeed;
    public float DropSpeedIncrease;
    public bool GameOver = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire3")){
            DropSpeed += DropSpeedIncrease;
        }
        if(Input.GetButtonUp("Fire3")){
            DropSpeed -= DropSpeedIncrease;
        }
        if(GameOver){
            DropSpeed = 0;
        }
    }
}
