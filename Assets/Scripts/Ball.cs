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
public CircleCollider2D CircleCollider2D;
private Color32 OldBallColor;
private Color32 NewBallColor;
public float TransitionTime;
private float TimeLeft;
private bool TransitionColorCheck = false;

	// Use this for initialization
	void Start () {
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
	}
	void OnMouseDown()
	{
		ChangeBallColor(PlayerColor.NextColor);
	}
	private void SetupBall(){
		SpriteRenderer = GetComponent<SpriteRenderer>();
		CircleCollider2D = GetComponent<CircleCollider2D>();
		SpriteRenderer.sprite = Sprite;
		DetermineColor();

	}

	//placeholder for color generation: Eventually it will have weighted values.		
	private void DetermineColor(){
		BallColor = Constants.WeightedGenerateColor(Constants.DefaultColorWeights);
		SpriteRenderer.color = Constants.SetColor(BallColor);
	}
	// private void DetermineColor(Color32 OldBallColor, Color32 NewBallColor){
	// 	SpriteRenderer.color = Color.Lerp(OldBallColor, NewBallColor, TransitionTime);
	// }

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