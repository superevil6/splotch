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




	public static Vector2 FindOffset(GameObject GO){
		float XOffset = GO.GetComponent<RectTransform>().rect.width;
		float YOffset = GO.GetComponent<RectTransform>().rect.height;
		return new Vector2(XOffset, YOffset);
	}

	
}
