using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {
public Text ScoreText;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		ScoreText.text = Constants.Score.ToString();
		if(Constants.RensaTime > 0){
			Constants.RensaTime -= Time.deltaTime;
		}
		else{
			print("Times up, multiplier was: " + Constants.ScoreMultiplier);
			Constants.ScoreMultiplier = 1;
		}
	}
}
