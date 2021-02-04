using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        
        public AudioSource source; 
        public float volume;
    }

    public Sound clickSound; 
    public Sound heliSound;
    
    
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
        clickSound.source.volume = .2f;

        // foreach(KeyValuePair<string, Sound> s in Sounds){
        //     s.Value.source = s.Value.obj.AddComponent<AudioSource>();
        //     s.Value.source.clip = s.Value.clip;
        //     s.Value.source.volume = s.Value.volume;
        // }
    }

    

    public void ClickSound(){
        clickSound.source.Play();
    }

    public void DisableSound ()
    {
        clickSound.source.volume = 0f;
        heliSound.source.volume = 0f;
    }

    public void EnableSound ()
    {
        clickSound.source.volume = clickSound.volume;
        heliSound.source.volume = heliSound.volume;
    }
}
