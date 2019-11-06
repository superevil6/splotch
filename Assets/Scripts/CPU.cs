using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class CPU : MonoBehaviour
{

    public PlayerManager PlayerManager;
    public Cursor Cursor;
    public cpuDifficulty cpuDifficulty;
    private float cursorMovementSpeed;
    private float coolDownTime;
    public Vector2 Ballsize;
    private RaycastHit2D[] HitUp;
    private RaycastHit2D[] HitDown;
    private RaycastHit2D[] HitLeft;
    private RaycastHit2D[] HitRight;
    private RaycastHit2D CurrentBall;
    public GameBoard Gameboard;
    // Start is called before the first frame update
    void Start()
    {
        Ballsize = Constants.FindOffset(Cursor.Ball.gameObject);
        Gameboard = Cursor.Gameboard;
        SetMovementSpeed(cpuDifficulty.Normal);
    }

    // Update is called once per frame
    void Update()
    {
        if(coolDownTime <= 0){
            //do stuff
            //MoveRight();
            //MoveUp();
            //ChangeColor();
        }    
        else{
            coolDownTime -= Time.deltaTime;
        }
    }
    #region setup methods
    private void SetMovementSpeed(cpuDifficulty cpuDifficulty){
        switch(cpuDifficulty){
            case cpuDifficulty.VeryEasy:
            cursorMovementSpeed = 1f;
            break;
            case cpuDifficulty.Easy:
            cursorMovementSpeed = 0.85f;
            break;
            case cpuDifficulty.Normal:
            cursorMovementSpeed = 0.70f;
            break;
            case cpuDifficulty.Hard:
            cursorMovementSpeed = 0.5f;
            break;
            case cpuDifficulty.VeryHard:
            cursorMovementSpeed = 0.33f;
            break;
        }
    }
    #endregion
    #region movement methods
    private void MoveUp(){
        print("Starting move up");
        HitUp = Physics2D.RaycastAll(transform.position, Vector2.up, Ballsize.y * 2);
        if(HitUp.Length > 1 && HitUp[1].transform.gameObject.tag == "Ball"){
            print("There is a hit");
            transform.position = HitUp[1].transform.position;
        }
        coolDownTime = cursorMovementSpeed;
    }
    private void MoveDown(){
        HitDown = Physics2D.RaycastAll(transform.position, -Vector2.up, Ballsize.y * Gameboard.Rows);
            if(HitDown.Length >= 1 && HitDown[0].transform.gameObject.tag == "Ball"){
                if(transform.position.y - HitDown[0].transform.position.y >= Ballsize.y / 4){
                    transform.position = HitDown[0].transform.position;
                }
                else if(HitDown.Length >= 2 && HitDown[1] && HitDown[1].transform.gameObject.tag == "Ball"){
                    transform.position = HitDown[1].transform.position;
                }
        }
        coolDownTime = cursorMovementSpeed;
    }
    private void MoveLeft(){
        HitLeft = Physics2D.RaycastAll(transform.position, -Vector2.right, Ballsize.x * Gameboard.Columns);
        if(HitLeft.Length >= 1 && HitLeft[0].transform.gameObject.tag == "Ball"){
            if(transform.position.x - HitLeft[0].transform.position.x >= Ballsize.x / 4){
                transform.position = HitLeft[0].transform.position;
            }
            else if(HitLeft.Length >= 2 && HitLeft[1] && HitLeft[1].transform.gameObject.tag == "Ball"){
                transform.position = HitLeft[1].transform.position;
            }
        }
        coolDownTime = cursorMovementSpeed;
    }
    private void MoveRight(){
        HitRight = Physics2D.RaycastAll(transform.position, Vector2.right, Ballsize.x * Gameboard.Columns);
        if(HitRight.Length >= 1 && HitRight[0].transform.gameObject.tag == "Ball"){
            if(transform.position.x - HitRight[0].transform.position.x >= Ballsize.x / 4){
                transform.position = HitRight[0].transform.position;
            }
            else if(HitRight.Length >= 2 && HitRight[1] && HitRight[1].transform.gameObject.tag == "Ball"){
                transform.position = HitRight[1].transform.position;
            }
        }
        coolDownTime = cursorMovementSpeed;
    }
    #endregion
    #region action regions 
    private void ChangeColor(){
        CurrentBall = Physics2D.Raycast(transform.position, Vector2.zero, 0.1f);
        if(CurrentBall && CurrentBall.transform.gameObject.tag == "Ball" && Cursor.LegalMoveCheck(CurrentBall.transform.GetComponent<Ball>().BallColor, Cursor.playerColorManager.ColorQueue[0])){
            CurrentBall.transform.gameObject.GetComponent<Ball>().ChangeBallColor(Cursor.playerColorManager.ColorQueue[0]);
            Cursor.playerColorManager.UpdateColorQueue();
        }
    }
    private void UseWhiteOut(){
        CurrentBall = Physics2D.Raycast(transform.position, Vector2.zero, 0.1f);
        if(CurrentBall && CurrentBall.transform.gameObject.tag == "Ball"){
            if(CurrentBall.transform.gameObject.GetComponent<Ball>().BallColor != BallColor.white && 
            Cursor.RemoveColor.Uses > 0){
                Cursor.RemoveColor.UseRemoveColor(CurrentBall.transform.gameObject.GetComponent<Ball>());
                StartCoroutine(Cursor.RemoveColor.RegenerateRemoveColor(Cursor.RemoveColor.RegenTime));
            }
        }
    }
    #endregion   
    #region detection methods

    #endregion
}
