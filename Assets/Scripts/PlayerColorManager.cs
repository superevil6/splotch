using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

public class PlayerColorManager : MonoBehaviour {
public List<PlayerColor> ColorQueue;
public Image FirstColorPanel; 
public Image SecondColorPanel; 
public Image ThirdColorPanel; 

	// Use this for initialization
	void Start () {
		ColorQueue = new List<PlayerColor>();
		FirstColorPanel = FirstColorPanel.GetComponent<Image>();
		SecondColorPanel = SecondColorPanel.GetComponent<Image>();
		ThirdColorPanel = ThirdColorPanel.GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		if(ColorQueue.Count < 3){
			ColorQueue.Add(Constants.GeneratePlayerColor());
		}

		else{
			FirstColorPanel.color = Constants.SetColor(ColorQueue[0]);
			SecondColorPanel.color = Constants.SetColor(ColorQueue[1]);
			ThirdColorPanel.color = Constants.SetColor(ColorQueue[2]);
		}
	}
		public void UpdateColorQueue(){
		ColorQueue.Remove(ColorQueue[0]);
		if(ColorQueue[0] == ColorQueue[1]){
			ColorQueue.Add(Constants.GenerateNonConsecutiveColor(ColorQueue[0]));
		}
		else{
			ColorQueue.Add(Constants.GeneratePlayerColor());
		}
	}
}
