using System;
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
public PlayerManager PlayerManager;
public BoxCollider2D BoxCollider2D;
public Rigidbody2D Rigidbody2D;
public GameBoard GameBoard;
private Color32 OldBallColor;
private Color32 NewBallColor;
public BallColor ColorToIgnore;
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
		Detection = GetComponent<Detection>();
		//GameBoard.Scale = 1f - GameBoard.Columns * 0.10f;
		// transform.localScale = new Vector2((transform.localScale.x * GameBoard.Scale /2), (transform.localScale.y * GameBoard.Scale) /2);
		// BoxCollider2D.size = new Vector2(15 * transform.localScale.x, 15 * transform.localScale.y);
		transform.localScale = new Vector2((GameBoard.GameboardWidth / GameBoard.Columns) * 0.5f, ((GameBoard.GameboardHeight) / GameBoard.Columns) * 0.5f);
		//BoxCollider2D.size = new Vector2((GameBoard.GameboardWidth / GameBoard.Columns) * 0.25f, ((GameBoard.GameboardHeight) / GameBoard.Columns) * 0.25f);
		
	}

	void OnEnable () {

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
		ChangeBallColor(PlayerManager.PlayerColorManager.ColorQueue[0]);
		PlayerManager.PlayerColorManager.UpdateColorQueue();
	}
	public static BallColor GenerateColor(){
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
	public static BallColor WeightedGenerateColor(int[] WeightedValues){
		Array Colors = Enum.GetValues(typeof(BallColor));
		for(int i = 0; i < WeightedValues.Length; i++){
			int RNG = UnityEngine.Random.Range(0, 100);
			if(RNG < WeightedValues[i]){
				return (BallColor)Colors.GetValue(i + 1);
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
		//gameObject.GetComponent<RectTransform>();
		SpriteRenderer.sprite = Sprite;
		DetermineColor();
	}

	public void DetermineColor(){
		BallColor = WeightedGenerateColor(Constants.DefaultColorWeights);
		//BallColor = GenerateColor(ColorToIgnore);
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

}