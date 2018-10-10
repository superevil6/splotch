using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour {
public Enums.BallColor BallColor;
public Detection Detection;
private Enums.BallColor NewColor;
public Enums.BallType Type;
public SpriteRenderer SpriteRenderer;
public Sprite Sprite;
public GameObject PlayerColorObject;
public PlayerColor PlayerColor;
public BoxCollider2D BoxCollider2D;
public Rigidbody2D Rigidbody2D;
private Color32 OldBallColor;
private Color32 NewBallColor;
public float TransitionTime;
private float TimeBetweenChecks;
public float TimeBetweenChecksTotal;
private float TimeLeft;
private bool TransitionColorCheck = false;
private bool HasMoved;

	// Use this for initialization

	void OnEnable () {
		print("Enabling");
		PlayerColorObject = GameObject.FindGameObjectWithTag("PlayerColor");
		PlayerColor = PlayerColorObject.GetComponent<PlayerColor>();
		Detection = gameObject.GetComponent<Detection>();
		SetupBall();
	}
	
	// Update is called once per frame
	void Update () {
		if(TimeLeft > 0){
			SpriteRenderer.color = Color.Lerp(SpriteRenderer.color, NewBallColor, Time.deltaTime / TimeLeft);
			TimeLeft -= Time.deltaTime;
		}
		// if(TimeBetweenChecks > 0){
		// 	TimeBetweenChecks -= Time.deltaTime;
		// }
		// else if(Rigidbody2D.velocity.magnitude == 0){
		// 	Detection.CheckForMatches();
		// 	TimeBetweenChecks = TimeBetweenChecksTotal;
		// }
		
		if(Rigidbody2D.velocity.magnitude >= 0.5){
			HasMoved = true;
		}
		if(Rigidbody2D.velocity.magnitude <= 1 && HasMoved){
			Detection.CheckForMatches();
			HasMoved = false;
		}
	}

	void OnMouseDown()
	{
		ChangeBallColor(PlayerColor.NextColor);
	}

	private void SetupBall(){
		SpriteRenderer = GetComponent<SpriteRenderer>();
		BoxCollider2D = GetComponent<BoxCollider2D>();
		SpriteRenderer.sprite = Sprite;
		DetermineColor();
	}

	public void DetermineColor(){
		BallColor = Constants.WeightedGenerateColor(Constants.DefaultColorWeights);
		SpriteRenderer.color = Constants.SetColor(BallColor);
	}

	private void ChangeBallColor(Enums.PlayerColor PlayerColor){
		switch(PlayerColor){
			case (Enums.PlayerColor.red) :
				if(BallColor.Equals(Enums.BallColor.white)){
					SetNewBallColor(Enums.BallColor.red);
					break;
				}
				if(BallColor.Equals(Enums.BallColor.blue)){
					SetNewBallColor(Enums.BallColor.purple);
					break;
				}
				if(BallColor.Equals(Enums.BallColor.yellow)){
					SetNewBallColor(Enums.BallColor.orange);
					break;
				}
				else if(BallColor.Equals(Enums.BallColor.purple) || BallColor.Equals(Enums.BallColor.green) || BallColor.Equals(Enums.BallColor.orange)){
					SetNewBallColor(Enums.BallColor.brown);
					break;
				}
				else if(BallColor.Equals(Enums.BallColor.brown)){
					SetNewBallColor(Enums.BallColor.black);
					break;
				}
			break;
			case (Enums.PlayerColor.yellow) :
				if(BallColor.Equals(Enums.BallColor.white)){
					SetNewBallColor(Enums.BallColor.yellow);
					break;
				}
				if(BallColor.Equals(Enums.BallColor.blue)){
					SetNewBallColor(Enums.BallColor.green);
					break;
				}
				if(BallColor.Equals(Enums.BallColor.red)){
					SetNewBallColor(Enums.BallColor.orange);
					break;
				}
				else if(BallColor.Equals(Enums.BallColor.purple) || BallColor.Equals(Enums.BallColor.green) || BallColor.Equals(Enums.BallColor.orange)){
					SetNewBallColor(Enums.BallColor.brown);
					break;
				}
				else if(BallColor.Equals(Enums.BallColor.brown)){
					SetNewBallColor(Enums.BallColor.black);
					break;
				}
			break;
			case (Enums.PlayerColor.blue) :
				if(BallColor.Equals(Enums.BallColor.white)){
					SetNewBallColor(Enums.BallColor.blue);
					break;
				}
				if(BallColor.Equals(Enums.BallColor.yellow)){
					SetNewBallColor(Enums.BallColor.green);
					break;
				}
				if(BallColor.Equals(Enums.BallColor.red)){
					SetNewBallColor(Enums.BallColor.purple);
					break;
				}
				else if(BallColor.Equals(Enums.BallColor.purple) || BallColor.Equals(Enums.BallColor.green) || BallColor.Equals(Enums.BallColor.orange)){
					SetNewBallColor(Enums.BallColor.brown);
					break;
				}
				else if(BallColor.Equals(Enums.BallColor.brown)){
					SetNewBallColor(Enums.BallColor.black);
					break;
				}
			break;
		} 
	}
	private void SetNewBallColor(Enums.BallColor InputBallColor){
		OldBallColor = Constants.SetColor(BallColor);
		BallColor = InputBallColor;
		NewBallColor = Constants.SetColor(BallColor);
		TimeLeft = TransitionTime;
		Detection.CheckForMatches();
		PlayerColorObject.GetComponent<PlayerColor>().NextColor = Constants.GeneratePlayerColor();
	}

}