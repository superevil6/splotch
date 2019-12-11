using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using System.Linq;
using System;

public static class Constants {
	public static Vector2 FindOffset(GameObject GO){
		float XOffset = GO.GetComponent<RectTransform>().rect.width;
		float YOffset = GO.GetComponent<RectTransform>().rect.height;
		return new Vector2(XOffset, YOffset);
	}
	public static BallColor PlayerColorToBallColor(PlayerColor playerColor){
		switch(playerColor){
			case PlayerColor.blue:
			return BallColor.blue;
			case PlayerColor.red:
			return BallColor.red;
			case PlayerColor.yellow:
			return BallColor.yellow;
		}
		return BallColor.blue;
	}
	public static PlayerColor BallColorToPlayerColor(BallColor ballColor){
		switch(ballColor){
			case BallColor.blue:
			return PlayerColor.blue;
			case BallColor.red:
			return PlayerColor.red;
			case BallColor.yellow:
			return PlayerColor.yellow;
		}
		return PlayerColor.blue;
	}
	public static List<GameObject> RaycastAllDirections(GameObject GO){
		List<GameObject> Hits = new List<GameObject>();
		Hits.AddRange(FindGameObjectsFromRaycast(Physics2D.RaycastAll(GO.transform.position, Vector2.up, 100, 1 << 8)));
		Hits.AddRange(FindGameObjectsFromRaycast(Physics2D.RaycastAll(GO.transform.position, -Vector2.up, 100, 1 << 8)));
		Hits.AddRange(FindGameObjectsFromRaycast(Physics2D.RaycastAll(GO.transform.position, -Vector2.right, 100, 1 << 8)));
		Hits.AddRange(FindGameObjectsFromRaycast(Physics2D.RaycastAll(GO.transform.position, Vector2.right, 100, 1 << 8)));
		Hits.AddRange(FindGameObjectsFromRaycast(Physics2D.RaycastAll(GO.transform.position, new Vector2(-1, 1), 100, 1 << 8)));
		Hits.AddRange(FindGameObjectsFromRaycast(Physics2D.RaycastAll(GO.transform.position, new Vector2(1, 1), 100, 1 << 8)));
		Hits.AddRange(FindGameObjectsFromRaycast(Physics2D.RaycastAll(GO.transform.position, new Vector2(-1, -1), 100, 1 << 8)));
		Hits.AddRange(FindGameObjectsFromRaycast(Physics2D.RaycastAll(GO.transform.position, new Vector2(1, -1), 100, 1 << 8)));
		return Hits;		
	}
	public static List<GameObject> FindGameObjectsFromRaycast(RaycastHit2D[] hits){
		List<GameObject> gameObjects = new List<GameObject>();
		if(hits.Length > 0){
			foreach(RaycastHit2D hit in hits){
				gameObjects.Add(hit.transform.gameObject);
			}
		}
		return gameObjects;
	}
	public static Color FindColor(BallColor color, ColorScheme colorScheme){
		switch(color){
			case BallColor.red:
			return colorScheme.Red;
			case BallColor.blue:
			return colorScheme.Blue;
			case BallColor.yellow:
			return colorScheme.Yellow;
			case BallColor.green:
			return colorScheme.Green;
			case BallColor.orange:
			return colorScheme.Orange;
			case BallColor.purple:
			return colorScheme.Purple;
			case BallColor.brown:
			return colorScheme.Brown;
			case BallColor.white:
			return colorScheme.White;
			case BallColor.black:
			return colorScheme.Black;
			default: return colorScheme.White;
		}
	}
	public static Color FindColor(PlayerColor color, ColorScheme colorScheme){
		switch(color){
			case PlayerColor.red:
			return colorScheme.Red;
			case PlayerColor.blue:
			return colorScheme.Blue;
			case PlayerColor.yellow:
			return colorScheme.Yellow;
			default: return colorScheme.White;
		}
	}
	public static BallColor RandomBallColor(){
		var Colors = Enum.GetValues(typeof(BallColor)); //.Cast<BallColor>().ToList();
		int index = UnityEngine.Random.Range(0, Colors.Length);
		return (BallColor)Colors.GetValue(index);
	}
	public static BallColor RandomWeightedBallColor(int[] WeightedBallColorPool){
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
}
