using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

public class MissionMode : MonoBehaviour
{
    public Mission Mission;
    private MissionType MissionType;
    public GameManager GameManager;
    public GameBoard GameBoard;
    public PlayerManager PlayerManager;
    public PlayerStats PlayerStats;
    public GameObject MissionDetails;
    public Text MissionDetailsText;
    public Text TimeRemaining;
    public Text ProgressText;
    public GameObject ResultsPanel;
    public Text ResultText;
    private BallColor GoalColor;
    private int RensaGoal;
    private int ScoreGoal;
    public int GoalAmount;
    public bool MissionModeTimeUp = false;
    public bool Victory;
    private float TimeLimit;

    // Start is called before the first frame update
    void Start()
    {
        GameBoard = GetComponentInParent<GameBoard>();
        PlayerManager = GetComponentInParent<PlayerManager>();
        PlayerStats = GetComponentInParent<PlayerStats>();
        Mission = GameOptions.Mission;
        MissionType = Mission.MissionType;
        MissionDetails.SetActive(true);
        switch(MissionType){
            case MissionType.AvoidColor:
                GoalAmount = Mission.ColorGoalAmount;
                GoalColor = Mission.ColorToAvoid;
                MissionDetailsText.text = "Avoid " + GoalAmount + " " + GoalColor.ToString() + " balls.";
            break;
            case MissionType.ClearColor:
                GoalColor = Mission.ColorToClear;
                GoalAmount = Mission.ColorGoalAmount;
                MissionDetailsText.text = "Clear " + GoalAmount + " " + GoalColor.ToString() + " balls.";
            break;
            case MissionType.Rensa:
                RensaGoal = Mission.RensaMultiplierGoal;
                GoalAmount = Mission.RensaGoalAmount;
                MissionDetailsText.text = "Get a combo of " + Mission.RensaMultiplierGoal.ToString() + " " + Mission.RensaGoalAmount.ToString() + " times.";   
            break;
            case MissionType.ScoreAttack:
                GoalAmount = Mission.ScoreToReach;
                MissionDetailsText.text = "Score at least " + Mission.ScoreToReach.ToString() + " points.";
            break;
            case MissionType.TimeAttack:

            break;
        }
        TimeLimit = Mission.TimeLimit;
    }

    // Update is called once per frame
    void Update()
    {
        if(TimeLimit > 0){
            ProgressText.text = FindProgressText(MissionType);
            TimeRemaining.text = TimeLimit.ToString();
            TimeLimit -= Time.deltaTime;
        }
        if(FinalResults() && !ResultsPanel.activeInHierarchy){
            Victory = true;
            PlayerManager.GameOver = true;
            if(Victory){
                ResultText.text = "Success!";
            } else{
                ResultText.text = "Failure.";
            }
            ResultsPanel.SetActive(true);
        }
        if(TimeLimit <= 0 && !MissionModeTimeUp){
            Victory = FinalResults();
            MissionModeTimeUp = true;

        }
        if(MissionModeTimeUp && !ResultsPanel.activeInHierarchy){
            ResultsPanel.SetActive(true);
            if(Victory){
                ResultText.text = "Success!";
            } else{
                ResultText.text = "Failure.";
            }    
        }
    }

    public bool FinalResults(){
        int GoalValue;
        switch(Mission.MissionType){
            case MissionType.AvoidColor:
                GoalValue = FindGoalColor(Mission.ColorToAvoid);
                if(GoalValue < Mission.ColorGoalAmount){
                    return true;
                }
            break;
            case MissionType.ClearColor:
                GoalValue = FindGoalColor(Mission.ColorToClear);
                if(GoalValue >= Mission.ColorGoalAmount){
                    return true;
                }
            break;
            case MissionType.Rensa:
                int rensaGoal = FindGoalRensa(Mission.RensaMultiplierGoal);
                if(rensaGoal < Mission.RensaGoalAmount){
                    return true;
                }
            break;
            case MissionType.ScoreAttack:
                if(Mission.ScoreToReach <= PlayerManager.Score){
                    return true;
                }
            break;
            case MissionType.TimeAttack:

            break;        
        }
        return false;
    }
    public int FindGoalColor(BallColor GoalColor){
        switch(GoalColor){
            case BallColor.red: return PlayerStats.RedCleared;
            case BallColor.blue: return PlayerStats.BlueCleared;
            case BallColor.yellow: return PlayerStats.YellowCleared;
            case BallColor.green: return PlayerStats.GreenCleared;
            case BallColor.orange: return PlayerStats.OrangeCleared;
            case BallColor.purple: return PlayerStats.PurpleCleared;
            case BallColor.brown: return PlayerStats.BrownCleared;
        }
        return 0;
    }
    public int FindGoalRensa(int rensa){
        switch(rensa){
            case 2: return PlayerStats.TwoRensa;
            case 3: return PlayerStats.ThreeRensa;
            case 4: return PlayerStats.FourRensa;
            case 5: return PlayerStats.FiveRensa;
            case 6: return PlayerStats.SixRensa;
            case 7: return PlayerStats.SevenRensa;
            case 8: return PlayerStats.EightRensa;
            case 9: return PlayerStats.NineRensa;
            case 10: return PlayerStats.TenRensa;
        }
        return 0;
    }
    public string FindProgressText(MissionType type){
        switch(type){
            case MissionType.AvoidColor: return (GoalAmount - FindGoalColor(GoalColor)).ToString() + " balls remaining.";
            case MissionType.ClearColor: return (GoalAmount - FindGoalColor(GoalColor)).ToString() + " balls remaining.";
            case MissionType.Rensa: return (GoalAmount - FindGoalRensa(RensaGoal)) + " combos remaining.";
            case MissionType.ScoreAttack: return (GoalAmount - PlayerManager.Score) + " points remaining";
        }
        return "I dunno lol";
    }
}
