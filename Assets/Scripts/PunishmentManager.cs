using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunishmentManager : MonoBehaviour
{
    private GameManager GameManager;
    private PlayerManager PlayerManager;
    public bool ShouldPunish = false;
    private List<PlayerManager> Players = new List<PlayerManager>();
    // Start is called before the first frame update
    void Start()
    {
        GameManager = GetComponentInParent<GameManager>();
        PlayerManager = GetComponentInParent<PlayerManager>();
        if(GameManager.NumberOfPlayers > 1){
            foreach(GameObject player in GameManager.Players){
                if(player.GetComponent<PlayerManager>().PlayerNumber != PlayerManager.PlayerNumber){
                    Players.Add(player.GetComponent<PlayerManager>());
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.NumberOfPlayers > 1 && ShouldPunish){
            print("Hi, this is happening");
            PunishOtherPlayer(PickPlayerToPunish(), PlayerManager.NumberOfSecondsForPunishment, PlayerManager.NumberOfBallsBeingCleared);
            PlayerManager.NumberOfSecondsForPunishment = 0;
            PlayerManager.NumberOfBallsBeingCleared = 0;
            ShouldPunish = false;
        }
    }
	public IEnumerator PunishThisPlayer(float time, float speed){
		float originalSpeed = PlayerManager.DropSpeed;
		PlayerManager.DropSpeed += (4*speed)/10;
		yield return new WaitForSeconds(time);
		PlayerManager.DropSpeed = originalSpeed;
	}
    public PlayerManager PickPlayerToPunish(){
        if(Players.Count > 0){
            int chosenPlayer = UnityEngine.Random.Range(0, Players.Count);
            print(PlayerManager.PlayerNumber + " is Punishing player " + Players[chosenPlayer].PlayerNumber);
            return Players[chosenPlayer];
        }
        return null;
    }
    public void PunishOtherPlayer(PlayerManager otherPlayer, float length, float speedIncrease){
        otherPlayer.StartCoroutine(PunishThisPlayer(length, speedIncrease));
    }
}
