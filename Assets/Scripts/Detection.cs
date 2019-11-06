using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class Detection : MonoBehaviour {
public PlayerManager PlayerManager;
public Ball Ball;
private Vector2 BallSize;
//public ObjectPooler ObjectPooler;
public Rigidbody2D Rigidbody2D;
public ParticleSystem ParticleSystem;
private RaycastHit2D[] HitUp;
private RaycastHit2D[] HitDown;
private RaycastHit2D[] HitLeft;
private RaycastHit2D[] HitRight;
private RaycastHit2D[] HitUpLeft;
private RaycastHit2D[] HitUpRight;
private RaycastHit2D[] HitDownLeft;
private RaycastHit2D[] HitDownRight;

private List<GameObject> Hits;
private List<GameObject> VerticalHits;
private List<GameObject> HorizontalHits;
private List<GameObject> DiagonalHitsULDR; //from upleft to downright
private List<GameObject> DiagonalHitsURDL; //from upright to downleft

private BallColor BallColor;
//Rework this so Columns is in a more accessible area, so I don't have repeate variables.
	// Use this for initialization
	void Start () {
		PlayerManager = GetComponentInParent<PlayerManager>();
		BallColor = Ball.BallColor;
		// var PartMain = ParticleSystem.main; 
		// PartMain.startColor = Ball.SetColor(Ball.BallColor);
		BallSize = Constants.FindOffset(Ball.gameObject);
		//BallSize = new Vector2(BallSize.x - Ball.GameBoardObject.Scale , BallSize.y - Ball.GameBoardObject.Scale);
		//ObjectPooler = GameObject.FindGameObjectWithTag("ObjectPooler").GetComponent<ObjectPooler>();
		Hits = new List<GameObject>();
		VerticalHits = new List<GameObject>();
		HorizontalHits = new List<GameObject>();
		DiagonalHitsULDR = new List<GameObject>(); 
		DiagonalHitsURDL = new List<GameObject>(); 

	}
	
	// Update is called once per frame
	void Update () {

	}
	public void CheckForMatches(){
		Hits.Clear();
		VerticalHits.Clear();
		HorizontalHits.Clear();
		DiagonalHitsULDR.Clear();
		DiagonalHitsURDL.Clear();
		BallColor = Ball.BallColor;
		/* HitUp will check for as many balls that spawn in a column, that way if bottom ball detects the max
		number of balls, it knows the game is over. Kind of hacky, I might redo this. */
		HitUp = Physics2D.RaycastAll(transform.position, Vector2.up, BallSize.x * 2);
		HitDown = Physics2D.RaycastAll(transform.position, -Vector2.up, BallSize.x * 2);
		HitLeft = Physics2D.RaycastAll(transform.position, -Vector2.right, BallSize.y * 2);
		HitRight = Physics2D.RaycastAll(transform.position, Vector2.right, BallSize.y * 2);
		HitUpLeft = Physics2D.RaycastAll(transform.position, new Vector2(-1, 1), (BallSize.x * BallSize.y) * 2);
		HitUpRight = Physics2D.RaycastAll(transform.position, new Vector2(1, 1), (BallSize.x * BallSize.y) * 2);
		HitDownLeft = Physics2D.RaycastAll(transform.position, new Vector2(-1, -1), (BallSize.x * BallSize.y) * 2);
		HitDownRight = Physics2D.RaycastAll(transform.position, new Vector2(1, -1), (BallSize.x * BallSize.y) * 2);

		CheckDirection(VerticalHits, HitUp);
		CheckDirection(VerticalHits, HitDown);
		CheckDirection(HorizontalHits, HitLeft);
		CheckDirection(HorizontalHits, HitRight);
		CheckDirection(DiagonalHitsULDR, HitUpLeft);
		CheckDirection(DiagonalHitsULDR, HitDownRight);
		CheckDirection(DiagonalHitsURDL, HitUpRight);
		CheckDirection(DiagonalHitsURDL, HitDownLeft);

		StartCoroutine(WaitForColorChange(Ball.TransitionTime));
		
	}
	private List<GameObject> CheckDirection(List<GameObject> Hits, RaycastHit2D[] Direction){
		if(Direction.Length > 1){
			if(Direction[1].transform.gameObject.tag == "Ball" && Direction[1].transform.GetComponent<Ball>().BallColor == BallColor){
				GameObject Hit = Direction[0].transform.gameObject;
				if(CheckForBlackAndWhite(Hit)){
					Hits.Add(Hit);
				}
				Hit = Direction[1].transform.gameObject;
				if(CheckForBlackAndWhite(Hit)){
					Hits.Add(Hit);
				}
				if(Direction.Length > 2){
					if(Direction[2].transform.gameObject.tag == "Ball" && Direction[2].collider.gameObject.GetComponent<Ball>().BallColor == BallColor){
						Hit = Direction[2].transform.gameObject;
						if(CheckForBlackAndWhite(Hit)){
							Hits.Add(Hit);
						}
					}
				}
				return Hits;
			}
		}
		return Hits;
	}

	private void DeactivateHits(List<GameObject> Hits){
		if(Hits.Count >= 3){
			// ParticleSystem.Emit(5);
			int scoreValue = PointValue(Hits[0].GetComponent<Ball>().BallColor);
			PlayerManager.Score += PlayerManager.ScoreMultiplier * scoreValue * Hits.Count;
			PlayerManager.ScoreMultiplier += 1;
			PlayerManager.RensaTime = PlayerManager.AllotedRensaTime;
			foreach(GameObject Hit in Hits){
			//Constants.RensaCheck = true;
			Hit.SetActive(false);
			}
		}
	}
	private IEnumerator WaitForColorChange(float ChangeTime){
		yield return new WaitForSeconds(ChangeTime);
		DeactivateHits(HorizontalHits);
		DeactivateHits(VerticalHits);
		DeactivateHits(DiagonalHitsULDR);
		DeactivateHits(DiagonalHitsURDL);

	}

	private bool CheckForBlackAndWhite(GameObject Ball){
		if(Ball.GetComponent<Ball>().BallColor != BallColor.white && Ball.GetComponent<Ball>().BallColor != BallColor.black){
			return true;
		}
		else{
			return false;
		}
	}
	private int PointValue(BallColor ballColor){
		if(ballColor == BallColor.red || ballColor == BallColor.blue || ballColor == BallColor.yellow){
			return 100;
		}
		if(ballColor == BallColor.purple || ballColor == BallColor.green || ballColor == BallColor.orange || Ball.Type == BallType.rainbow){
			return 200;
		}
		if(ballColor == BallColor.brown){
			return 50;
		}
		return 0;
	}
}
