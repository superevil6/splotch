using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameBoard gameBoard;
    public ObjectPooler objectPooler;
    public DetectorManager detectorManager;
    public PlayerColorManager PlayerColorManager;
    public PlayerNumber PlayerNumber;
    public Difficulty Difficulty;
    public PlayerColor IgnoredColor;
    public float DropSpeed;
    public float DropSpeedIncrease;
    public bool GameOver = false;
    // Start is called before the first frame update
    void Start()
    {
        switch(Difficulty){
            case Difficulty.VeryEasy: 
            DropSpeed = 1;
            IgnoredColor = PlayerColorManager.GeneratePlayerColor();
            break;
            
            case Difficulty.Easy:
            DropSpeed = 1.5f;
            IgnoredColor = PlayerColorManager.GeneratePlayerColor();
            break;

            case Difficulty.Normal:
            DropSpeed = 2f;
            break;

            case Difficulty.Hard:
            DropSpeed = 2.5f;
            break;

            case Difficulty.VeryHard:
            DropSpeed = 3f;
            break;
        }
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
