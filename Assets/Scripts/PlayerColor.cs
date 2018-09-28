using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerColor : MonoBehaviour {
public GameObject NextColorPanel;
public Enums.PlayerColor NextColor;
public Image NextColorPanelColor; 

	// Use this for initialization
	void Start () {
		NextColorPanelColor = NextColorPanel.GetComponent<Image>();
		NextColor = Constants.GeneratePlayerColor();
		NextColorPanelColor.color = Constants.SetColor(NextColor);
	}
	
	// Update is called once per frame
	void Update () {
		NextColorPanelColor.color = Constants.SetColor(NextColor);
	}

}
