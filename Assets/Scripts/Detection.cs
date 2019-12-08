using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
public int RensaMultiplier;
private float RensaTime;
private string PlayerPrefix;
//Rework this so Columns is in a more accessible area, so I don't have repeate variables.
	// Use this for initialization
	void Start () {
		PlayerManager = GetComponentInParent<PlayerManager>();
		PlayerPrefix = PlayerManager.PlayerNumberManager.PlayerPrefix;
		BallColor = Ball.BallColor;
		BallSize = new Vector2(gameObject.transform.localScale.x, gameObject.transform.localScale.y);
		//BallSize = transform.localScale;
		// var PartMain = ParticleSystem.main; 
		// PartMain.startColor = Ball.SetColor(Ball.BallColor);
		//BallSize = Constants.FindOffset(Ball.gameObject);
		Hits = new List<GameObject>();
		VerticalHits = new List<GameObject>();
		HorizontalHits = new List<GameObject>();
		DiagonalHitsULDR = new List<GameObject>(); 
		DiagonalHitsURDL = new List<GameObject>(); 

	}
	
	// Update is called once per frame
	void Update () {
		if(RensaTime > 0){
			RensaTime -= Time.deltaTime;
		}
		if(RensaTime <= 0 && RensaMultiplier > 1){
			//print("rensa time expired.");
			RensaMultiplier = 1;
		}
	}
	public void CheckForMatches(bool renseChain){
		Hits.Clear();
		VerticalHits.Clear();
		HorizontalHits.Clear();
		DiagonalHitsULDR.Clear();
		DiagonalHitsURDL.Clear();
		BallColor = Ball.BallColor;
		/* HitUp will check for as many balls that spawn in a column, that way if bottom ball detects the max
		number of balls, it knows the game is over. Kind of hacky, I might redo this. */
		HitUp = Physics2D.RaycastAll(transform.position, Vector2.up, BallSize.y * 2, 1 << 8);
		HitDown = Physics2D.RaycastAll(transform.position, -Vector2.up, BallSize.y * 2, 1 << 8);
		HitLeft = Physics2D.RaycastAll(transform.position, -Vector2.right, BallSize.x * 2, 1 << 8);
		HitRight = Physics2D.RaycastAll(transform.position, Vector2.right, BallSize.x * 2, 1 << 8);
		HitUpLeft = Physics2D.RaycastAll(transform.position, new Vector2(-transform.localScale.x, transform.localScale.y), 100, 1 << 8);
		HitUpRight = Physics2D.RaycastAll(transform.position, new Vector2(transform.localScale.x, transform.localScale.y), 100, 1 << 8);
		HitDownLeft = Physics2D.RaycastAll(transform.position, new Vector2(-transform.localScale.x, -transform.localScale.y), 100, 1 << 8);
		HitDownRight = Physics2D.RaycastAll(transform.position, new Vector2(transform.localScale.x, -transform.localScale.y), 100, 1 << 8);

		CheckDirection(HitUp, VerticalHits);
		CheckDirection(HitDown, VerticalHits);
		CheckDirection(HitLeft, HorizontalHits);
		CheckDirection(HitRight, HorizontalHits);
		CheckDirection(HitUpLeft, DiagonalHitsULDR);
		CheckDirection(HitDownRight, DiagonalHitsULDR);
		CheckDirection(HitUpRight, DiagonalHitsURDL);
		CheckDirection(HitDownLeft, DiagonalHitsURDL);

		StartCoroutine(WaitForColorChange(Ball.TransitionTime, renseChain));
		
	}

	private void CheckDirection(RaycastHit2D[] Direction, List<GameObject> hits){
		if(Direction.Length > 1){
			if(Direction[1].transform.gameObject.tag == "Ball" + PlayerPrefix && Direction[1].transform.GetComponent<Ball>().BallColor == BallColor){
				GameObject Hit = Direction[0].transform.gameObject;
				if(CheckForBlackAndWhite(Hit)){
					hits.Add(Hit);
				}
				Hit = Direction[1].transform.gameObject;
				if(CheckForBlackAndWhite(Hit)){
					hits.Add(Hit);
				}
				if(Direction.Length > 2){
					if(Direction[2].transform.gameObject.tag == "Ball" + PlayerPrefix && Direction[2].collider.gameObject.GetComponent<Ball>().BallColor == BallColor){
						Hit = Direction[2].transform.gameObject;
						if(CheckForBlackAndWhite(Hit)){
							hits.Add(Hit);
						}
					}
				}
			}
		}
	}

	private void DeactivateHits(List<GameObject> Hits, bool rensaChain){
		if(Hits.Count >= 3){
			PassRensaMultiplier();
			int scoreValue = PointValue(Hits[0].GetComponent<Ball>().BallColor);
			List<int> rensaValues = new List<int>();				
			foreach(GameObject go in Hits){
				rensaValues.Add(go.transform.GetComponent<Detection>().RensaMultiplier);
			}
			int highestRensa = rensaValues.Max();
			PlayerManager.Score += highestRensa * scoreValue * Hits.Count;
			PlayerManager.NumberOfBallsBeingCleared += Hits.Count;
			foreach(GameObject Hit in Hits){
				Hit.SetActive(false);
			}
		}
	}
	private IEnumerator WaitForColorChange(float ChangeTime, bool rensaChain){
		yield return new WaitForSeconds(ChangeTime);
		if(HorizontalHits.Count >= 3){
			Hits.AddRange(HorizontalHits);
		}
		if(VerticalHits.Count >= 3){
			Hits.AddRange(VerticalHits);
		}
		if(DiagonalHitsULDR.Count >= 3){
			Hits.AddRange(DiagonalHitsULDR);
		}
		if(DiagonalHitsURDL.Count >= 3){
			Hits.AddRange(DiagonalHitsURDL);
		}
		DeactivateHits(Hits, rensaChain);
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

	private void PassRensaMultiplier(){
		RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.up, 1000, 1 << 8);
		foreach(RaycastHit2D hit in hits){
			if(hit.transform.tag == "Ball" + PlayerManager.PlayerNumberManager.PlayerPrefix){
				hit.transform.GetComponent<Detection>().RensaMultiplier += 1;
				hit.transform.GetComponent<Detection>().RensaTime = 1f;
			}
		}
	}
}
