using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int RedCleared;
    public int BlueCleared;
    public int YellowCleared;
    public int GreenCleared;
    public int OrangeCleared;
    public int PurpleCleared;
    public int BrownCleared;
    public int TwoRensa;
    public int ThreeRensa;
    public int FourRensa;
    public int FiveRensa;
    public int SixRensa;
    public int SevenRensa;
    public int EightRensa;
    public int NineRensa;
    public int TenRensa;
    // Start is called before the first frame update
    void Start()
    {
        ResetStats();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ResetStats(){
        RedCleared = 0;
        BlueCleared = 0;
        YellowCleared = 0;
        GreenCleared = 0;
        OrangeCleared = 0;
        PurpleCleared = 0;
        BrownCleared = 0;
        TwoRensa = 0;
        ThreeRensa = 0;
        FourRensa = 0;
        FiveRensa = 0;
        SixRensa = 0;
        SevenRensa = 0;
        EightRensa = 0;
        NineRensa = 0;
        TenRensa = 0;
    }
}
