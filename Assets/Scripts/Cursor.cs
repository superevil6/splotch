using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    //public PlayerColor playerColor;
    public PlayerColorManager playerColorManager;
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
    // Start is called before the first frame update
    void Start()
    {
        Ballsize = Constants.FindOffset(Ball.gameObject);
        Sprite = GetComponent<SpriteRenderer>();
        this.transform.position = new Vector3(0, 0, 0);
        playerColorManager = GameObject.FindGameObjectWithTag("PlayerColor").GetComponent<PlayerColorManager>();
        string[] joysticks = Input.GetJoystickNames();
        foreach(string js in joysticks){
            print(js);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(playerColorManager.ColorQueue.Count > 1){
            Sprite.color = Constants.SetColor(playerColorManager.ColorQueue[0]);
        }
        if(WaitTimer <= 0){
            //Up
            if(Input.GetAxis("Vertical") ==  -1f){
                print("Derop");
                HitUp = Physics2D.RaycastAll(transform.position, Vector2.up, Ballsize.y * 2);
                if(HitUp.Length > 1 && HitUp[1].transform.gameObject.tag == "Ball"){
                    transform.position = HitUp[1].transform.position;
                }
                WaitTimer += 0.15f;
            }
            //Down
            if(Input.GetAxis("Vertical") == 1f){
                print("input detected");
                HitDown = Physics2D.RaycastAll(transform.position, -Vector2.up, Ballsize.y * 2);
                if(HitDown.Length > 1 && HitDown[1].transform.gameObject.tag == "Ball"){
                    transform.position = HitDown[1].transform.position;
                }
                WaitTimer += 0.15f;

            }
            //Left
            if(Input.GetAxis("Horizontal") == -1f){
                HitLeft = Physics2D.RaycastAll(transform.position, -Vector2.right, Ballsize.x * 2);
                if(HitLeft.Length > 1 && HitLeft[1].transform.gameObject.tag == "Ball"){
                    transform.position = HitLeft[1].transform.position;
                }
                WaitTimer += 0.15f;
            }
            //Right
            if(Input.GetAxis("Horizontal") == 1f){
                HitRight = Physics2D.RaycastAll(transform.position, Vector2.right, Ballsize.x * 2);
                if(HitRight.Length > 1 && HitRight[1].transform.gameObject.tag == "Ball"){
                    transform.position = HitRight[1].transform.position;
                }
                WaitTimer += 0.15f;
            }
            if(Input.GetButton("Fire1")){
                CurrentBall = Physics2D.Raycast(transform.position, Vector2.zero, 0.1f);
                if(CurrentBall && CurrentBall.transform.gameObject.tag == "Ball"){
                    CurrentBall.transform.gameObject.GetComponent<Ball>().ChangeBallColor(playerColorManager.ColorQueue[0]);
                }
                WaitTimer += 0.15f;
            }
            if(Input.GetButton("Fire2")){
                print("Remove Color");
                CurrentBall = Physics2D.Raycast(transform.position, Vector2.zero, 0.1f);
                if(CurrentBall && CurrentBall.transform.gameObject.tag == "Ball"){
                    if(CurrentBall.transform.gameObject.GetComponent<Ball>().BallColor != BallColor.white && 
                    RemoveColor.Uses > 0){
                        RemoveColor.UseRemoveColor(CurrentBall.transform.gameObject.GetComponent<Ball>());
                        StartCoroutine(RemoveColor.RegenerateRemoveColor(RemoveColor.RegenTime));
                    }
                }
                WaitTimer += 0.15f;
            }        }
        if(WaitTimer > 0){
            WaitTimer -= Time.deltaTime;
        }

    }

}

/*
Ideas:
Snap it to the 0, 0 of a game object so you don't have to calculate position or use hard coded values.

 */

