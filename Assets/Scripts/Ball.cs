﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

public class Ball : MonoBehaviour {
public BallColor BallColor;
public Detection Detection;
private BallColor NewColor;
public BallType Type;
public SpriteRenderer SpriteRenderer;
public Sprite Sprite;
public GameObject PlayerColorObject;
public PlayerColorManager PlayerColorManager;
public BoxCollider2D BoxCollider2D;
public Rigidbody2D Rigidbody2D;
public GameObject GameBoard;
public GameBoard GameBoardObject;
private Color32 OldBallColor;
private Color32 NewBallColor;
public float TransitionTime;
private float TimeBetweenChecks;
public float TimeBetweenChecksTotal;
private float TimeLeft;
private bool TransitionColorCheck = false;
private bool HasMoved;
private bool DoOnce = false;

	// Use this for initialization

	void OnEnable () {

		//print(Scale);
		PlayerColorObject = GameObject.FindGameObjectWithTag("PlayerColor");
		GameBoard = GameObject.FindGameObjectWithTag("ObjectPooler");
		PlayerColorManager = PlayerColorObject.GetComponent<PlayerColorManager>();
		GameBoardObject = GameBoard.GetComponent<GameBoard>();
		GameBoardObject.Scale = 1f - GameBoardObject.Columns * 0.10f;
		if(!DoOnce){
			transform.localScale = new Vector2((transform.localScale.x * GameBoardObject.Scale /2), (transform.localScale.y * GameBoardObject.Scale) /2);
			BoxCollider2D.size = new Vector2(15 * transform.localScale.x, 15 * transform.localScale.y);
			DoOnce = true;
		}
		Detection = gameObject.GetComponent<Detection>();
		SetupBall();
	}
	
	// Update is called once per frame
	void Update () {
		if(TimeLeft > 0){
			SpriteRenderer.color = Color.Lerp(SpriteRenderer.color, NewBallColor, Time.deltaTime / TimeLeft);
			TimeLeft -= Time.deltaTime;
		}
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
		ChangeBallColor(PlayerColorManager.ColorQueue[0]);
		PlayerColorManager.UpdateColorQueue();
	}

	private void SetupBall(){
		SpriteRenderer = GetComponent<SpriteRenderer>();
		BoxCollider2D = GetComponent<BoxCollider2D>();
		//gameObject.GetComponent<RectTransform>();
		SpriteRenderer.sprite = Sprite;
		DetermineColor();
	}

	public void DetermineColor(){
		BallColor = Constants.WeightedGenerateColor(Constants.DefaultColorWeights);
		SpriteRenderer.color = Constants.SetColor(BallColor);
	}

	public void ChangeBallColor(PlayerColor PlayerColor){
		switch(PlayerColor){
			case (PlayerColor.red) :
				if(BallColor.Equals(BallColor.white)){
					SetNewBallColor(BallColor.red);
					break;
				}
				if(BallColor.Equals(BallColor.blue)){
					SetNewBallColor(BallColor.purple);
					break;
				}
				if(BallColor.Equals(BallColor.yellow)){
					SetNewBallColor(BallColor.orange);
					break;
				}
				else if(BallColor.Equals(BallColor.purple) || BallColor.Equals(BallColor.green) || BallColor.Equals(BallColor.orange)){
					SetNewBallColor(BallColor.brown);
					break;
				}
				else if(BallColor.Equals(BallColor.brown)){
					SetNewBallColor(BallColor.black);
					break;
				}
			break;
			case (PlayerColor.yellow) :
				if(BallColor.Equals(BallColor.white)){
					SetNewBallColor(BallColor.yellow);
					break;
				}
				if(BallColor.Equals(BallColor.blue)){
					SetNewBallColor(BallColor.green);
					break;
				}
				if(BallColor.Equals(BallColor.red)){
					SetNewBallColor(BallColor.orange);
					break;
				}
				else if(BallColor.Equals(BallColor.purple) || BallColor.Equals(BallColor.green) || BallColor.Equals(BallColor.orange)){
					SetNewBallColor(BallColor.brown);
					break;
				}
				// else if(BallColor.Equals(BallColor.brown)){
				// 	SetNewBallColor(BallColor.black);
				// 	break;
				// }
			break;
			case (PlayerColor.blue) :
				if(BallColor.Equals(BallColor.white)){
					SetNewBallColor(BallColor.blue);
					break;
				}
				if(BallColor.Equals(BallColor.yellow)){
					SetNewBallColor(BallColor.green);
					break;
				}
				if(BallColor.Equals(BallColor.red)){
					SetNewBallColor(BallColor.purple);
					break;
				}
				else if(BallColor.Equals(BallColor.purple) || BallColor.Equals(BallColor.green) || BallColor.Equals(BallColor.orange)){
					SetNewBallColor(BallColor.brown);
					break;
				}
				// else if(BallColor.Equals(BallColor.brown)){
				// 	SetNewBallColor(BallColor.black);
				// 	break;
				// }
			break;
		} 
	}
	public void SetNewBallColor(BallColor InputBallColor){
		OldBallColor = Constants.SetColor(BallColor);
		BallColor = InputBallColor;
		NewBallColor = Constants.SetColor(BallColor);
		TimeLeft = TransitionTime;
		Detection.CheckForMatches();
	}

}