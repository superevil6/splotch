using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    //public PlayerColor playerColor;
    public PlayerColorManager playerColorManager;
    public SpriteRenderer Sprite;
    public Rigidbody2D Rigidbody2D;
    public Ball Ball;
    public Vector2 Ballsize;
    private RaycastHit2D[] HitUp;
    private RaycastHit2D[] HitDown;
    private RaycastHit2D[] HitLeft;
    private RaycastHit2D[] HitRight;
    // Start is called before the first frame update
    void Start()
    {
        Ballsize = Constants.FindOffset(Ball.gameObject);
        Sprite = GetComponent<SpriteRenderer>();
        this.transform.position = new Vector3(0, 0, 0);
        playerColorManager = GameObject.FindGameObjectWithTag("PlayerColor").GetComponent<PlayerColorManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerColorManager.ColorQueue.Count > 1){
            Sprite.color = Constants.SetColor(playerColorManager.ColorQueue[0]);
        }
        if(Input.GetKeyDown("up")){
            HitUp = Physics2D.RaycastAll(transform.position, Vector2.up, Ballsize.y * 2);
            if(HitUp.Length > 1 && HitUp[1].transform.gameObject.tag == "Ball"){
                transform.position = HitUp[1].transform.position;
            }
        }
        if(Input.GetKeyDown("down")){
            HitDown = Physics2D.RaycastAll(transform.position, -Vector2.up, Ballsize.y * 2);
            if(HitDown.Length > 1 && HitDown[1].transform.gameObject.tag == "Ball"){
                transform.position = HitDown[1].transform.position;
            }
        }
        if(Input.GetKeyDown("left")){
            HitLeft = Physics2D.RaycastAll(transform.position, -Vector2.right, Ballsize.x * 2);
            if(HitLeft.Length > 1 && HitLeft[1].transform.gameObject.tag == "Ball"){
                transform.position = HitLeft[1].transform.position;
            }
        }
        if(Input.GetKeyDown("right")){
            HitRight = Physics2D.RaycastAll(transform.position, Vector2.right, Ballsize.x * 2);
            if(HitRight.Length > 1 && HitRight[1].transform.gameObject.tag == "Ball"){
                transform.position = HitRight[1].transform.position;
            }
        }
    }

}

/*
Ideas:
Snap it to the 0, 0 of a game object so you don't have to calculate position or use hard coded values.

 */

