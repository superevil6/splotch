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
        foreach(GameObject player in GameManager.Players){
            if(player.GetComponent<PlayerManager>().PlayerNumber != PlayerManager.PlayerNumber){
                Players.Add(player.GetComponent<PlayerManager>());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(ShouldPunish){
            print("PUnishing");
            PunishOtherPlayer(PickPlayerToPunish(), PlayerManager.NumberOfSecondsForPunishment, PlayerManager.NumberOfBallsBeingCleared);
            PlayerManager.NumberOfSecondsForPunishment = 0;
            PlayerManager.NumberOfBallsBeingCleared = 0;
            ShouldPunish = false;
        }
    }
	public IEnumerator PunishThisPlayer(float time, float speed){
        //print("time" + time + "speed: " + speed);
		float originalSpeed = PlayerManager.DropSpeed;
        //print("original speed: " + originalSpeed);
		PlayerManager.DropSpeed += (4*speed)/10;
        //print("Drop speed = " + PlayerManager.DropSpeed);
		yield return new WaitForSeconds(time);
		PlayerManager.DropSpeed = originalSpeed;
        //print("Returning to original drop speed: " + PlayerManager.DropSpeed);
	}
    public PlayerManager PickPlayerToPunish(){
        print("picking player to punish");
        if(Players.Count > 0){
            int chosenPlayer = UnityEngine.Random.Range(0, Players.Count);
            return Players[chosenPlayer];
        }
        return null;
    }
    public void PunishOtherPlayer(PlayerManager otherPlayer, float length, float speedIncrease){
        otherPlayer.StartCoroutine(PunishThisPlayer(length, speedIncrease));
    }
}
