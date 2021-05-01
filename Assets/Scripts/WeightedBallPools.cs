using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
using UnityEngine;

[CreateAssetMenu(fileName = "weightedBallPool", menuName = "ScriptableObjects/WeightedBallPools", order = 1)]
public class WeightedBallPools : ScriptableObject
{
    //An array would be sufficient, but this way no one needs to remember which color is which index. That'd be annoying and dumb.
    public int WhiteChance;
    public int RedChance;
    public int BlueChance;
    public int YellowChance;
    public int GreenChance;
    public int OrangeChance;
    public int PurpleChance;
    public int BrownChance;
    public int BlackChance;

    public BallColor RandomBallColor(){
        //This will use randomization to make a range of all color chances then use the max to pick the highest.
        List<int> values = new List<int>();
        values.Add(Random.Range(0, WhiteChance));
        values.Add(Random.Range(0, RedChance));
        values.Add(Random.Range(0, BlueChance));
        values.Add(Random.Range(0, YellowChance));
        values.Add(Random.Range(0, GreenChance));
        values.Add(Random.Range(0, OrangeChance));
        values.Add(Random.Range(0, PurpleChance));
        values.Add(Random.Range(0, BrownChance));
        values.Add(Random.Range(0, BlackChance));
        int highestValue = values.Max();
        int colorIndex = 0;
        for(int i = 0; i < values.Count; i++){
            if(values[i] == highestValue){
                colorIndex = i;
            }
        }
        switch(colorIndex){
            case 0: return BallColor.white;
            case 1: return BallColor.red;
            case 2: return BallColor.blue;
            case 3: return BallColor.yellow;
            case 4: return BallColor.green;
            case 5: return BallColor.orange;
            case 6: return BallColor.purple;
            case 7: return BallColor.brown;
            case 8: return BallColor.black;
        }
        return BallColor.white;
    }
}
