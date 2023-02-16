using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{

    private Dictionary<string, Sound> sounds;

    private GameObject player;

    private List<AudioSource> activeSounds;

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

        DontDestroyOnLoad(this);

    }

    public void loadSound(string name, AudioClip clip){
        if(sounds.ContainsKey(name)){
            return;
        }
        Sound s = new Sound(name, clip);
        sounds[name] = s;
    }

    /**
     * Loads a new sound and replaces it with an old sound if one with the same name already exists
     */
    public void loadSoundAndReplace(string name, AudioClip clip){
        Sound s = new Sound(name, clip);
        sounds[name] = s;
    }

    public void reloadSounds(){
        if(sounds == null){
            return;
        }
        Component[] audioSources = GetComponentsInChildren(typeof (AudioSource));
        sounds.Clear();
        foreach(Component audioSource in audioSources){
            Sound s = new Sound(audioSource.name, ((AudioSource) audioSource).clip);
            sounds[audioSource.name] = s;
        }
    }

    public AudioSource playSound(SoundParameter parameters){
        return playSound(parameters.soundName, parameters.gameObject, false, parameters.dontDestroyOnLoad);
    }

    public AudioSource playSoundLooping(SoundParameter parameters){
        return playSound(parameters.soundName, parameters.gameObject, true, parameters.dontDestroyOnLoad);
    }

    private AudioSource playSound(string name, GameObject gameObject, bool looping, bool dontDestroyOnLoad){
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = sounds[name].clip;
        source.loop = looping;
        source.Play();
        if(!looping){
            activeSounds.Add(source);
        } // looping sounds will automatically be destroyed upon loading a new scene, or when they are stopped
        if(dontDestroyOnLoad){
            DontDestroyOnLoad(source);
        }
        return source;
    }

    /**
     * Stops any sound
     * Can be useful if there is a looping sound which should not be destroyed on load
     */
    public void stopSound(AudioSource sound){
        sound.Stop();
        Destroy(sound);
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
