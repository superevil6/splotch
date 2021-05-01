using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public Song[] Songs;
    public Song CurrentSong;
    public AudioSource AudioSource;
    // Start is called before the first frame update
    void Start()
    {
        print("Hey!");
        StartCoroutine(PlayIntro(CurrentSong.Intro));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator PlayIntro(AudioClip intro){
        print("Hi");
        AudioSource.clip = intro;
        AudioSource.Play();
        yield return new WaitForSecondsRealtime(intro.length);
        PlayMainLoop(CurrentSong.MainLoop);
    }
    public void PlayMainLoop(AudioClip loop){
        AudioSource.clip = loop;
        AudioSource.loop = true;
        AudioSource.Play();
    }
}
