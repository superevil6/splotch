using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> Players;
    public GameObject PlayerPrefab;
    public List<Theme> Themes = new List<Theme>();
    public WeightedBallPools WeightedBallPool;
    public List<BallColor> TwoColorInitialBalls;
    public List<BallColor> ThreeColorInitialBalls;
    public int NumberOfPlayers;
    public int InitialBallCount;
    //public int[] WeightBallPool;
    // Start is called before the first frame update
    void Start()
    {
        NumberOfPlayers = GameOptions.PlayerNumber;

        InitializePlayers(NumberOfPlayers);
		TwoColorInitialBalls = new List<BallColor>();
		ThreeColorInitialBalls = new List<BallColor>();
        GenerateInitialBallList(false);
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
            if(GameOptions.PlayerOne == PlayerType.CPU){
                newPlayer.GetComponentInChildren<Cursor>().GetComponent<CPU>().enabled = true;
                //newPlayer.AddComponent<CPU>();
                //playerManager.GetComponentInChildren<Cursor>().gameObject.AddComponent<CPU>();
                //CPU cpu = newPlayer GetComponentInChildren<Cursor>.AddComponent(typeof(CPU)) as CPU;
            }
            break;
            case 1:
            playerManager.PlayerNumber = PlayerNumber.two;
            if(GameOptions.PlayerTwo == PlayerType.CPU){
                newPlayer.GetComponentInChildren<Cursor>().GetComponent<CPU>().enabled = true;

                // CPU cpu = newPlayer.gameObject.AddComponent(typeof(CPU)) as CPU;
            }
            break;
            case 2:
            playerManager.PlayerNumber = PlayerNumber.three;
            if(GameOptions.PlayerThree == PlayerType.CPU){
                newPlayer.GetComponentInChildren<Cursor>().GetComponent<CPU>().enabled = true;

                // CPU cpu = newPlayer.gameObject.AddComponent(typeof(CPU)) as CPU;
            }
            break;
            case 3: 
            playerManager.PlayerNumber = PlayerNumber.four;
            if(GameOptions.PlayerFour == PlayerType.CPU){
                newPlayer.GetComponentInChildren<Cursor>().GetComponent<CPU>().enabled = true;

                // CPU cpu = newPlayer.gameObject.AddComponent(typeof(CPU)) as CPU;
            }
            break;
        }
         
        //Add in component connections!!
        return newPlayer;
    }
	public void GenerateInitialBallList(bool threeColors){
		for(int i = 0; i < InitialBallCount; i++){
			ThreeColorInitialBalls.Add(WeightedBallPool.RandomBallColor());
		}
	}
}
