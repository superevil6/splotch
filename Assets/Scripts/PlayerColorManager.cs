using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;
using System;

public class PlayerColorManager : MonoBehaviour {
public PlayerManager PlayerManager;
private ColorScheme ColorScheme;
public List<PlayerColor> ColorQueue;
public Image FirstColorPanel; 
public Image SecondColorPanel; 
public Image ThirdColorPanel; 

	// Use this for initialization
	void Start () {
		ColorScheme = PlayerManager.ColorScheme;
		ColorQueue = new List<PlayerColor>();
		FirstColorPanel = FirstColorPanel.GetComponent<Image>();
		SecondColorPanel = SecondColorPanel.GetComponent<Image>();
		ThirdColorPanel = ThirdColorPanel.GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		if(ColorQueue.Count < 3){
			AddColorToQueue();
		}

		else{
			FirstColorPanel.color = SetColor(ColorQueue[0]);
			SecondColorPanel.color = SetColor(ColorQueue[1]);
			ThirdColorPanel.color = SetColor(ColorQueue[2]);
		}
	}
	public PlayerColor GeneratePlayerColor(){
		Array Colors = Enum.GetValues(typeof(PlayerColor));
		return (PlayerColor)Colors.GetValue(UnityEngine.Random.Range(0, Colors.Length));
	}
	public PlayerColor GeneratePlayerColor(PlayerColor ColorToIgnore){
		Array Colors = Enum.GetValues(typeof(PlayerColor));
		PlayerColor ColorToReturn = ColorToIgnore;
		while(ColorToReturn == ColorToIgnore){
			ColorToReturn = (PlayerColor)Colors.GetValue(UnityEngine.Random.Range(0, Colors.Length));
		}
		return ColorToReturn;
	}
		public void UpdateColorQueue(){
		ColorQueue.Remove(ColorQueue[0]);
		if(ColorQueue[0] == ColorQueue[1]){
			ColorQueue.Add(GenerateNonConsecutiveColor(ColorQueue[0]));
		}
		else{
			AddColorToQueue();
		}
	}
	public Color32 SetColor(PlayerColor playerColor){
		switch(playerColor){
			case PlayerColor.red : 
				return ColorScheme.Red;
			case PlayerColor.blue : 
				return ColorScheme.Blue;
			case PlayerColor.yellow : 
				return ColorScheme.Yellow;
		}
		return Color.grey;
	}
	public PlayerColor GenerateNonConsecutiveColor(PlayerColor AlreadyUsedColor){
		PlayerColor ChosenColor = GeneratePlayerColor(PlayerManager.IgnoredPlayerColor);
		while(ChosenColor == AlreadyUsedColor){
			ChosenColor = GeneratePlayerColor(PlayerManager.IgnoredPlayerColor);
		}
		return ChosenColor;
	}
	public void AddColorToQueue(){
		if(PlayerManager.Difficulty == Difficulty.VeryEasy || PlayerManager.Difficulty == Difficulty.Easy){
			ColorQueue.Add(GeneratePlayerColor(PlayerManager.IgnoredPlayerColor));
		}
		else{
			ColorQueue.Add(GeneratePlayerColor());
		}
	}
}
