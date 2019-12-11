using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

[CreateAssetMenu(fileName = "Mission", menuName = "ScriptableObjects/Missions", order = 3)]
public class Mission : ScriptableObject
{
    public MissionType MissionType;
    public float TimeLimit;
    public int ScoreToReach;
    public BallColor ColorToClear;
    public BallColor ColorToAvoid;
    public int ColorGoalAmount;
    public int RensaMultiplierGoal;
    public int RensaGoalAmount;
    public string MissionName;
    public string MissionDescription;
    public int RewardAmount;

}
