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
public PlayerManager PlayerManager;
public ColorScheme ColorScheme;
public Detection Detection;
private BallColor NewColor;
public BallType Type;
public SpriteRenderer SpriteRenderer;
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
		ColorScheme = PlayerManager.ColorScheme;
		WeightedBallColorPool = PlayerManager.WeightedBallColorPool;
		Detection = GetComponent<Detection>();
		transform.localScale = new Vector2((GameBoard.GameboardWidth / GameBoard.Columns) * 0.5f, ((GameBoard.GameboardHeight) / GameBoard.Columns) * 0.5f);
		gameObject.layer = 8;
		SetupBall();
		SpriteRenderer.sprite = PickSprite(PlayerManager.Theme.BallSprite);
		
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
	public Color SetColor(BallColor ballColor){
		switch(ballColor){
			case BallColor.white : 
				return ColorScheme.White;
			case BallColor.red : 
				return ColorScheme.Red;
			case BallColor.blue : 
				return ColorScheme.Blue;
			case BallColor.yellow : 
				return ColorScheme.Yellow;
			case BallColor.green : 
				return ColorScheme.Green;
			case BallColor.orange : 
				return ColorScheme.Orange;
			case BallColor.purple : 
				return ColorScheme.Purple;
			case BallColor.brown : 
				return ColorScheme.Brown;
			case BallColor.black : 
				return ColorScheme.Black;
		}
		return Color.white;
	}
	private void SetupBall(){
		SpriteRenderer = GetComponent<SpriteRenderer>();
		BoxCollider2D = GetComponent<BoxCollider2D>();
		SpecialBallCheck(false);
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
	public Sprite PickSprite(Sprite[] sprites){
		int chosenSprite = UnityEngine.Random.Range(0, sprites.Length);
		return sprites[chosenSprite];
	}
}

/*
Coroutine couldn't be started because the the game object 'Ball(Clone)' is inactive!
UnityEngine.MonoBehaviour:StartCoroutine(IEnumerator)
Detection:CheckForMatches() (at Assets/Scripts/Detection.cs:80)
Ball:SetNewBallColor(BallColor) (at Assets/Scripts/Ball.cs:202)
Ball:ChangeBallColor(PlayerColor) (at Assets/Scripts/Ball.cs:161)
CPU:ChangeColor(Ball) (at Assets/Scripts/CPU.cs:225)
CPU:Update() (at Assets/Scripts/CPU.cs:95)

*/