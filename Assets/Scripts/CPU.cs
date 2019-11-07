using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class CPU : MonoBehaviour
{
    public PlayerManager PlayerManager;
    public PlayerColorManager PlayerColorManager;
    public Cursor Cursor;
    public cpuDifficulty cpuDifficulty;
    private float cursorMovementSpeed;
    private float coolDownTime;
    public Vector2 BallSize;
    // private RaycastHit2D[] HitUp;
    // private RaycastHit2D[] HitDown;
    // private RaycastHit2D[] HitLeft;
    // private RaycastHit2D[] HitRight;
    private RaycastHit2D CurrentBall;
    private List<Ball> UsableBalls = new List<Ball>();
    private List<Ball> CurrentBalls = new List<Ball>();
    private GameObject chosenBall;
    private bool atDestination;
    private bool ballChosen;
    private List<GameObject> SingleMatchingNeighbors = new List<GameObject>();
    private List<GameObject> MultipleMatchingNeighbors = new List<GameObject>();
    public GameBoard Gameboard;
    // private RaycastHit2D[] HitUpLeft;
    // private RaycastHit2D[] HitUpRight;
    // private RaycastHit2D[] HitDownLeft;
    // private RaycastHit2D[] HitDownRight;

    private List<GameObject> Hits;
    private List<GameObject> VerticalHits;
    private List<GameObject> HorizontalHits;
    private List<GameObject> DiagonalHitsULDR; //from upleft to downright
    private List<GameObject> DiagonalHitsURDL; //from upright to downleft
    // Start is called before the first frame update
    void Start()
    {
        BallSize = Constants.FindOffset(Cursor.Ball.gameObject);
        Gameboard = Cursor.Gameboard;
        SetMovementSpeed(cpuDifficulty.Normal);
        atDestination = false;
        ballChosen = false;
        coolDownTime = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        if(coolDownTime <= 0 && !ballChosen && !atDestination){
            print("THIS SHOULD ONLY HAPPEN ONCE!");
            FindSameColor();
            FindSameColorNeighbors(UsableBalls);
            chosenBall = PickBallToGoTo(MultipleMatchingNeighbors, SingleMatchingNeighbors);
            print("Chosen ball is here: " + chosenBall.transform.localPosition);
            ballChosen = true;
            SingleMatchingNeighbors.Clear();
            MultipleMatchingNeighbors.Clear();
            coolDownTime = cursorMovementSpeed;
        }
        if(coolDownTime <= 0 && ballChosen && !atDestination){
            MoveStepTowardsDestination(chosenBall.transform.position);
            print("Destination: " + chosenBall.transform.position);
            print("Cursor: " + Cursor.transform.position);
            coolDownTime = cursorMovementSpeed;
        }
        if(coolDownTime <= 0 && atDestination && !ballChosen){
            print("At destination");
            ChangeColor();
            coolDownTime = cursorMovementSpeed;
            ballChosen = false;
            atDestination = false;
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
    private void MoveStepTowardsDestination(Vector2 destination){
        if(destination.x > (Cursor.transform.position.x + BallSize.x)){
            MoveRight();
            return;
        }
        if(destination.x < (Cursor.transform.position.x - BallSize.x)){
            MoveLeft();
            return;
        }
        if(destination.y > (Cursor.transform.position.y + BallSize.y)){
            MoveUp();
            return;
        }
        if(destination.y < (Cursor.transform.position.y - BallSize.y)){
            MoveDown();
            return;
        }
        else{
            atDestination = true;
            ballChosen = false;
        }
        // if((destination.x - Cursor.transform.position.x <= 150) && (destination.y - Cursor.transform.position.y <= 150)){
        //     atDestination = true;
        // }
    }
    private void MoveUp(){
        RaycastHit2D[] HitUp = Physics2D.RaycastAll(transform.position, Vector2.up, BallSize.y * 2);
        if(HitUp.Length > 1 && HitUp[1].transform.gameObject.tag == "Ball"){
            transform.position = HitUp[1].transform.position;
        }
        coolDownTime = cursorMovementSpeed;
    }
    private void MoveDown(){
        RaycastHit2D[] HitDown = Physics2D.RaycastAll(transform.position, -Vector2.up, BallSize.y * Gameboard.Rows);
        if(HitDown.Length >= 1 && HitDown[0].transform.gameObject.tag == "Ball"){
            if(transform.position.y - HitDown[0].transform.position.y >= BallSize.y / 4){
                transform.position = HitDown[0].transform.position;
            }
            else if(HitDown.Length >= 2 && HitDown[1] && HitDown[1].transform.gameObject.tag == "Ball"){
                transform.position = HitDown[1].transform.position;
            }
        }
        coolDownTime = cursorMovementSpeed;
    }
    private void MoveLeft(){
        RaycastHit2D[] HitLeft = Physics2D.RaycastAll(transform.position, -Vector2.right, BallSize.x * Gameboard.Columns);
        if(HitLeft.Length >= 1 && HitLeft[0].transform.gameObject.tag == "Ball"){
            if(transform.position.x - HitLeft[0].transform.position.x >= BallSize.x / 4){
                transform.position = HitLeft[0].transform.position;
            }
            else if(HitLeft.Length >= 2 && HitLeft[1] && HitLeft[1].transform.gameObject.tag == "Ball"){
                transform.position = HitLeft[1].transform.position;
            }
        }
        coolDownTime = cursorMovementSpeed;
    }
    private void MoveRight(){
        RaycastHit2D[] HitRight = Physics2D.RaycastAll(transform.position, Vector2.right, BallSize.x * Gameboard.Columns);
        if(HitRight.Length >= 1 && HitRight[0].transform.gameObject.tag == "Ball"){
            if(transform.position.x - HitRight[0].transform.position.x >= BallSize.x / 4){
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
    private GameObject PickBallToGoTo(List<GameObject> MultipleMatchingNeighbors, List<GameObject> SingleMatchingNeighbors){
        print(MultipleMatchingNeighbors.Count + " = multimatching");
        print(SingleMatchingNeighbors.Count + " = singlematching");
        int singleOrMultiple = Random.Range(0, 1);
        if(singleOrMultiple == 0){
            return SingleMatchingNeighbors[Random.Range(0, SingleMatchingNeighbors.Count)]; 
        }
        else{
            return MultipleMatchingNeighbors[Random.Range(0, MultipleMatchingNeighbors.Count)];
        }
    }
    #endregion   
    #region detection methods
    public void FindSameColor(){
        UsableBalls.Clear();
        CurrentBalls.Clear();
        //Get the current color
        PlayerColor playerColor = PlayerColorManager.ColorQueue[0];
        BallColor ballColor;
        switch(playerColor){
            case PlayerColor.red:
            ballColor = BallColor.red;
            break;
            case PlayerColor.blue:
            ballColor = BallColor.blue;
            break;
            case PlayerColor.yellow:
            ballColor = BallColor.yellow;
            break;
            default:
            ballColor = BallColor.white;
            break;
        }
        //Find all existing balls with the same color on the board.
        foreach(Ball ball in Gameboard.GetComponentsInChildren<Ball>()){
            CurrentBalls.Add(ball);
        }
        foreach(Ball ball in CurrentBalls){
            if(ball.BallColor == ballColor && ball.isActiveAndEnabled){
                UsableBalls.Add(ball);
            }
        }
    }
    public void FindSameColorNeighbors(List<Ball> balls){
        foreach(Ball ball in balls){
            CheckForMatches(ball);
        }
    }
    public void CheckForMatches(Ball ball){
		RaycastHit2D[] HitUp = Physics2D.RaycastAll(ball.transform.position, Vector2.up, BallSize.x * 2);
		RaycastHit2D[] HitDown = Physics2D.RaycastAll(ball.transform.position, -Vector2.up, BallSize.x * 2);
		RaycastHit2D[] HitLeft = Physics2D.RaycastAll(ball.transform.position, -Vector2.right, BallSize.y * 2);
		RaycastHit2D[] HitRight = Physics2D.RaycastAll(ball.transform.position, Vector2.right, BallSize.y * 2);
		RaycastHit2D[] HitUpLeft = Physics2D.RaycastAll(ball.transform.position, new Vector2(-1, 1), (BallSize.x * BallSize.y) * 2);
		RaycastHit2D[] HitUpRight = Physics2D.RaycastAll(ball.transform.position, new Vector2(1, 1), (BallSize.x * BallSize.y) * 2);
		RaycastHit2D[] HitDownLeft = Physics2D.RaycastAll(ball.transform.position, new Vector2(-1, -1), (BallSize.x * BallSize.y) * 2);
		RaycastHit2D[] HitDownRight = Physics2D.RaycastAll(ball.transform.position, new Vector2(1, -1), (BallSize.x * BallSize.y) * 2);
        CheckForMatchingNeighbors(ball, HitUp);
        CheckForMatchingNeighbors(ball, HitDown);
        CheckForMatchingNeighbors(ball, HitLeft);
        CheckForMatchingNeighbors(ball, HitRight);
        CheckForMatchingNeighbors(ball, HitUpLeft);
        CheckForMatchingNeighbors(ball, HitDownRight);
        CheckForMatchingNeighbors(ball, HitUpRight);
        CheckForMatchingNeighbors(ball, HitDownLeft);

	}
    private void CheckForMatchingNeighbors(Ball ball, RaycastHit2D[] Direction){
        List<GameObject> Hits = new List<GameObject>();
        int sameBallColor = 0;
        if(Direction.Length == 3){
            if(Direction[1].transform.gameObject.tag == "Ball" && (Direction[1].transform.GetComponent<Ball>().BallColor == ball.BallColor || Direction[1].transform.GetComponent<Ball>().BallColor == BallColor.white)){
                print("direction length" + Direction.Length);
                print("First ball: " + Direction[1].transform.GetComponent<Ball>().BallColor);
                if(Direction[1].transform.GetComponent<Ball>().BallColor == ball.BallColor){
                    sameBallColor += 1;
                }
                Hits.Add(Direction[1].transform.gameObject);
            }
            if(Direction[2].transform.gameObject.tag == "Ball" && (Direction[2].transform.GetComponent<Ball>().BallColor == ball.BallColor || Direction[2].transform.GetComponent<Ball>().BallColor == BallColor.white)){
                print("Second ball: " + Direction[2].transform.GetComponent<Ball>().BallColor);
                if(Direction[2].transform.GetComponent<Ball>().BallColor == ball.BallColor){
                    sameBallColor += 1;
                }
                Hits.Add(Direction[2].transform.gameObject);
            }
            print("Same Color: " + sameBallColor.ToString());
            if(Hits.Count > 0){
                foreach(GameObject go in Hits){
                    if(go.GetComponent<Ball>().BallColor == BallColor.white){
                        if(sameBallColor == 1){
                            MultipleMatchingNeighbors.Add(go);
                        }
                        if(sameBallColor == 0){
                            SingleMatchingNeighbors.Add(go);
                        }
                    }
                }
            }
            sameBallColor = 0;
        }
    }
    #endregion
}
