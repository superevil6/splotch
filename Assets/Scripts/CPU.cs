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
    private RaycastHit2D CurrentBall;
    private List<Ball> UsableBalls = new List<Ball>();
    private List<Ball> CurrentBalls = new List<Ball>();
    private GameObject chosenBall;
    private CPUActions action;
    private List<GameObject> SingleMatchingNeighbors = new List<GameObject>();
    private List<GameObject> MultipleMatchingNeighbors = new List<GameObject>();
    public GameBoard Gameboard;
    private List<GameObject> Hits;
    private List<Detector> Detectors;
    private string PlayerPrefix;
    // Start is called before the first frame update
    void Start()
    {
        //BallSize = Constants.FindOffset(Cursor.Ball.gameObject);
        Gameboard = Cursor.Gameboard;
        Detectors = PlayerManager.detectorManager.VerticalDetectors;
        PlayerPrefix = PlayerManager.PlayerNumberManager.PlayerPrefix;
        SetMovementSpeed(cpuDifficulty);
        if(Gameboard.ObjectPooler.PooledItems.Count > 0){
            BallSize = Gameboard.ObjectPooler.PooledItems[0].transform.localScale;
        }
        action = CPUActions.FindBall;
        coolDownTime = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        if(coolDownTime <= 0 && !PlayerManager.GameOver){
            switch(action){
                case CPUActions.CheckColumns:
                print("Checvking COlumns");
                foreach(Detector detector in Detectors){
                    if(detector.BallCount()){
                        print("Mkae brown");
                        action = CPUActions.MakeBrown;
                        break;
                    }
                }
                print(action + " is the action");
                //action = CPUActions.FindBall;
                break;

                case CPUActions.FindBall:
                FindSameColor();
                FindSameColorNeighbors(UsableBalls);
                chosenBall = PickBallToGoTo(MultipleMatchingNeighbors, SingleMatchingNeighbors);
                SingleMatchingNeighbors.Clear();
                MultipleMatchingNeighbors.Clear();
                if(chosenBall != null){
                    action = CPUActions.Move;
                }
                coolDownTime = cursorMovementSpeed;
                break;

                case CPUActions.WhiteOut:
                UseWhiteOut(chosenBall.GetComponent<Ball>());
                action = CPUActions.FindBall;
                coolDownTime = cursorMovementSpeed;
                break;

                case CPUActions.Move:
                if(chosenBall != null){
                    float destX = Mathf.Round(chosenBall.transform.position.x);
                    float destY = Mathf.Round(chosenBall.transform.position.y);
                    MoveStepTowardsDestination(destX, destY);
                    if(Mathf.Round(transform.position.x) == destX && Mathf.Round(transform.position.y) == destY){
                        if(chosenBall.GetComponent<Ball>().BallColor == BallColor.black){
                            action = CPUActions.WhiteOut;
                        }
                        else{
                            action = CPUActions.ChangeColor;
                        }
                    }
                    break;
                }
                else{
                    action = CPUActions.FindBall;
                    break;
                }

                case CPUActions.ChangeColor:
                transform.position = chosenBall.transform.position;
                ChangeColor(chosenBall.GetComponent<Ball>());
                action = CPUActions.CheckColumns;
                coolDownTime = cursorMovementSpeed;
                break;

                case CPUActions.MakeBrown:
                chosenBall = MakeABrownBall();
                action = CPUActions.Move;
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
        if(Mathf.Round(y) > Mathf.Round(transform.position.y)){
           moveSucceeded = MoveUp();
           if(!moveSucceeded){
                if(Mathf.Round(x) > Mathf.Round(transform.position.x) && !moveSucceeded){
                    moveSucceeded = MoveRight();
                }
                else{
                    moveSucceeded = MoveLeft();
                }
           }
        }
        if(Mathf.Round(y) < Mathf.Round(transform.position.y) && !moveSucceeded){
            moveSucceeded = MoveDown();
        }
        if(Mathf.Round(x) > Mathf.Round(transform.position.x) && !moveSucceeded){
            moveSucceeded = MoveRight();
            if(!moveSucceeded){
                    if(Mathf.Round(x) > Mathf.Round(transform.position.x) && !moveSucceeded){
                        moveSucceeded = MoveDown();
                    }
                    else{
                        moveSucceeded = MoveUp();
                    }
            }
        }
        if(Mathf.Round(x) < Mathf.Round(transform.position.x) && !moveSucceeded){
            moveSucceeded = MoveLeft();
            if(!moveSucceeded){
                    if(Mathf.Round(x) > Mathf.Round(transform.position.x) && !moveSucceeded){
                        moveSucceeded = MoveDown();
                    }
                    else{
                        moveSucceeded = MoveUp();
                    }
            }
        }
        coolDownTime = cursorMovementSpeed;
    }
    private bool MoveUp(){
        RaycastHit2D[] HitUp = Physics2D.RaycastAll(transform.position, Vector2.up, 100, 1 << 8);
        if(HitUp.Length > 1 && HitUp[1].transform.gameObject.tag == "Ball" + PlayerPrefix){
            transform.position = HitUp[1].transform.position;
            return true;
        }
        return false;
    }
    private bool MoveDown(){
        RaycastHit2D[] HitDown = Physics2D.RaycastAll(transform.position, -Vector2.up, 100, 1 << 8); // Gameboard.Rows);
        if(HitDown.Length >= 1 && HitDown[0].transform.gameObject.tag == "Ball" + PlayerPrefix){
            if(transform.position.y - HitDown[0].transform.position.y >= BallSize.y / 4){
                transform.position = HitDown[0].transform.position;
                return true;
            }
            if(HitDown.Length >= 2 && HitDown[1] && HitDown[1].transform.gameObject.tag == "Ball" + PlayerPrefix){
                transform.position = HitDown[1].transform.position;
                return true;
            }
        }
        return false;
    }
    private bool MoveLeft(){
        RaycastHit2D[] HitLeft = Physics2D.RaycastAll(transform.position, -Vector2.right, 100, 1 << 8); // Gameboard.Columns);
        if(HitLeft.Length >= 1 && HitLeft[0].transform.gameObject.tag == "Ball" + PlayerPrefix){
            if(transform.position.x - HitLeft[0].transform.position.x >= BallSize.x / 4){
                transform.position = HitLeft[0].transform.position;
                return true;
            }
            if(HitLeft.Length >= 2 && HitLeft[1] && HitLeft[1].transform.gameObject.tag == "Ball" + PlayerPrefix){
                transform.position = HitLeft[1].transform.position;
                return true;
            }
        }
        return false;
    }
    private bool MoveRight(){
        RaycastHit2D[] HitRight = Physics2D.RaycastAll(transform.position, Vector2.right, 100, 1 << 8); // Gameboard.Columns);
        if(HitRight.Length >= 1 && HitRight[0].transform.gameObject.tag == "Ball" + PlayerPrefix){
            if(transform.position.x - HitRight[0].transform.position.x >= BallSize.x / 4){
                transform.position = HitRight[0].transform.position;
                return true;
            }
            if(HitRight.Length >= 2 && HitRight[1] && HitRight[1].transform.gameObject.tag == "Ball" + PlayerPrefix){
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
        if(CurrentBall && CurrentBall.tag == "Ball" + PlayerPrefix && Cursor.LegalMoveCheck(CurrentBall.BallColor, PlayerColorManager.ColorQueue[0])){
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
        int singleOrMultiple = Random.Range(0, 1);
        GameObject ball = null;
        if(SingleMatchingNeighbors != null && MultipleMatchingNeighbors != null){
            if(singleOrMultiple == 0 && SingleMatchingNeighbors.Count > 1){
                ball = SingleMatchingNeighbors[Random.Range(0, SingleMatchingNeighbors.Count)]; 
            }
            else if(singleOrMultiple == 1 && MultipleMatchingNeighbors.Count > 1){
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
        CheckDirectionForMatchingNeighbors(action, ball, Physics2D.RaycastAll(ball.transform.position, Vector2.up, 100, 1 << 8));
        CheckDirectionForMatchingNeighbors(action, ball, Physics2D.RaycastAll(ball.transform.position, -Vector2.up, 100, 1 << 8));
        CheckDirectionForMatchingNeighbors(action, ball, Physics2D.RaycastAll(ball.transform.position, -Vector2.right, 100, 1 << 8));
        CheckDirectionForMatchingNeighbors(action, ball, Physics2D.RaycastAll(ball.transform.position, Vector2.right, 100, 1 << 8));
        CheckDirectionForMatchingNeighbors(action, ball, Physics2D.RaycastAll(ball.transform.position, new Vector2(-1, 1), 100, 1 << 8));
        CheckDirectionForMatchingNeighbors(action, ball, Physics2D.RaycastAll(ball.transform.position, new Vector2(1, 1), 100, 1 << 8));
        CheckDirectionForMatchingNeighbors(action, ball, Physics2D.RaycastAll(ball.transform.position, new Vector2(-1, -1), 100, 1 << 8));
        CheckDirectionForMatchingNeighbors(action, ball, Physics2D.RaycastAll(ball.transform.position, new Vector2(1, -1), 100, 1 << 8));
	}
    private void CheckDirectionForMatchingNeighbors(CPUActions cpuAction, Ball ball, RaycastHit2D[] direction){
        if(direction.Length > 0){
            switch (cpuAction){
                case CPUActions.FindBall:
                    CheckForMatchingNeighbors(ball, direction);
                break;
                case CPUActions.MakeBrown:
                    CheckForBrownNeighbors(ball, direction);
                break;
                default:
                break;
            }
        }
    }
    private void CheckForBrownNeighbors(Ball ball, RaycastHit2D[] Direction){
        List<GameObject> Hits = new List<GameObject>();
        int brownBalls = 0;
        Ball secondBall = null; // = Direction[1].transform.GetComponent<Ball>();
        Ball thirdBall = null; // = Direction[2].transform.GetComponent<Ball>();
        if(Direction.Length >= 2){
            secondBall = Direction[1].transform.GetComponent<Ball>();
            if(secondBall.BallColor == BallColor.brown){
                if(secondBall.BallColor == ball.BallColor){
                    brownBalls += 1;
                }
                Hits.Add(secondBall.transform.gameObject);
            }
        }
        if(Direction.Length >= 3){
            thirdBall = Direction[2].transform.GetComponent<Ball>();
            if(thirdBall.BallColor == BallColor.brown){
                if(thirdBall.BallColor == ball.BallColor){
                    brownBalls += 1;
                }
                Hits.Add(thirdBall.transform.gameObject);
            }
        }
        if(Hits.Count > 0){
            foreach(GameObject go in Hits){
                Ball chosenBall = go.GetComponent<Ball>();
                if((chosenBall.BallColor != BallColor.brown && chosenBall.BallColor != Constants.PlayerColorToBallColor(PlayerColorManager.ColorQueue[0]) && chosenBall.tag == "Ball" + PlayerPrefix)){
                    if(brownBalls >= 1){
                        MultipleMatchingNeighbors.Add(go);
                    }
                    if(brownBalls == 0){
                        SingleMatchingNeighbors.Add(go);
                    }
                }

            }
        }
        brownBalls = 0;
    }
    private void CheckForMatchingNeighbors(Ball ball, RaycastHit2D[] Direction){
        List<GameObject> Hits = new List<GameObject>();
        int sameBallColor = 0;
        //Primary Ball color will be white for RED BLUE AND YELLOW. If there is a secondary color for this ball, it'll check 
        BallColor primaryBallColor = BallColor.white;
        BallColor secondaryBallColor = BallColor.white;
        BallColor tertiaryBallColor = BallColor.white;
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
        if(Direction.Length >= 2){
            Ball secondBall = Direction[1].transform.GetComponent<Ball>();
            if(secondBall.BallColor == ball.BallColor || secondBall.BallColor == primaryBallColor || secondBall.BallColor == secondaryBallColor || secondBall.BallColor == tertiaryBallColor){
                if(secondBall.BallColor == ball.BallColor){
                    sameBallColor += 1;
                }
                Hits.Add(secondBall.transform.gameObject);
            }
        }
        if(Direction.Length >= 3){
            Ball thirdBall = Direction[2].transform.GetComponent<Ball>();
            if(thirdBall.BallColor == ball.BallColor || thirdBall.BallColor == primaryBallColor || thirdBall.BallColor == secondaryBallColor || thirdBall.BallColor == tertiaryBallColor){
                if(thirdBall.BallColor == ball.BallColor){
                    sameBallColor += 1;
                }
                Hits.Add(thirdBall.transform.gameObject);
            }
        }
        if(Hits.Count > 0){
            foreach(GameObject go in Hits){
                Ball chosenBall = go.GetComponent<Ball>();
                if((chosenBall.BallColor == primaryBallColor || chosenBall.BallColor == secondaryBallColor || chosenBall.BallColor == tertiaryBallColor) && chosenBall.tag == "Ball" + PlayerPrefix){
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
    private GameObject MakeABrownBall(){
        List<Detector> candidates = new List<Detector>();
        foreach(Detector detector in Detectors){
            if(detector.BallCount()){
                candidates.Add(detector);
            }
        }
        Detector chosenDetector = candidates[UnityEngine.Random.Range(0, candidates.Count)];
        return PickABrownBallInColumn(chosenDetector);
    }
	public GameObject PickABrownBallInColumn(Detector detector){
        print("Huh?");
		RaycastHit2D[] ballsInColumn = Physics2D.RaycastAll(transform.position, -Vector2.up, 100, 1 << 8);
        List<GameObject> eligibleBalls = new List<GameObject>();
        foreach(RaycastHit2D ball in ballsInColumn){
            BallColor ballColor = ball.transform.GetComponent<Ball>().BallColor;
            if(ballColor != Constants.PlayerColorToBallColor(PlayerManager.PlayerColorManager.ColorQueue[0]) || ballColor != BallColor.brown || ballColor != BallColor.black){
                eligibleBalls.Add(ball.transform.gameObject);
            }
        }
        foreach(GameObject ball in eligibleBalls){
            CheckForMatches(ball.GetComponent<Ball>());
        }
		return eligibleBalls[UnityEngine.Random.Range(0, eligibleBalls.Count)].transform.gameObject;
	}
    #endregion
}
