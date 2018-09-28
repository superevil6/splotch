using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Constants {

	public static Enums.BallColor GenerateColor(){
		Array Colors = Enum.GetValues(typeof(Enums.BallColor));
		return (Enums.BallColor)Colors.GetValue(UnityEngine.Random.Range(0, Colors.Length));
	}
	public static Enums.PlayerColor GeneratePlayerColor(){
		Array Colors = Enum.GetValues(typeof(Enums.PlayerColor));
		return (Enums.PlayerColor)Colors.GetValue(UnityEngine.Random.Range(0, Colors.Length));
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
					return new Color32(255,69,0,255);
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
}
