using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Enums;

public class ScoreManager : MonoBehaviour {
public GameManager GameManager;
public PlayerManager PlayerManager;
public Text ScoreText;
public GameObject ComboText;
public GameObject Cursor;
public AudioClip BallPopSound;
public AudioSource AudioSource;
private PunishmentManager PunishmentManager;
public int CurrentRensa;
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
		// if(PlayerManager.RensaTime >= 0){
		// 	RensaOver = false;
		// 	if(!AudioSource.isPlaying && PlayerManager.ScoreMultiplier > TempScoreMultiplier){
		// 		AudioSource.pitch = 0.5f * PlayerManager.ScoreMultiplier;
		// 		AudioSource.Play();
		// 		TempScoreMultiplier = PlayerManager.ScoreMultiplier;
		// 	}
		// 	if(PlayerManager.ScoreMultiplier > 2){
		// 		StopCoroutine("DisplayComboText");
		// 		StartCoroutine("DisplayComboText");
		// 	}
		// 	PlayerManager.RensaTime -= Time.deltaTime;
		// 	if(PlayerManager.RensaTime <= 0 && !RensaOver){
		// 		if(GameManager.NumberOfPlayers > 1 && PlayerManager.ScoreMultiplier > 2 && !PunishmentManager.ShouldPunish){
		// 			PunishmentManager.ShouldPunish = true;
		// 		}
		// 		ResetMultiplierValues();
		// 		RensaOver = true;
		// 	}
		// }
		// if(!AudioSource.isPlaying && PlayerManager.ScoreMultiplier > TempScoreMultiplier){
		// 	AudioSource.pitch = 0.5f * PlayerManager.ScoreMultiplier;
		// 	AudioSource.Play();
		// 	TempScoreMultiplier = PlayerManager.ScoreMultiplier;
		// }
		// if(PlayerManager.ScoreMultiplier > 2){
		// 	StopCoroutine("DisplayComboText");
		// 	StartCoroutine("DisplayComboText");
		// }
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
	public void DeactivateHits(List<GameObject> hits){
		int scoreValue = PointValue(hits[0].GetComponent<Ball>().BallColor);
		int[] rensaValues = hits.Select(hit => hit.GetComponent<Detection>().RensaMultiplier).ToArray();
		int highestRensa = rensaValues.Max();
		PlayerManager.Score += highestRensa * scoreValue * hits.Count;
		float[] ys = hits.Select(hit => hit.transform.localPosition.y).ToArray();
		float highestY = ys.Max();
		foreach(GameObject hit in hits){
			if(hit.transform.localPosition.y == highestY){
				PassRensaMultiplier(hit.transform.localPosition);
			}
		}
		AddToBallCount(hits[0].GetComponent<Ball>().BallColor, hits.Count);
		AddHighestRensa(highestRensa);
		PlayerManager.NumberOfBallsBeingCleared += hits.Count;
		foreach(GameObject Hit in hits){
			Hit.SetActive(false);
		}
	}
	private int PointValue(BallColor ballColor){
		if(ballColor == BallColor.red || ballColor == BallColor.blue || ballColor == BallColor.yellow){
			return 100;
		}
		if(ballColor == BallColor.purple || ballColor == BallColor.green || ballColor == BallColor.orange){
			return 300;
		}
		if(ballColor == BallColor.brown){
			return 50;
		}
		return 0;
	}
	private void PassRensaMultiplier(Vector2 transform){
		RaycastHit2D[] hits = Physics2D.RaycastAll(transform, Vector2.up, 1000, 1 << 8);
		foreach(RaycastHit2D hit in hits){
			if(hit.transform.tag == "Ball" + PlayerManager.PlayerNumberManager.PlayerPrefix){
				hit.transform.GetComponent<Detection>().RensaMultiplier += 1;
				hit.transform.GetComponent<Detection>().RensaTime = 1f;
			}
		}
	}
	#region PlayerStats
	private void AddToBallCount(BallColor color, int numberOfBalls){
		switch(color){
			case BallColor.red:
			PlayerManager.PlayerStats.RedCleared += numberOfBalls;
			break;
			case BallColor.blue:
			PlayerManager.PlayerStats.BlueCleared += numberOfBalls;
			break;
			case BallColor.yellow:
			PlayerManager.PlayerStats.YellowCleared += numberOfBalls;
			break;
			case BallColor.orange:
			PlayerManager.PlayerStats.OrangeCleared += numberOfBalls;
			break;
			case BallColor.purple:
			PlayerManager.PlayerStats.PurpleCleared += numberOfBalls;
			break;
			case BallColor.green:
			PlayerManager.PlayerStats.GreenCleared += numberOfBalls;
			break;
			case BallColor.brown:
			PlayerManager.PlayerStats.BrownCleared += numberOfBalls;
			break;

		}
	}
	private void AddHighestRensa(int rensa){
		if(rensa >= 10){
			PlayerManager.PlayerStats.TenRensa += 1;
		}
		else{
			switch(rensa){
				case 2: PlayerManager.PlayerStats.TwoRensa += 1; break;
				case 3: PlayerManager.PlayerStats.ThreeRensa += 1; break;
				case 4: PlayerManager.PlayerStats.FourRensa += 1; break;
				case 5: PlayerManager.PlayerStats.FiveRensa += 1; break;
				case 6: PlayerManager.PlayerStats.SixRensa += 1; break;
				case 7: PlayerManager.PlayerStats.SevenRensa += 1; break;
				case 8: PlayerManager.PlayerStats.EightRensa += 1; break;
				case 9: PlayerManager.PlayerStats.NineRensa += 1; break;	
			}
		}
	}
	#endregion
}
