using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {
public PlayerManager PlayerManager;
public Text ScoreText;
public GameObject ComboText;
public GameObject Cursor;
public AudioClip BallPopSound;
public AudioSource AudioSource;
private int TempScoreMultiplier = 1;
	// Use this for initialization
	void Start () {
		ComboText.SetActive(false);
		AudioSource.clip = BallPopSound;
	}
	
	// Update is called once per frame
	void Update () {
		ScoreText.text = PlayerManager.Score.ToString();
		if(PlayerManager.RensaTime > 0){
			if(!AudioSource.isPlaying && PlayerManager.ScoreMultiplier > TempScoreMultiplier){
				AudioSource.pitch = 0.5f * PlayerManager.ScoreMultiplier;
				AudioSource.Play();
				TempScoreMultiplier = PlayerManager.ScoreMultiplier;
			}
			if(PlayerManager.ScoreMultiplier > 2){
				StopCoroutine("DisplayComboText");
				StartCoroutine("DisplayComboText");
			}
			PlayerManager.RensaTime -= Time.deltaTime;
		}
		else{
			PlayerManager.ScoreMultiplier = 1;
			TempScoreMultiplier = PlayerManager.ScoreMultiplier;			
			AudioSource.pitch = 1f;
		}
	}
	public IEnumerator DisplayComboText(){
		ComboText.GetComponent<Text>().text = "X" + (PlayerManager.ScoreMultiplier -1) + " Combo";
		ComboText.GetComponent<Text>().fontSize = 10 * PlayerManager.ScoreMultiplier;
		ComboText.transform.position = new Vector2(Cursor.transform.position.x + 0.5f, Cursor.transform.position.y + 0.5f);
		ComboText.SetActive(true);
		yield return new WaitForSeconds(1);
		ComboText.SetActive(false);
	}
	public IEnumerator PlaySound(AudioClip clip){
		AudioSource.Play();
		yield return new WaitForSeconds(clip.length);
	}
}
