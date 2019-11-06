using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    //public PlayerColor playerColor;
    public PlayerColorManager playerColorManager;
    public PlayerManager PlayerManager;
    public GameBoard Gameboard;
    public RemoveColor RemoveColor;
    public SpriteRenderer SpriteRenderer;
    public Sprite Sprite;
    public Rigidbody2D Rigidbody2D;
    public Ball Ball;
    public Vector2 Ballsize;
    private RaycastHit2D[] HitUp;
    private RaycastHit2D[] HitDown;
    private RaycastHit2D[] HitLeft;
    private RaycastHit2D[] HitRight;
    private RaycastHit2D CurrentBall;
    private float WaitTimer;
    public float StartWait;
    public PlayerNumberManager PlayerNumberManager;
    private string PlayerPrefix;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefix = PlayerNumberManager.PlayerPrefix;
        Ballsize = Constants.FindOffset(Ball.gameObject);
        SpriteRenderer = GetComponent<SpriteRenderer>();
        SpriteRenderer.sprite = null;
        StartCoroutine(FindFirstBall(StartWait));
        //transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(playerColorManager.ColorQueue.Count > 1){
            SpriteRenderer.color = playerColorManager.SetColor(playerColorManager.ColorQueue[0]);
        }
        if(WaitTimer <= 0 && !PlayerManager.GameOver){
            //Up
            if(Input.GetAxis(PlayerPrefix + "Vertical") ==  -1f){
                HitUp = Physics2D.RaycastAll(transform.position, Vector2.up, Ballsize.y * 2);
                if(HitUp.Length > 1 && HitUp[1].transform.gameObject.tag == "Ball"){
                    transform.position = HitUp[1].transform.position;
                }
                WaitTimer = 0.15f;
            }
            //Down
            if(Input.GetAxis(PlayerPrefix + "Vertical") == 1f){
                HitDown = Physics2D.RaycastAll(transform.position, -Vector2.up, Ballsize.y * Gameboard.Rows);
                if(HitDown.Length >= 1 && HitDown[0].transform.gameObject.tag == "Ball"){
                    if(transform.position.y - HitDown[0].transform.position.y >= Ballsize.y / 4){
                        transform.position = HitDown[0].transform.position;
                    }
                    else if(HitDown.Length >= 2 && HitDown[1] && HitDown[1].transform.gameObject.tag == "Ball"){
                        transform.position = HitDown[1].transform.position;
                    }
                }
                WaitTimer = 0.15f;

            }
            //Left
            if(Input.GetAxis(PlayerPrefix + "Horizontal") == -1f){
                HitLeft = Physics2D.RaycastAll(transform.position, -Vector2.right, Ballsize.x * Gameboard.Columns);
                
                if(HitLeft.Length >= 1 && HitLeft[0].transform.gameObject.tag == "Ball"){
                    if(transform.position.x - HitLeft[0].transform.position.x >= Ballsize.x / 4){
                        transform.position = HitLeft[0].transform.position;
                    }
                    else if(HitLeft.Length >= 2 && HitLeft[1] && HitLeft[1].transform.gameObject.tag == "Ball"){
                        transform.position = HitLeft[1].transform.position;
                    }
                }
                WaitTimer = 0.15f;
            }
            //Right
            if(Input.GetAxis(PlayerPrefix + "Horizontal") == 1f){
                HitRight = Physics2D.RaycastAll(transform.position, Vector2.right, Ballsize.x * Gameboard.Columns);
                if(HitRight.Length >= 1 && HitRight[0].transform.gameObject.tag == "Ball"){
                    if(transform.position.x - HitRight[0].transform.position.x >= Ballsize.x / 4){
                        transform.position = HitRight[0].transform.position;
                    }
                    else if(HitRight.Length >= 2 && HitRight[1] && HitRight[1].transform.gameObject.tag == "Ball"){
                        transform.position = HitRight[1].transform.position;
                    }
                }
                WaitTimer = 0.15f;
            }
   
        }
        if(Input.GetButtonDown(PlayerPrefix + "Fire1")){
            CurrentBall = Physics2D.Raycast(transform.position, Vector2.zero, 0.1f);
            if(CurrentBall && CurrentBall.transform.gameObject.tag == "Ball" && LegalMoveCheck(CurrentBall.transform.GetComponent<Ball>().BallColor, playerColorManager.ColorQueue[0])){
                CurrentBall.transform.gameObject.GetComponent<Ball>().ChangeBallColor(playerColorManager.ColorQueue[0]);
                playerColorManager.UpdateColorQueue();
            }
        }
        if(Input.GetButtonDown(PlayerPrefix + "Fire2")){
            CurrentBall = Physics2D.Raycast(transform.position, Vector2.zero, 0.1f);
            if(CurrentBall && CurrentBall.transform.gameObject.tag == "Ball"){
                if(CurrentBall.transform.gameObject.GetComponent<Ball>().BallColor != BallColor.white && 
                RemoveColor.Uses > 0){
                    RemoveColor.UseRemoveColor(CurrentBall.transform.gameObject.GetComponent<Ball>());
                    StartCoroutine(RemoveColor.RegenerateRemoveColor(RemoveColor.RegenTime));
                }
            }
        }     
        if(WaitTimer > 0){
            WaitTimer -= Time.deltaTime;
        }
    }
    public IEnumerator FindFirstBall(float startTime){
        yield return new WaitForSeconds(startTime);
        SpriteRenderer.sprite = Sprite;
        transform.localPosition = new Vector2(Gameboard.transform.localPosition.x, -Gameboard.GameboardHeight + 10);
        print(-Gameboard.GameboardHeight * 2);
        HitRight = Physics2D.RaycastAll(transform.position, Vector2.right, Ballsize.x * 3);
        if(HitRight.Length > 1 && HitRight[1].transform.gameObject.tag == "Ball"){
            transform.position = HitRight[1].transform.position;
        }
        else if(HitRight.Length == 1 && HitRight[0].transform.gameObject.tag == "Ball"){
            transform.position = HitRight[0].transform.position;
        }
    }
    public bool LegalMoveCheck(BallColor ball, PlayerColor player){
        if(ball == BallColor.brown || ball == BallColor.black){
            return false;
        }
        else{
            bool verdict = false;
            switch(player){
                case PlayerColor.red:
                verdict = (ball != BallColor.red) ? true : false;
                break;
                case PlayerColor.blue:
                verdict = (ball != BallColor.blue) ? true : false;
                break;
                case PlayerColor.yellow:
                verdict = (ball != BallColor.yellow) ? true : false;
                break;
            }
            return verdict;
        }
    }
}
