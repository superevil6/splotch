using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;
using UnityEngine.SceneManagement;

public class MissionButton : MonoBehaviour
{
    public Mission Mission;
    public Text MissionName;
    public Text MissionDescription;
    public Text ClearStatus;
    // Start is called before the first frame update
    void Start()
    {
        MissionName.text = Mission.MissionName;
        MissionDescription.text = Mission.MissionDescription;
    }
    public void SetMission(){
        GameOptions.GameMode = GameMode.Mission;
        GameOptions.Mission = Mission;
        GameOptions.PlayerNumber = 1;
        SceneChange(1);
    }
    public void SceneChange(int scene){
        SceneManager.LoadScene(scene);
    }
}
