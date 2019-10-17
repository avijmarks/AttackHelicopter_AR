using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public struct Sound{
        public string name;
        public AudioClip clip;
        [HideInInspector]
        public AudioSource source; 
    }

    public Sound clickSound; 
    
    public static AudioManager instance;
    void Awake()
    {
        if (instance == null){
            instance = this;
        } else {
            Destroy(gameObject);
        }
        
        //doing this manually rather than iterating through a class(instead of struct) because shouldn't be adding any more audio. 
        clickSound.source = gameObject.AddComponent<AudioSource>();
        clickSound.source.clip = clickSound.clip;
    }

    

    public void ClickSound(){
        clickSound.source.Play();
    }
}
