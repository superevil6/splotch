using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> Players;
    public GameObject PlayerPrefab;
    public List<Theme> Themes = new List<Theme>();
    public int NumberOfPlayers;
    // Start is called before the first frame update
    void Start()
    {
        InitializePlayers(NumberOfPlayers);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void InitializePlayers(int numberOfPlayers){
        numberOfPlayers -= 1;
        for(int i = 0; i <= numberOfPlayers; i++){
            Players.Add(InitializePlayer(i));
        }
        foreach(GameObject Player in Players){
            Player.transform.SetParent(transform);
            Player.transform.position = new Vector3(0, 0, 1);
            Player.transform.localScale = new Vector3(1,1,1);
            if(NumberOfPlayers == 3){
            Player.transform.localScale = new Vector3(0.75f, 1, 1);
            }
            if(NumberOfPlayers == 4){
            Player.transform.localScale = new Vector3(0.5f, 1, 1);
            }
        }
    }
    public GameObject InitializePlayer(int playerNumber){
        GameObject newPlayer = (GameObject)Instantiate(PlayerPrefab);
        PlayerManager playerManager = newPlayer.GetComponent<PlayerManager>();
        switch(playerNumber){
            case 0 :
            playerManager.PlayerNumber = PlayerNumber.one;
            break;
            case 1:
            playerManager.PlayerNumber = PlayerNumber.two;
            break;
            case 2:
            playerManager.PlayerNumber = PlayerNumber.three;
            break;
            case 3: 
            playerManager.PlayerNumber = PlayerNumber.four;
            break;
        }
         
        //Add in component connections!!
        return newPlayer;
    }
}
