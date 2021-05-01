using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Song", menuName = "ScriptableObjects/Songs", order = 4)]
public class Song : ScriptableObject
{
    public AudioClip Intro;
    public AudioClip MainLoop;
    // Start is called before the first frame update
}
