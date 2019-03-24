using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> Players;
    public GameObject PlayerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void InitializePlayers(int numberOfPlayers){
        for(int i = 0; i <= numberOfPlayers; i++){
            Players.Add(InitializePlayer(i));
        }
    }
    public GameObject InitializePlayer(int playerNumber){
        GameObject newPlayer = (GameObject)Instantiate(PlayerPrefab);
        PlayerManager playerManager = newPlayer.GetComponent<PlayerManager>();
        switch(playerNumber){
            case 0 :
            playerManager.playerNumber = PlayerNumber.one;
            break;
            case 1:
            playerManager.playerNumber = PlayerNumber.two;
            break;
            case 2:
            playerManager.playerNumber = PlayerNumber.three;
            break;
            case 3: 
            playerManager.playerNumber = PlayerNumber.four;
            break;
        }
         
        //Add in component connections!!
        return newPlayer;
    }
}
