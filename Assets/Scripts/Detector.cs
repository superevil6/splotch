using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

public class Detector : MonoBehaviour {
public Orientation Orientation;
public SpriteRenderer SpriteRenderer;
public Sprite Sprite;
public GameObject Ball;
public GameBoard GameBoard;
public PlayerManager PlayerManager;
public ObjectPooler ObjectPooler;
private RaycastHit2D[] Hits;
public float TimeBetweenChecks;
private float TimeBetweenChecksRemaining;
public int RandomRange;

	// Use this for initialization
	void Start () {
		GameBoard = transform.GetComponentInParent<GameBoard>();
		ObjectPooler = GameBoard.GetComponent<ObjectPooler>();
		PlayerManager = GameBoard.GetComponentInParent<PlayerManager>();
		transform.localScale = Ball.transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		if(TimeBetweenChecksRemaining > 0){
			TimeBetweenChecksRemaining -= Time.deltaTime * PlayerManager.DropSpeed;
		}
		else{
			CheckColumnsForMatches();
			TimeBetweenChecksRemaining += TimeBetweenChecks;
		}
	}

	public void CheckRowsForMatches(){
		Hits = Physics2D.RaycastAll(transform.position, -Vector2.up, GameBoard.GameboardHeight / 2, 1 << 8);
	}

	public void CheckColumnsForMatches(){
		Hits = Physics2D.RaycastAll(transform.position, -Vector2.up, GameBoard.GameboardHeight / 2, 1 << 8);
		if(Hits.Length > GameBoard.Rows){
			PlayerManager.GameOver = true;
		}
	}
	public void CheckColumnForEmptySpots(){
		Hits = Physics2D.RaycastAll(transform.position, -Vector2.up, 100, 1 << 8);		
		if(Hits.Length - 1 < GameBoard.Rows){
			foreach(GameObject GO in ObjectPooler.PooledItems){
				if(!GO.activeInHierarchy){
					GO.transform.position = this.transform.position;
					Ball Ball = GO.GetComponent<Ball>();
					Ball.DetermineColor(false);
					GO.SetActive(true);
					break;
				}
			}
		}
	}
	public bool BallCount(){
		Hits = Physics2D.RaycastAll(transform.position, -Vector2.up, 10000 , 1 << 8);
		if(Hits.Length > GameBoard.Rows - 3){
			return true;
		}
		else{
			return false;
		}
	}

}
