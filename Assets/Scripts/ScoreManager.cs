using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {
public GameManager GameManager;
public PlayerManager PlayerManager;
public Text ScoreText;
public GameObject ComboText;
public GameObject Cursor;
public AudioClip BallPopSound;
public AudioSource AudioSource;
private PunishmentManager PunishmentManager;
private bool RensaOver = true;
private int TempScoreMultiplier = 1;
	// Use this for initialization
	void Start () {
		GameManager = GetComponentInParent<GameManager>();
		ComboText.SetActive(false);
		AudioSource.clip = BallPopSound;
		PunishmentManager = PlayerManager.PunishmentManager;
	}
	
	// Update is called once per frame
	void Update () {
		ScoreText.text = PlayerManager.Score.ToString();
		if(PlayerManager.RensaTime >= 0){
			RensaOver = false;
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
			if(PlayerManager.RensaTime <= 0 && !RensaOver){
				if(GameManager.NumberOfPlayers > 1 && PlayerManager.ScoreMultiplier > 2 && !PunishmentManager.ShouldPunish){
					PunishmentManager.ShouldPunish = true;
				}
				ResetMultiplierValues();
				RensaOver = true;
			}
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
	public void ResetMultiplierValues(){
		if(GameManager.NumberOfPlayers > 1){
			PunishmentManager.PunishOtherPlayer(PunishmentManager.PickPlayerToPunish(), PlayerManager.ScoreMultiplier, PlayerManager.NumberOfBallsBeingCleared);
		}
		PlayerManager.ScoreMultiplier = 1;
		TempScoreMultiplier = PlayerManager.ScoreMultiplier;			
		AudioSource.pitch = 1f;
	}
}
