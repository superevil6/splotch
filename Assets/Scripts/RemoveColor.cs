using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

public class RemoveColor : MonoBehaviour
{
    public int Uses;
    public float RegenTime;
    public GameObject Panel;
    public Image UseOne;
    public Image UseTwo;
    public Image UseThree;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UseRemoveColor(Ball ball){
        ball.SetNewBallColor(BallColor.white);
        Uses -= 1;
        IndicatorOff();
    }

    public IEnumerator RegenerateRemoveColor(float RegenTime){
        yield return new WaitForSecondsRealtime(RegenTime);
        Uses += 1;
        IndicatorOn();
    }

    public void IndicatorOff(){
        switch(Uses){
            case 2: 
            UseThree.color = Color.gray;
            break;
            case 1:
            UseTwo.color = Color.gray;
            break;
            case 0:
            UseOne.color = Color.gray;
            break;
        }
    }
    public void IndicatorOn(){
        switch(Uses){
            case 1: 
            UseOne.color = Color.white;
            break;
            case 2:
            UseTwo.color = Color.white;
            break;
            case 3:
            UseThree.color = Color.white;
            break;
        }
    }
}
