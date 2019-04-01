using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    //public PlayerColor playerColor;
    public PlayerColorManager playerColorManager;
    public PlayerManager PlayerManager;
    public RemoveColor RemoveColor;
    public SpriteRenderer Sprite;
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
    // Start is called before the first frame update
    void Start()
    {
        Ballsize = Constants.FindOffset(Ball.gameObject);
        Sprite = GetComponent<SpriteRenderer>();
        StartCoroutine(FindFirstBall(StartWait));
        //transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(playerColorManager.ColorQueue.Count > 1){
            Sprite.color = playerColorManager.SetColor(playerColorManager.ColorQueue[0]);
        }
        if(WaitTimer <= 0 && !PlayerManager.GameOver){
            //Up
            if(Input.GetAxis("Vertical") ==  -1f){
                HitUp = Physics2D.RaycastAll(transform.position, Vector2.up, Ballsize.y * 2);
                if(HitUp.Length > 1 && HitUp[1].transform.gameObject.tag == "Ball"){
                    transform.position = HitUp[1].transform.position;
                }
                else if(HitUp.Length == 1 && HitUp[0].transform.gameObject.tag == "Ball"){
                    transform.position = HitUp[0].transform.position;
                }
                WaitTimer = 0.15f;
            }
            //Down
            if(Input.GetAxis("Vertical") == 1f){
                HitDown = Physics2D.RaycastAll(transform.position, -Vector2.up, Ballsize.y * 3);
                if(HitDown.Length > 1 && HitDown[1].transform.gameObject.tag == "Ball"){
                    transform.position = HitDown[1].transform.position;
                }
                else if(HitDown.Length == 1 && HitDown[0].transform.gameObject.tag == "Ball"){
                    transform.position = HitDown[0].transform.position;
                }
                WaitTimer = 0.15f;

            }
            //Left
            if(Input.GetAxis("Horizontal") == -1f){
                HitLeft = Physics2D.RaycastAll(transform.position, -Vector2.right, Ballsize.x * 3);
                if(HitLeft.Length > 1 && HitLeft[1].transform.gameObject.tag == "Ball"){
                    transform.position = HitLeft[1].transform.position;
                }
                else if(HitLeft.Length == 1 && HitLeft[0].transform.gameObject.tag == "Ball"){
                    transform.position = HitLeft[0].transform.position;
                }
                WaitTimer = 0.15f;
            }
            //Right
            if(Input.GetAxis("Horizontal") == 1f){
                HitRight = Physics2D.RaycastAll(transform.position, Vector2.right, Ballsize.x * 3);
                if(HitRight.Length > 1 && HitRight[1].transform.gameObject.tag == "Ball"){
                    transform.position = HitRight[1].transform.position;
                }
                else if(HitRight.Length == 1 && HitRight[0].transform.gameObject.tag == "Ball"){
                    transform.position = HitRight[0].transform.position;
                }
                WaitTimer = 0.15f;
            }
   
        }
        if(Input.GetButtonDown("Fire1")){
            CurrentBall = Physics2D.Raycast(transform.position, Vector2.zero, 0.1f);
            if(CurrentBall && CurrentBall.transform.gameObject.tag == "Ball"){
                CurrentBall.transform.gameObject.GetComponent<Ball>().ChangeBallColor(playerColorManager.ColorQueue[0]);
                playerColorManager.UpdateColorQueue();
            }
        }
        if(Input.GetButtonDown("Fire2")){
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
        HitRight = Physics2D.RaycastAll(transform.position, Vector2.right, Ballsize.x * 3);
        if(HitRight.Length > 1 && HitRight[1].transform.gameObject.tag == "Ball"){
            transform.position = HitRight[1].transform.position;
        }
        else if(HitRight.Length == 1 && HitRight[0].transform.gameObject.tag == "Ball"){
            transform.position = HitRight[0].transform.position;
        }
    }
}
