using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {
public PlayerManager PlayerManager;
public Text ScoreText;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		ScoreText.text = PlayerManager.Score.ToString();
		if(PlayerManager.RensaTime > 0){
			PlayerManager.RensaTime -= Time.deltaTime;
		}
		else{
			print("Times up, multiplier was: " + PlayerManager.ScoreMultiplier);
			PlayerManager.ScoreMultiplier = 1;
		}
	}
}
