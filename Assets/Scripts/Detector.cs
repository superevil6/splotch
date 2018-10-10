using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Detector : MonoBehaviour {
public Image Image;
public Enums.Orientation Orientation;
public GameObject Ball;
public GameBoard GameBoard;
public ObjectPooler ObjectPooler;
private RaycastHit2D[] Hits;
public float TimeBetweenChecks;
private float TimeBetweenChecksRemaining;
public int RandomRange;

	// Use this for initialization
	void Start () {
		GameBoard = GameObject.FindGameObjectWithTag("ObjectPooler").GetComponent<GameBoard>();
		ObjectPooler = GameObject.FindGameObjectWithTag("ObjectPooler").GetComponent<ObjectPooler>();
	}
	
	// Update is called once per frame
	void Update () {
		if(TimeBetweenChecksRemaining > 0){
			TimeBetweenChecksRemaining -= Time.deltaTime;
		}
		else{
			if(Orientation == Enums.Orientation.Vertical){
				CheckRowsForMatches();
			}
			else{
				CheckColumnsForMatches();
			}
			if(Orientation == Enums.Orientation.Vertical){
				CheckColumnForEmptySpots();
			}
			TimeBetweenChecksRemaining += TimeBetweenChecks;
		}
	}

	public void CheckRowsForMatches(){
		Hits = Physics2D.RaycastAll(transform.position, -Vector2.left, 100);
		//Logic for Color matching
		/*
		How can I detect colors?
		I can cycle through each color except black and white, then see 
		if the next subsequent color matches the first one. If it doesn't
		then abandon the first one, and start the check again on the second
		one. Go through each ball hit by the cast.
		
		 */
		//Logic for clearing the balls

		//Logic for rensa check
	}

	public void CheckColumnsForMatches(){
		Hits = Physics2D.RaycastAll(transform.position, -Vector2.up, 100);


	}
	public void CheckColumnForEmptySpots(){
		if(Random.Range(0, RandomRange) < 1){
			Hits = Physics2D.RaycastAll(transform.position, -Vector2.up, 100);		
			if(Hits.Length - 1 < GameBoard.Rows){
				foreach(GameObject GO in ObjectPooler.PooledItems){
					if(!GO.activeInHierarchy){
						GO.transform.position = this.transform.position;
						Ball Ball = GO.GetComponent<Ball>();
						Ball.DetermineColor();
						GO.SetActive(true);
						break;
					}
				}
			}
		}
	}

	// private void CheckColors(RaycastHit2D[] Hits){
	// 	//Array Colors = Enum.GetValues(typeof(Enums.PlayerColor));
	// 	for(int i = 0; i <= Hits.Length; i++){
	// 		Ball Ball = Hits[i].transform.gameObject.GetComponent<Ball>();
	// 		Ball BallTwo;
	// 		if(Hits[i + 1] != null){
	// 			BallTwo = Hits[i+1].transform.gameObject.GetComponent<Ball>();
	// 		}
	// 		else{
	// 			break;
	// 		}
	// 		if(Ball.BallColor == BallTwo.BallColor){
				
	// 		}
	// 	}
	// }
}
