using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {
public static MusicPlayer musicPlayer;

    // Using the singleton pattern to prevent the 
    // music player from destroying itself between 
    // the scenes.
    private void Awake() {
        if(musicPlayer != null){
            Destroy(gameObject);
        }

        else{
            musicPlayer = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
