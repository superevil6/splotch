using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Enums;

public class Detection : MonoBehaviour {
public PlayerManager PlayerManager;
public ScoreManager ScoreManager;
public Ball Ball;
private GameBoard GameBoard;
public ParticleSystem ParticleSystem;
public GameObject ParticleObject;
private Vector2 BallSize;
private RectTransform RectTransform;
public Rigidbody2D Rigidbody2D;
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
public int CheckDistance;
public int RensaMultiplier = 1;
public float RensaTime;
private string PlayerPrefix;
//Rework this so Columns is in a more accessible area, so I don't have repeate variables.
	// Use this for initialization
	void Start () {
		PlayerManager = GetComponentInParent<PlayerManager>();
		ScoreManager = PlayerManager.ScoreManager;
		PlayerPrefix = PlayerManager.PlayerNumberManager.PlayerPrefix;
		BallColor = Ball.BallColor;
		RensaMultiplier = 1;
		RectTransform = (RectTransform)gameObject.transform;
		BallSize = new Vector2(gameObject.transform.localScale.x, gameObject.transform.localScale.y);
		GameBoard = Ball.GameBoard;
		ParticleSystem = ParticleObject.GetComponent<ParticleSystem>();
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
			RensaMultiplier = 1;
		}
	}

	public void CheckForMatches(bool rensaChain){
		Hits.Clear();
		VerticalHits.Clear();
		HorizontalHits.Clear();
		DiagonalHitsULDR.Clear();
		DiagonalHitsURDL.Clear();
		BallColor = Ball.BallColor;
		/* HitUp will check for as many balls that spawn in a column, that way if bottom ball detects the max
		number of balls, it knows the game is over. Kind of hacky, I might redo this. */
		HitUp = Physics2D.RaycastAll(transform.position, Vector2.up, transform.lossyScale.y * CheckDistance, 1 << 8);
		Debug.DrawRay(transform.position, Vector2.up * transform.lossyScale.y * 3, Color.blue, 4);
		HitDown = Physics2D.RaycastAll(transform.position, -Vector2.up, transform.lossyScale.y * CheckDistance, 1 << 8);
		HitLeft = Physics2D.RaycastAll(transform.position, -Vector2.right, transform.lossyScale.x * CheckDistance, 1 << 8);
		HitRight = Physics2D.RaycastAll(transform.position, Vector2.right, transform.lossyScale.x * CheckDistance, 1 << 8);
		HitUpLeft = Physics2D.RaycastAll(transform.position, new Vector2(-transform.lossyScale.x, transform.lossyScale.y), (transform.lossyScale.x * transform.lossyScale.y) * CheckDistance, 1 << 8);
		HitUpRight = Physics2D.RaycastAll(transform.position, new Vector2(transform.lossyScale.x, transform.lossyScale.y), (transform.lossyScale.x * transform.lossyScale.y) * CheckDistance, 1 << 8);
		HitDownLeft = Physics2D.RaycastAll(transform.position, new Vector2(-transform.lossyScale.x, -transform.lossyScale.y), (transform.lossyScale.x * transform.lossyScale.y) * CheckDistance, 1 << 8);
		HitDownRight = Physics2D.RaycastAll(transform.position, new Vector2(transform.lossyScale.x, -transform.lossyScale.y), (transform.lossyScale.x * transform.lossyScale.y) * CheckDistance, 1 << 8);

		CheckDirection(HitUp, VerticalHits);
		CheckDirection(HitDown, VerticalHits);
		CheckDirection(HitLeft, HorizontalHits);
		CheckDirection(HitRight, HorizontalHits);
		CheckDirection(HitUpLeft, DiagonalHitsULDR);
		CheckDirection(HitDownRight, DiagonalHitsULDR);
		CheckDirection(HitUpRight, DiagonalHitsURDL);
		CheckDirection(HitDownLeft, DiagonalHitsURDL);

		//StartCoroutine(WaitForColorChange(Ball.TransitionTime, renseChain));
		CheckForEnoughHits(Hits, rensaChain);
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
	private IEnumerator WaitForColorChange(float ChangeTime, bool rensaChain){
		yield return new WaitForSeconds(ChangeTime);

		CheckForEnoughHits(Hits, rensaChain);
	}
	private void CheckForEnoughHits(List<GameObject> Hits, bool rensaChain){
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
		if(Hits.Count >= 3){
			PlayParticleEffect();
			ScoreManager.DeactivateHits(Hits);
		}
	}

	private void PlayParticleEffect(){
		foreach(GameObject go in GameBoard.ObjectPooler.PooledParticleEmitters){
			if(!go.GetComponent<ParticleSystem>().isPlaying){
				go.transform.position = gameObject.transform.position;
				var ps = go.GetComponent<ParticleSystem>();
				var main = ps.main;
				var color = Constants.FindColor(Ball.BallColor, Ball.PlayerManager.ColorScheme);
				main.startColor = color;
				ps.Play();
			}
		}
	}

	private bool CheckForBlackAndWhite(GameObject Ball){
		if(Ball.GetComponent<Ball>().BallColor != BallColor.white && Ball.GetComponent<Ball>().BallColor != BallColor.black){
			return true;
		}
		else{
			return false;
		}
	}
}
