using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour {
public Enums.BallColor BallColor;
public Enums.BallType Type;
public SpriteRenderer SpriteRenderer;
public Sprite Sprite;
public GameObject PlayerColorObject;
public PlayerColor PlayerColor;
public BoxCollider2D BoxCollider2D;

	// Use this for initialization
	void Start () {
		PlayerColorObject = GameObject.FindGameObjectWithTag("PlayerColor");
		PlayerColor = PlayerColorObject.GetComponent<PlayerColor>();
		SetupBall();
		
	}
	
	// Update is called once per frame
	void Update () {

	}
	void OnMouseDown()
	{
		print("Detecting Touch");
		ChangeBallColor(PlayerColor.NextColor);
	}
	private void SetupBall(){
		SpriteRenderer = GetComponent<SpriteRenderer>();
		BoxCollider2D = GetComponent<BoxCollider2D>();
		SpriteRenderer.sprite = Sprite;
		DetermineColor();

	}

	//placeholder for color generation: Eventually it will have weighted values.		
	private void DetermineColor(){
		BallColor = Constants.GenerateColor();
		SpriteRenderer.color = Constants.SetColor(BallColor);
	}
	private void DetermineColor(Enums.BallColor NewBallColor){
		print(NewBallColor);
		SpriteRenderer.color = Constants.SetColor(NewBallColor);
	}
	private void ChangeBallColor(Enums.PlayerColor PlayerColor){
		switch(PlayerColor){
			case (Enums.PlayerColor.red) :
			print("Red Ball");
				if(BallColor.Equals(Enums.BallColor.white)){
					DetermineColor(Enums.BallColor.red);
					BallColor = Enums.BallColor.red;
					PlayerColorObject.GetComponent<PlayerColor>().NextColor = Constants.GeneratePlayerColor();
				}
				if(BallColor.Equals(Enums.BallColor.blue)){
					DetermineColor(Enums.BallColor.purple);
					BallColor = Enums.BallColor.purple;
					PlayerColorObject.GetComponent<PlayerColor>().NextColor = Constants.GeneratePlayerColor();
				}
				if(BallColor.Equals(Enums.BallColor.yellow)){
					DetermineColor(Enums.BallColor.orange);
					BallColor = Enums.BallColor.orange;
					PlayerColorObject.GetComponent<PlayerColor>().NextColor = Constants.GeneratePlayerColor();
				}
				else if(BallColor.Equals(Enums.BallColor.purple) || BallColor.Equals(Enums.BallColor.green) || BallColor.Equals(Enums.BallColor.orange)){
					DetermineColor(Enums.BallColor.brown);
					BallColor = Enums.BallColor.brown;
					PlayerColorObject.GetComponent<PlayerColor>().NextColor = Constants.GeneratePlayerColor();
				}
				else if(BallColor.Equals(Enums.BallColor.brown)){
					DetermineColor(Enums.BallColor.black);
					BallColor = Enums.BallColor.black;
					PlayerColorObject.GetComponent<PlayerColor>().NextColor = Constants.GeneratePlayerColor();
				}
			break;
			case (Enums.PlayerColor.yellow) :
				if(BallColor.Equals(Enums.BallColor.white)){
					DetermineColor(Enums.BallColor.yellow);
					BallColor = Enums.BallColor.yellow;
					PlayerColorObject.GetComponent<PlayerColor>().NextColor = Constants.GeneratePlayerColor();
				}
				if(BallColor.Equals(Enums.BallColor.blue)){
					DetermineColor(Enums.BallColor.green);
					BallColor = Enums.BallColor.green;
					PlayerColorObject.GetComponent<PlayerColor>().NextColor = Constants.GeneratePlayerColor();
				}
				if(BallColor.Equals(Enums.BallColor.red)){
					DetermineColor(Enums.BallColor.orange);
					BallColor = Enums.BallColor.orange;
					PlayerColorObject.GetComponent<PlayerColor>().NextColor = Constants.GeneratePlayerColor();
				}
				else if(BallColor.Equals(Enums.BallColor.purple) || BallColor.Equals(Enums.BallColor.green) || BallColor.Equals(Enums.BallColor.orange)){
					DetermineColor(Enums.BallColor.brown);
					BallColor = Enums.BallColor.brown;
					PlayerColorObject.GetComponent<PlayerColor>().NextColor = Constants.GeneratePlayerColor();
				}
				else if(BallColor.Equals(Enums.BallColor.brown)){
					DetermineColor(Enums.BallColor.black);
					BallColor = Enums.BallColor.black;
					PlayerColorObject.GetComponent<PlayerColor>().NextColor = Constants.GeneratePlayerColor();
				}
			break;
			case (Enums.PlayerColor.blue) :
				if(BallColor.Equals(Enums.BallColor.white)){
					DetermineColor(Enums.BallColor.blue);
					BallColor = Enums.BallColor.blue;
					PlayerColorObject.GetComponent<PlayerColor>().NextColor = Constants.GeneratePlayerColor();
				}
				if(BallColor.Equals(Enums.BallColor.yellow)){
					DetermineColor(Enums.BallColor.green);
					BallColor = Enums.BallColor.green;
					PlayerColorObject.GetComponent<PlayerColor>().NextColor = Constants.GeneratePlayerColor();
				}
				if(BallColor.Equals(Enums.BallColor.red)){
					DetermineColor(Enums.BallColor.purple);
					BallColor = Enums.BallColor.purple;
					PlayerColorObject.GetComponent<PlayerColor>().NextColor = Constants.GeneratePlayerColor();
				}
				else if(BallColor.Equals(Enums.BallColor.purple) || BallColor.Equals(Enums.BallColor.green) || BallColor.Equals(Enums.BallColor.orange)){
					DetermineColor(Enums.BallColor.brown);
					BallColor = Enums.BallColor.brown;
					PlayerColorObject.GetComponent<PlayerColor>().NextColor = Constants.GeneratePlayerColor();
				}
				else if(BallColor.Equals(Enums.BallColor.brown)){
					DetermineColor(Enums.BallColor.black);
					BallColor = Enums.BallColor.black;
					PlayerColorObject.GetComponent<PlayerColor>().NextColor = Constants.GeneratePlayerColor();
				}
			break;
		} 
	}

}
