using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour {
public Ball Ball;
private Vector2 BallSize;
//public ObjectPooler ObjectPooler;
public Rigidbody2D Rigidbody2D;
private RaycastHit2D[] HitUp;
private RaycastHit2D[] HitDown;
private RaycastHit2D[] HitLeft;
private RaycastHit2D[] HitRight;
private List<GameObject> Hits;
private List<GameObject> VerticalHits;
private List<GameObject> HorizontalHits;
private Enums.BallColor BallColor;
	// Use this for initialization
	void Start () {
		BallColor = Ball.BallColor;
		BallSize = Constants.FindOffset(Ball.gameObject);
		//ObjectPooler = GameObject.FindGameObjectWithTag("ObjectPooler").GetComponent<ObjectPooler>();
		Hits = new List<GameObject>();
		VerticalHits = new List<GameObject>();
		HorizontalHits = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {

	}
	public void CheckForMatches(){
		Hits.Clear();
		VerticalHits.Clear();
		HorizontalHits.Clear();
		BallColor = Ball.BallColor;
		HitUp = Physics2D.RaycastAll(transform.position, Vector2.up, BallSize.x * 2);
		HitDown = Physics2D.RaycastAll(transform.position, -Vector2.up, BallSize.x * 2);
		HitLeft = Physics2D.RaycastAll(transform.position, -Vector2.right, BallSize.y * 2);
		HitRight = Physics2D.RaycastAll(transform.position, Vector2.right, BallSize.y * 2);
		CheckDirection(VerticalHits, HitUp);
		CheckDirection(VerticalHits, HitDown);
		CheckDirection(HorizontalHits, HitLeft);
		CheckDirection(HorizontalHits, HitRight);
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
			int scoreValue = PointValue(Hits[0].GetComponent<Ball>().BallColor);
			Constants.Score += Constants.ScoreMultiplier * scoreValue * Hits.Count;
			Constants.ScoreMultiplier += 1;
			Constants.RensaTime = Constants.AllotedRensaTime;
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
	}

	private bool CheckForBlackAndWhite(GameObject Ball){
		if(Ball.GetComponent<Ball>().BallColor != Enums.BallColor.white && Ball.GetComponent<Ball>().BallColor != Enums.BallColor.black){
			return true;
		}
		else{
			return false;
		}
	}
	private int PointValue(Enums.BallColor ballColor){
		if(ballColor == Enums.BallColor.red || ballColor == Enums.BallColor.blue || ballColor == Enums.BallColor.yellow){
			return 100;
		}
		if(ballColor == Enums.BallColor.purple || ballColor == Enums.BallColor.green || ballColor == Enums.BallColor.orange){
			return 200;
		}
		if(ballColor == Enums.BallColor.brown){
			return 50;
		}
		return 0;
	}
}
