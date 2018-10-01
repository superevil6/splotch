using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour {
public Ball Ball;
private Vector2 BallSize;
public ObjectPooler ObjectPooler;
public Rigidbody2D Rigidbody2D;
private RaycastHit2D[] HitUp;
private RaycastHit2D[] HitDown;
private RaycastHit2D[] HitLeft;
private RaycastHit2D[] HitRight;
private List<GameObject> Hits;
private List<RaycastHit2D> VerticalHits;
private List<RaycastHit2D> HorizontalHits;
private Enums.BallColor BallColor;
	// Use this for initialization
	void Start () {
		BallColor = Ball.BallColor;
		BallSize = Constants.FindOffset(Ball.gameObject);
		ObjectPooler = GameObject.FindGameObjectWithTag("ObjectPooler").GetComponent<ObjectPooler>();
		Hits = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {

	}
	public void CheckForMatches(){
		BallColor = Ball.BallColor;
		HitUp = Physics2D.RaycastAll(transform.position, Vector2.up, BallSize.x * 2);
		HitDown = Physics2D.RaycastAll(transform.position, -Vector2.up, BallSize.x * 2);
		HitLeft = Physics2D.RaycastAll(transform.position, -Vector2.right, BallSize.y * 2);
		HitRight = Physics2D.RaycastAll(transform.position, Vector2.right, BallSize.y * 2);

		CheckDirection(Hits, HitUp);
		CheckDirection(Hits, HitDown);
		CheckDirection(Hits, HitLeft);
		CheckDirection(Hits, HitRight);
		StartCoroutine(WaitForColorChange(Ball.TransitionTime));

		// foreach(RaycastHit2D Hit in HitUp){
		// 	if()
		// 	HorizontalHits.Add(Hit);
		// }
		// foreach(RaycastHit2D Hit in HitDown){
		// 	HorizontalHits.Add(Hit);
		// }


	}
	private List<GameObject> CheckDirection(List<GameObject> Hits, RaycastHit2D[] Direction){
		if(Direction.Length > 1 && Direction[1].collider.GetComponent<Ball>().BallColor == BallColor){
			print(Direction[1].transform.gameObject);
			GameObject Hit = Direction[0].transform.gameObject;
			Hits.Add(Hit);
			Hit = Direction[1].transform.gameObject;
			Hits.Add(Hit);
			
			if(Direction.Length > 2 && Direction[2].collider.gameObject.GetComponent<Ball>().BallColor == BallColor){
				Hit = Direction[2].transform.gameObject;
				Hits.Add(Hit);
			}
			return Hits;
		}
		return Hits;
	}

	private void DeactivateHits(List<GameObject> Hits){
		if(Hits.Count >= 3){
			foreach(GameObject Hit in Hits){
			Hit.SetActive(false);
			}
		}
	}
	private IEnumerator WaitForColorChange(float ChangeTime){
		yield return new WaitForSeconds(ChangeTime);
		DeactivateHits(Hits);
	}
}
