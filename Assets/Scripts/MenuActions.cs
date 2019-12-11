using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Enums;

public class MenuActions : MonoBehaviour
{
    public Text PlayerOneText;
    public Text PlayerTwoText;
    public Text PlayerThreeText;
    public Text PlayerFourText;
    private SceneManager SceneManager;
    public string[] PlayerOneOptions = {"P1", "CPU", "None"};
    public string[] PlayerTwoOptions = {"P2", "CPU", "None"};
    public string[] PlayerThreeOptions = {"P3", "CPU", "None"};
    public string[] PlayerFourOptions = {"P4", "CPU", "None"};
    public int PlayerOneIndex = 0;
    public int PlayerTwoIndex = 0;
    public int PlayerThreeIndex = 0;
    public int PlayerFourIndex = 0;
    private int CurrentScene;

    //Mission Mode Variables
    public Mission[] Missions;
    public Mission ChosenMission;
    public GameObject MissionPanel;
    public Button MissionButton;
    public Text MissionDescription;

    // Start is called before the first frame update
    void Start()
    {
        CurrentScene = SceneManager.GetActiveScene().buildIndex;
        if(CurrentScene == 0){
            GameOptions.PlayerNumber = 0;
        }
        GenerateMissions(Missions);

    }

    // Update is called once per frame
    void Update()
    {
        if(CurrentScene == 0){
            PlayerOneText.text = PlayerOneOptions[PlayerOneIndex];
            PlayerTwoText.text = PlayerTwoOptions[PlayerTwoIndex];
            PlayerThreeText.text = PlayerThreeOptions[PlayerThreeIndex];
            PlayerFourText.text = PlayerFourOptions[PlayerFourIndex];
        }
    }
    //Unity is dumb and doesn't let you use enums as inputs in the editor, so I have to make all of the methods for game modes individually. 
    public void SetArcadeMode(){
        GameOptions.PlayerNumber = 1;
        GameOptions.GameMode = GameMode.Arcade;
    }
    public void SetMissionMode(){
        GameOptions.PlayerNumber = 1;
        GameOptions.GameMode = GameMode.Mission;
    }
    public void SetMarathonMode(){
        GameOptions.PlayerNumber = 1;
        GameOptions.GameMode = GameMode.Marathon;
    }
    public void SetPuzzleMode(){
        GameOptions.PlayerNumber = 1;
        GameOptions.GameMode = GameMode.Puzzle;
    }
    public void SetStoryMode(){
        GameOptions.PlayerNumber = 1;
        GameOptions.GameMode = GameMode.Story;
    }
    public void SetVSMode(){
        GameOptions.GameMode = GameMode.VS;
    }
    public void SetBattleMode(){
        GameOptions.GameMode = GameMode.Battle;
    }
    public void SetPlayerType(int playerNumber, PlayerType type){
        switch(playerNumber){
            case 1:
            GameOptions.PlayerOne = type;
            break;
            case 2:
            GameOptions.PlayerTwo = type;
            break; 
            case 3:
            GameOptions.PlayerThree = type;
            break; 
            case 4:
            GameOptions.PlayerFour = type;
            break;  
        }
    }
    public void CyclePlayerUp(int playerNumber){
        switch(playerNumber){
            case 1: PlayerOneIndex = CycleIndex(true, PlayerOneIndex);
            break;
            case 2: PlayerTwoIndex = CycleIndex(true, PlayerTwoIndex);
            break;
            case 3: PlayerThreeIndex = CycleIndex(true, PlayerThreeIndex);
            break;
            case 4: PlayerFourIndex = CycleIndex(true, PlayerFourIndex);
            break;
        }
    }
    public void CyclePlayerDown(int playerNumber){
        switch(playerNumber){
            case 1: PlayerOneIndex = CycleIndex(false, PlayerOneIndex);
            break;
            case 2: PlayerTwoIndex = CycleIndex(false, PlayerTwoIndex);
            break;
            case 3: PlayerThreeIndex = CycleIndex(false, PlayerThreeIndex);
            break;
            case 4: PlayerFourIndex = CycleIndex(false, PlayerFourIndex);
            break;
        }
    }
    public void FinalizePlayerCount(){
        GameOptions.PlayerOne = FinalizePlayerType(PlayerOneIndex);
        GameOptions.PlayerTwo = FinalizePlayerType(PlayerTwoIndex);
        GameOptions.PlayerThree = FinalizePlayerType(PlayerThreeIndex);
        GameOptions.PlayerFour = FinalizePlayerType(PlayerFourIndex);
    }

    public PlayerType FinalizePlayerType(int playerArrayIndex){
        switch(playerArrayIndex){
            case 0:
            GameOptions.PlayerNumber += 1;
            return PlayerType.Human;
            case 1: 
            GameOptions.PlayerNumber += 1;
            return PlayerType.CPU;
            case 2: return PlayerType.None;
            default: return PlayerType.None;
        }
    }
    public int CycleIndex(bool up, int playerIndex){
        if(up){
            if(playerIndex > 0){
                return playerIndex -= 1;
            }
            else{
                return playerIndex = 2;
            }
        }
        else{
            if(playerIndex < 2){
                return playerIndex += 1;
            }
            else{
                return playerIndex = 0;
            }
        }
    }
    public void SceneChange(int scene){
        SceneManager.LoadScene(scene);
    }

    #region Mission Mode
    private void GenerateMissions(Mission[] missions){
        foreach(Mission mission in missions){
            Button btn = (Button)Instantiate(MissionButton);
            btn.GetComponent<MissionButton>().Mission = mission;
            btn.transform.SetParent(MissionPanel.transform);
        }
    }

    public void ChooseMission(){
        GameOptions.Mission = gameObject.GetComponent<MissionButton>().Mission;
    }
    #endregion
}
