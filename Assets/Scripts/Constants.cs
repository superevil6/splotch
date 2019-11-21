using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
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
}
