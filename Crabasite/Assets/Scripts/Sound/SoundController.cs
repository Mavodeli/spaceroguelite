using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{

    private Dictionary<string, Sound> sounds;

    private GameObject player;

    private List<AudioSource> activeSounds;

    // Start is called before the first frame update
    void Start()
    {
        Component[] audioSources = GetComponentsInChildren(typeof (AudioSource));
        sounds = new Dictionary<string, Sound>();
        foreach(Component audioSource in audioSources){
            Sound s = new Sound(audioSource.name, ((AudioSource) audioSource).clip);
            sounds[audioSource.name] = s;
        }

        player = GameObject.Find("Player");

        activeSounds = new List<AudioSource>();

    }

    // Plays sound once on gameObject
    void playSound(string name, GameObject gameObject){
        playSound(name, gameObject, false);
    }

    // Plays sound in looping on gameObject
    void playSoundLooping(string name, GameObject gameObject){
        playSound(name, gameObject, true);
    }

    /**
     * name: the name of the sound that should be played
     * gameObject: the gameObject which should "play" the sound, meaning on it's position it will be played
     * looping: if true, the sound will loop and also will be destroyed upon loading a new scene
     * destroyOnLoad: if false, the sound will keep playing upon loading new scene
     */
    private void playSound(string name, GameObject gameObject, bool looping){
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = sounds[name].clip;
        source.loop = looping;
        source.Play();
        if(!looping){
            activeSounds.Add(source);
        } // looping sounds will automatically be destroyed upon loading a new scene

    }

    private int frameCounter = 0;

    void Update(){
        frameCounter++;
        if(frameCounter == 60){
            for(int i = activeSounds.Count - 1; i >= 0; i--)
            {
                AudioSource activeSound = activeSounds[i];
                if(!activeSound.isPlaying){
                    activeSounds.RemoveAt(i);
                    Destroy(activeSound);
                }
            }
            frameCounter = 0;
        }
    }

}
