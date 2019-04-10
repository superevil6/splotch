using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

public class Ball : MonoBehaviour {
public BallColor BallColor;
//public BallColor IgnoredColor;
public int[] WeightedBallColorPool;
public Detection Detection;
private BallColor NewColor;
public BallType Type;
public SpriteRenderer SpriteRenderer;
public Sprite Sprite;
public PlayerManager PlayerManager;
public BoxCollider2D BoxCollider2D;
public Rigidbody2D Rigidbody2D;
public GameBoard GameBoard;
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
	void Start () {
		GameBoard = GetComponentInParent<GameBoard>();
		PlayerManager = GetComponentInParent<PlayerManager>();
		WeightedBallColorPool = PlayerManager.WeightedBallColorPool;
		Detection = GetComponent<Detection>();
		transform.localScale = new Vector2((GameBoard.GameboardWidth / GameBoard.Columns) * 0.5f, ((GameBoard.GameboardHeight) / GameBoard.Columns) * 0.5f);
		SetupBall();		
	}

	void OnEnable () {
		TimeLeft = 0;
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
		if(PlayerManager.GameOver){
			SetNewBallColor(BallColor.black);
		}
	}

	// void OnMouseDown()
	// {
	// 	ChangeBallColor(PlayerManager.PlayerColorManager.ColorQueue[0]);
	// 	PlayerManager.PlayerColorManager.UpdateColorQueue();
	// }
	public BallColor GenerateColor(){
		Array Colors = Enum.GetValues(typeof(BallColor));
		return (BallColor)Colors.GetValue(UnityEngine.Random.Range(0, Colors.Length));
	}
	public BallColor GenerateColor(BallColor ColorToIgnore){
		Array Colors = Enum.GetValues(typeof(PlayerColor));
		BallColor ColorToReturn = ColorToIgnore;
		while(ColorToReturn == ColorToIgnore){
			ColorToReturn = (BallColor)Colors.GetValue(UnityEngine.Random.Range(0, Colors.Length));
		}
		return ColorToReturn;
	}
	public BallColor WeightedGenerateColor(){
		Array Colors = Enum.GetValues(typeof(BallColor));
		for(int i = 0; i < WeightedBallColorPool.Length; i++){
			int RNG = UnityEngine.Random.Range(0, 100);
			if(RNG < WeightedBallColorPool[i]){
				return (BallColor)Colors.GetValue(i);
			}
			continue;
		}
		return BallColor.white;
	}
		public static Color32 SetColor(BallColor BallColor){
			switch(BallColor){
				case BallColor.white : 
					return Color.white;
				case BallColor.red : 
					return Color.red;
				case BallColor.blue : 
					return Color.blue;
				case BallColor.yellow : 
					return Color.yellow;
				case BallColor.green : 
					return Color.green;
				case BallColor.orange : 
					return new Color32(255,165,0,255);
				case BallColor.purple : 
					return new Color32(128,0,128,255);
				case BallColor.brown : 
					return new Color32(128,80,0,255);
				case BallColor.black : 
					return Color.black;
		}
		return Color.grey;
	}
	private void SetupBall(){
		SpriteRenderer = GetComponent<SpriteRenderer>();
		BoxCollider2D = GetComponent<BoxCollider2D>();
		SpecialBallCheck(false);
		SpriteRenderer.sprite = Sprite;
		if(Type != BallType.rainbow){
			//This is a temp fix if you can't fix the issue that lets you click brown balls.
			//NewBallColor = OldBallColor;
			DetermineColor();
		}
	}

	public void DetermineColor(){
		BallColor = WeightedGenerateColor();
		NewColor = BallColor;
		SpriteRenderer.color = SetColor(BallColor);
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
				// else if(BallColor.Equals(BallColor.brown)){
				// 	SetNewBallColor(BallColor.black);
				// 	break;
				// }
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
		OldBallColor = SetColor(BallColor);
		BallColor = InputBallColor;
		NewBallColor = SetColor(BallColor);
		TimeLeft = TransitionTime;
		Detection.CheckForMatches();
	}
	public BallType SpecialBallCheck(bool BattleItems){
		int RNG = UnityEngine.Random.Range(0, 100);
		if(RNG < 1){
			return BallType.rainbow;
		}
		RNG = UnityEngine.Random.Range(0, 100);
		if(BattleItems && RNG < 1){
			return BallType.powerup;
		}
		return BallType.normal;
	}
}