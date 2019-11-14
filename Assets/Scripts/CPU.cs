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
    private CPUActions action;
    private List<GameObject> SingleMatchingNeighbors = new List<GameObject>();
    private List<GameObject> MultipleMatchingNeighbors = new List<GameObject>();
    public GameBoard Gameboard;
    private List<GameObject> Hits;
    // Start is called before the first frame update
    void Start()
    {
        BallSize = Constants.FindOffset(Cursor.Ball.gameObject);
        Gameboard = Cursor.Gameboard;
        SetMovementSpeed(cpuDifficulty);
        action = CPUActions.FindBall;
        coolDownTime = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        if(coolDownTime <= 0){
            switch(action){
                case CPUActions.FindBall:
                FindSameColor();
                FindSameColorNeighbors(UsableBalls);
                chosenBall = PickBallToGoTo(MultipleMatchingNeighbors, SingleMatchingNeighbors);
                SingleMatchingNeighbors.Clear();
                MultipleMatchingNeighbors.Clear();
                action = CPUActions.Move;
                coolDownTime = cursorMovementSpeed;
                break;

                case CPUActions.WhiteOut:
                UseWhiteOut(chosenBall.GetComponent<Ball>());
                action = CPUActions.FindBall;
                coolDownTime = cursorMovementSpeed;
                break;

                case CPUActions.Move:
                float destX = Mathf.Round(chosenBall.transform.position.x);
                float destY = Mathf.Round(chosenBall.transform.position.y);
                MoveStepTowardsDestination(destX, destY);
                if(Mathf.Round(transform.position.x) == destX && Mathf.Round(transform.position.y) == destY){
                    if(chosenBall.GetComponent<Ball>().BallColor == BallColor.black){
                        print("It's a black ball");
                        action = CPUActions.WhiteOut;
                    }
                    else{
                        action = CPUActions.ChangeColor;
                    }
                }
                break;

                case CPUActions.ChangeColor:
                transform.position = chosenBall.transform.position;
                ChangeColor(chosenBall.GetComponent<Ball>());
                action = CPUActions.FindBall;
                coolDownTime = cursorMovementSpeed;
                break;

                default:
                break;
            }
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
            cursorMovementSpeed = 0.75f;
            break;
            case cpuDifficulty.Normal:
            cursorMovementSpeed = 0.50f;
            break;
            case cpuDifficulty.Hard:
            cursorMovementSpeed = 0.33f;
            break;
            case cpuDifficulty.VeryHard:
            cursorMovementSpeed = 0.15f;
            break;
        }
    }
    #endregion
    #region movement methods
    private void MoveStepTowardsDestination(float x, float y){
        bool moveSucceeded = false;
        if(Mathf.Round(x) > Mathf.Round(transform.position.x)){
            moveSucceeded = MoveRight();
            print(moveSucceeded);
        }
        if(Mathf.Round(x) < Mathf.Round(transform.position.x) && !moveSucceeded){
            moveSucceeded = MoveLeft();
            print(moveSucceeded);
        }
        if(Mathf.Round(y) > Mathf.Round(transform.position.y) && !moveSucceeded){
           moveSucceeded = MoveUp();
           print(moveSucceeded);
        }
        if(Mathf.Round(y) < Mathf.Round(transform.position.y) && !moveSucceeded){
            moveSucceeded = MoveDown();
            print(moveSucceeded);
        }


        coolDownTime = cursorMovementSpeed;
    }
    private bool MoveUp(){
        RaycastHit2D[] HitUp = Physics2D.RaycastAll(transform.position, Vector2.up, BallSize.y * 2);
        if(HitUp.Length > 1 && HitUp[1].transform.gameObject.tag == "Ball"){
            transform.position = HitUp[1].transform.position;
            return true;
        }
        return false;
    }
    private bool MoveDown(){
        RaycastHit2D[] HitDown = Physics2D.RaycastAll(transform.position, -Vector2.up, BallSize.y * Gameboard.Rows);
        if(HitDown.Length >= 1 && HitDown[0].transform.gameObject.tag == "Ball"){
            if(transform.position.y - HitDown[0].transform.position.y >= BallSize.y / 4){
                transform.position = HitDown[0].transform.position;
                return true;
            }
            if(HitDown.Length >= 2 && HitDown[1] && HitDown[1].transform.gameObject.tag == "Ball"){
                transform.position = HitDown[1].transform.position;
                return true;
            }
        }
        return false;
    }
    private bool MoveLeft(){
        RaycastHit2D[] HitLeft = Physics2D.RaycastAll(transform.position, -Vector2.right, BallSize.x * Gameboard.Columns);
        if(HitLeft.Length >= 1 && HitLeft[0].transform.gameObject.tag == "Ball"){
            if(transform.position.x - HitLeft[0].transform.position.x >= BallSize.x / 4){
                transform.position = HitLeft[0].transform.position;
                return true;
            }
            if(HitLeft.Length >= 2 && HitLeft[1] && HitLeft[1].transform.gameObject.tag == "Ball"){
                transform.position = HitLeft[1].transform.position;
                return true;
            }
        }
        return false;
    }
    private bool MoveRight(){
        RaycastHit2D[] HitRight = Physics2D.RaycastAll(transform.position, Vector2.right, BallSize.x * Gameboard.Columns);
        if(HitRight.Length >= 1 && HitRight[0].transform.gameObject.tag == "Ball"){
            if(transform.position.x - HitRight[0].transform.position.x >= BallSize.x / 4){
                transform.position = HitRight[0].transform.position;
                return true;
            }
            if(HitRight.Length >= 2 && HitRight[1] && HitRight[1].transform.gameObject.tag == "Ball"){
                transform.position = HitRight[1].transform.position;
                return true;
            }
        }
        return false;
    }
    #endregion
    #region action regions 
    private void ChangeColor(Ball CurrentBall){
        //CurrentBall = Physics2D.Raycast(transform.position, Vector2.zero, 0.1f);
        if(CurrentBall && CurrentBall.tag == "Ball" && Cursor.LegalMoveCheck(CurrentBall.BallColor, PlayerColorManager.ColorQueue[0])){
            CurrentBall.ChangeBallColor(PlayerColorManager.ColorQueue[0]);
            PlayerColorManager.UpdateColorQueue();
        }
    }
    private void UseWhiteOut(Ball ball){
        if(Cursor.RemoveColor.Uses > 0){
            Cursor.RemoveColor.UseRemoveColor(ball);
            StartCoroutine(Cursor.RemoveColor.RegenerateRemoveColor(Cursor.RemoveColor.RegenTime));
        }
    }
    private GameObject PickBallToGoTo(List<GameObject> MultipleMatchingNeighbors, List<GameObject> SingleMatchingNeighbors){
        //print(MultipleMatchingNeighbors.Count + " = multimatching");
        //print(SingleMatchingNeighbors.Count + " = singlematching");
        int singleOrMultiple = Random.Range(0, 1);
        GameObject ball = null;
        if(SingleMatchingNeighbors != null && MultipleMatchingNeighbors != null){
            if(singleOrMultiple == 0){
                ball = SingleMatchingNeighbors[Random.Range(0, SingleMatchingNeighbors.Count)]; 
            }
            else{
                ball = MultipleMatchingNeighbors[Random.Range(0, MultipleMatchingNeighbors.Count)];
            }

        }
        else if(SingleMatchingNeighbors.Count > 0 && MultipleMatchingNeighbors == null){
            ball = SingleMatchingNeighbors[Random.Range(0, SingleMatchingNeighbors.Count)]; 
        }
        else if(SingleMatchingNeighbors == null && MultipleMatchingNeighbors.Count > 0){
            ball = SingleMatchingNeighbors[Random.Range(0, SingleMatchingNeighbors.Count)]; 
        }
        //ball.transform.GetComponent<Ball>().SpriteRenderer.color = new Color(255, 0, 155, 255);
        return ball;
    }
    #endregion   
    #region detection methods
    public void FindSameColor(){
        UsableBalls.Clear();
        CurrentBalls.Clear();
        //Get the current color
        PlayerColor playerColor = PlayerColorManager.ColorQueue[0];
        BallColor ballColor = BallColor.white;;
        BallColor secondaryBallColor = BallColor.white;
        BallColor tertiaryBallColor = BallColor.white; 
        switch(playerColor){
            case PlayerColor.red:
            ballColor = BallColor.red;
            secondaryBallColor = BallColor.purple;
            tertiaryBallColor = BallColor.orange;
            break;
            case PlayerColor.blue:
            ballColor = BallColor.blue;
            secondaryBallColor = BallColor.green;
            tertiaryBallColor = BallColor.purple;
            break;
            case PlayerColor.yellow:
            ballColor = BallColor.yellow;
            secondaryBallColor = BallColor.orange;
            tertiaryBallColor = BallColor.green;
            break;
            default:
            break;
        }
        //Find all existing balls with the same color on the board.
        foreach(Ball ball in Gameboard.GetComponentsInChildren<Ball>()){
            if(ball.isActiveAndEnabled){
                CurrentBalls.Add(ball);
            }
        }
        foreach(Ball ball in CurrentBalls){
            if(ball.BallColor == BallColor.black && Cursor.RemoveColor.Uses > 0){
                chosenBall = ball.gameObject;
                action = CPUActions.Move;
                return;
            }
            if((ball.BallColor == ballColor || ball.BallColor == secondaryBallColor || ball.BallColor == tertiaryBallColor)){
                UsableBalls.Add(ball);
            }
        }
        //In case there are balls matching the player color at all.
        if(UsableBalls.Count <= 0){
            Ball randomWhiteBall = CurrentBalls[Random.Range(0, CurrentBalls.Count)];
            UsableBalls.Add(randomWhiteBall);
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
        //Primary Ball color will be white for RED BLUE AND YELLOW. If there is a secondary color for this ball, it'll check 
        BallColor primaryBallColor = BallColor.white;
        BallColor secondaryBallColor = BallColor.white;
        BallColor tertiaryBallColor = BallColor.white;
        //print("Direction length: " +  Direction.Length);
        if(Direction.Length == 4){
            Ball secondBall = Direction[1].transform.GetComponent<Ball>();
            Ball thirdBall = Direction[2].transform.GetComponent<Ball>();
            //print("Initial Check: BallColor: " + ball.BallColor.ToString() + " secondBallColor: " + secondBall.BallColor.ToString() + " thirdBallColor: " + thirdBall.BallColor.ToString());
            switch(ball.BallColor){
                case BallColor.orange:
                    secondaryBallColor = BallColor.yellow;
                    tertiaryBallColor = BallColor.red;
                break;
                case BallColor.purple:
                    secondaryBallColor = BallColor.red;
                    tertiaryBallColor = BallColor.blue;
                break;
                case BallColor.green:
                    secondaryBallColor = BallColor.blue;
                    tertiaryBallColor = BallColor.yellow;
                break;
                default:
                break;
            }
            //print("First Check " + primaryBallColor.ToString() + secondaryBallColor.ToString() + tertiaryBallColor.ToString());
            if(secondBall && secondBall.tag == "Ball" && secondBall.BallColor == ball.BallColor || secondBall.BallColor == primaryBallColor || secondBall.BallColor == secondaryBallColor || secondBall.BallColor == tertiaryBallColor){
                if(secondBall.BallColor == ball.BallColor){
                    sameBallColor += 1;
                }
                Hits.Add(secondBall.transform.gameObject);
            }
            //print("Second Check " + primaryBallColor.ToString() + secondaryBallColor.ToString() + tertiaryBallColor.ToString());
            if(thirdBall && thirdBall.tag == "Ball" && thirdBall.BallColor == ball.BallColor || thirdBall.BallColor == primaryBallColor || thirdBall.BallColor == secondaryBallColor || thirdBall.BallColor == tertiaryBallColor){
                if(thirdBall.BallColor == ball.BallColor){
                    sameBallColor += 1;
                }
                Hits.Add(thirdBall.transform.gameObject);
            }
            //print("Hits Count" + Hits.Count);

            if(Hits.Count > 0){
                foreach(GameObject go in Hits){
                    Ball chosenBall = go.GetComponent<Ball>();
                    if(chosenBall.BallColor == primaryBallColor || chosenBall.BallColor == secondaryBallColor || chosenBall.BallColor == tertiaryBallColor){
                        if(sameBallColor >= 1){
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
