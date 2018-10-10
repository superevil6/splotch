using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Constants {
	public static int Score;
	public static float Time;
	public static int ScoreMultiplier;
	//This is the amount of time you have to continue your rensa.
	public static float AllotedRensaTime = 1.5f;
	public static float RensaTime;

//Weighted Values are processed in this order, WHITE(Ignored) BLACK BROWN GREEN PURPLE ORANGE RED BLUE YELLOW, if it doesn't proc any of those, it's white
public static int[] DefaultColorWeights = {5, 5, 10, 10, 10, 20, 20, 20};
public static bool RensaCheck = false;

	public static Enums.BallColor GenerateColor(){
		Array Colors = Enum.GetValues(typeof(Enums.BallColor));
		return (Enums.BallColor)Colors.GetValue(UnityEngine.Random.Range(0, Colors.Length));
	}
	public static Enums.PlayerColor GeneratePlayerColor(){
		Array Colors = Enum.GetValues(typeof(Enums.PlayerColor));
		return (Enums.PlayerColor)Colors.GetValue(UnityEngine.Random.Range(0, Colors.Length));
	}
	public static Enums.BallColor WeightedGenerateColor(int[] WeightedValues){
		Array Colors = Enum.GetValues(typeof(Enums.BallColor));
		for(int i = 0; i < WeightedValues.Length; i++){
			int RNG = UnityEngine.Random.Range(0, 100);
			if(RNG < WeightedValues[i]){
				return (Enums.BallColor)Colors.GetValue(i + 1);
			}
			continue;
		}
		return Enums.BallColor.white;
	}
	public static Color32 SetColor(Enums.BallColor BallColor){
			switch(BallColor){
				case Enums.BallColor.white : 
					return Color.white;
				case Enums.BallColor.red : 
					return Color.red;
				case Enums.BallColor.blue : 
					return Color.blue;
				case Enums.BallColor.yellow : 
					return Color.yellow;
				case Enums.BallColor.green : 
					return Color.green;
				case Enums.BallColor.orange : 
					return new Color32(255,165,0,255);
				case Enums.BallColor.purple : 
					return new Color32(128,0,128,255);
				case Enums.BallColor.brown : 
					return new Color32(128,80,0,255);
				case Enums.BallColor.black : 
					return Color.black;
		}
		return Color.grey;
	}
	public static Color32 SetColor(Enums.PlayerColor PlayerColor){
			switch(PlayerColor){
				case Enums.PlayerColor.red : 
					return Color.red;
				case Enums.PlayerColor.blue : 
					return Color.blue;
				case Enums.PlayerColor.yellow : 
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
