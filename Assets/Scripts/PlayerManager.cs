using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameBoard gameBoard;
    public ObjectPooler objectPooler;
    public DetectorManager detectorManager;
    public ScoreManager ScoreManager;
    public PlayerColorManager PlayerColorManager;
    public PlayerNumber PlayerNumber;
    public Difficulty Difficulty;
    public PlayerColor IgnoredPlayerColor;
    public BallColor IgnoredBallColor;
    public GameObject GameOverPanel;
    public int[] WeightedBallColorPool;
    //Weighted Values are processed in this order, White, BLACK BROWN GREEN PURPLE ORANGE RED BLUE YELLOW
    public int[] DefaultColorWeights = {0, 0, 10, 10, 10, 10, 30, 30, 30};
    public List<PlayerColor> PlayerColorPool;
    public float DropSpeed;
    public float DropSpeedIncrease;
    public bool GameOver = false;
    public int Score;
	public float Time;
	public int ScoreMultiplier;
	//This is the amount of time you have to continue your rensa.
	public float AllotedRensaTime = 1.5f;
	public float RensaTime;
	public bool RensaCheck = false;
    public PlayerNumberManager PlayerNumberManager;
    private string PlayerPrefix;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefix = PlayerNumberManager.PlayerPrefix;
        GameOverPanel.SetActive(false);
        
        switch(Difficulty){
            case Difficulty.VeryEasy: 
            DropSpeed = 1;
            IgnoredPlayerColor = PlayerColorManager.GeneratePlayerColor();
            IgnoredBallColor = MatchIgnoredColors(IgnoredPlayerColor);
            break;
            
            case Difficulty.Easy:
            DropSpeed = 1.5f;
            IgnoredPlayerColor = PlayerColorManager.GeneratePlayerColor();
            IgnoredBallColor = MatchIgnoredColors(IgnoredPlayerColor);
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
        DetermineColorPool();

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown(PlayerPrefix + "Fire3")){
            DropSpeed += DropSpeedIncrease;
        }
        if(Input.GetButtonUp(PlayerPrefix + "Fire3")){
            DropSpeed -= DropSpeedIncrease;
        }
        if(GameOver){
            GameOverPanel.SetActive(true);
            DropSpeed = 0;
        }
    }
    public void DetermineColorPool(){
        Array PlayerColors = Enum.GetValues(typeof(PlayerColor));
        foreach(PlayerColor color in PlayerColors){
            if(color != IgnoredPlayerColor){
                PlayerColorPool.Add(color);
            }
        }
        //Weighted Values are processed in this order, 
        //0WHITE 1BLACK 2BROWN 3GREEN 4PURPLE 5ORANGE 6RED 7BLUE 8YELLOW, 
        //if it doesn't proc any of those, it's white
        WeightedBallColorPool = DefaultColorWeights;
        switch(IgnoredBallColor){
            case BallColor.red:
            //Ignore Red, Orange, and Purple
            WeightedBallColorPool[6] = 0;
            WeightedBallColorPool[5] = 0;
            WeightedBallColorPool[4] = 0;
            break;
            case BallColor.blue:
            //Ignore Blue, Purple, and Green
            WeightedBallColorPool[7] = 0;
            WeightedBallColorPool[4] = 0;
            WeightedBallColorPool[3] = 0;
            break;
            case BallColor.yellow:
            //Ignore Yellow, Orange, and Green
            WeightedBallColorPool[8] = 0;
            WeightedBallColorPool[5] = 0;
            WeightedBallColorPool[3] = 0;
            break;
        }
    }
    public BallColor MatchIgnoredColors(PlayerColor color){
        switch(color){
            case PlayerColor.red:
            return BallColor.red;
            case PlayerColor.blue:
            return BallColor.blue;
            case PlayerColor.yellow:
            return BallColor.yellow;
            default:
            return BallColor.white;
        }
    }
}
