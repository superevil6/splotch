using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using System;

public static class Constants {
	public static int Score;
	public static float Time;
	public static int ScoreMultiplier;
	//This is the amount of time you have to continue your rensa.
	public static float AllotedRensaTime = 1.5f;
	public static float RensaTime;

//Weighted Values are processed in this order, WHITE(Ignored) BLACK BROWN GREEN PURPLE ORANGE RED BLUE YELLOW, if it doesn't proc any of those, it's white
public static int[] DefaultColorWeights = {0, 5, 10, 0, 0, 0, 20, 20};
public static bool RensaCheck = false;

	public static BallColor GenerateColor(){
		Array Colors = Enum.GetValues(typeof(BallColor));
		return (BallColor)Colors.GetValue(UnityEngine.Random.Range(0, Colors.Length));
	}
	public static PlayerColor GeneratePlayerColor(){
		Array Colors = Enum.GetValues(typeof(PlayerColor));
		return (PlayerColor)Colors.GetValue(UnityEngine.Random.Range(0, Colors.Length));
	}
	public static PlayerColor GenerateNonConsecutiveColor(PlayerColor AlreadyUsedColor){
		PlayerColor ChosenColor = GeneratePlayerColor();
		while(ChosenColor == AlreadyUsedColor){
			ChosenColor = GeneratePlayerColor();
		}
		return ChosenColor;
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
	public static Color32 SetColor(PlayerColor PlayerColor){
			switch(PlayerColor){
				case PlayerColor.red : 
					return Color.red;
				case PlayerColor.blue : 
					return Color.blue;
				case PlayerColor.yellow : 
					return Color.yellow;
		}
		return Color.grey;
	}
	public static Vector2 FindOffset(GameObject GO){
		float XOffset = GO.GetComponent<RectTransform>().rect.width;
		float YOffset = GO.GetComponent<RectTransform>().rect.height;
		return new Vector2(XOffset, YOffset);
	}

	
}
