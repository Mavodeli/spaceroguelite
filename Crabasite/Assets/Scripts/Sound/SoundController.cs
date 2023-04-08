using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{

    private Dictionary<string, Sound> sounds;

    private GameObject player;

    private List<NamedAudioSource> activeSounds;

    [SerializeField]
    private float volume = 0.8f; // Range from 0f to 1f

    void Start()
    {

        GameObject[] possibleSoundControllers = GameObject.FindGameObjectsWithTag("SoundController");
        if(possibleSoundControllers.Length > 1){
            Destroy(this.gameObject);
            return;
        }

        Component[] audioSources = GetComponentsInChildren(typeof (AudioSource));
        sounds = new Dictionary<string, Sound>();
        foreach(Component audioSource in audioSources){
            Sound s = new Sound(audioSource.name, ((AudioSource) audioSource).clip);
            sounds[audioSource.name] = s;
        }

        player = GameObject.Find("Player");

        activeSounds = new List<NamedAudioSource>();

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

    public void playSound(SoundParameter parameters){
        playSound(parameters.soundName, parameters.gameObject, parameters.volume, parameters.dontDestroyOnLoad);
    }

    /**
     * plays sound looping only if sound is not playing yet
     */
    public void playSoundLoopingSafe(SoundParameter parameters){
        foreach(NamedAudioSource nas in activeSounds){
            if(nas.name == parameters.soundName)
            {
                return;
            }
        }
        AudioSource source = parameters.gameObject.AddComponent<AudioSource>();
        source.clip = sounds[parameters.soundName].clip;
        source.loop = true;
        source.volume = parameters.volume * this.volume;
        source.Play();
        NamedAudioSource audio = new NamedAudioSource(parameters.soundName, source);
        activeSounds.Add(audio);
    }

    public void playSoundSafe(SoundParameter parameters){
        foreach(NamedAudioSource nas in activeSounds){
            if(nas.name == parameters.soundName){
                return;
            }
        }
        playSound(parameters);
    }

    private void playSound(string name, GameObject gameObject, float volume, bool dontDestroyOnLoad){
        if (!gameObject || gameObject == null) { return; }
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = sounds[name].clip;
        source.volume = volume * this.volume;
        source.Play();
        activeSounds.Add(new NamedAudioSource(name, source));
        if(dontDestroyOnLoad){
            DontDestroyOnLoad(source);
        }
    }

    /**
     * stops every sound with the soundName
     * should only be used to stop looping sounds, which should already only play once
     */
    public void stopSound(string soundName){
        foreach(NamedAudioSource activeSound in activeSounds){
            if(activeSound.source != null && activeSound.name == soundName){
                activeSound.source.Stop();
            }
        }
    }

    public void setVolume(float volume){
        if(volume < 0f){
            this.volume = 0f;
        } else if(volume > 1f){
            this.volume = 1f;
        } else {
            this.volume = volume;
        }
    }

    private int frameCounter = 0;

    void Update(){

        frameCounter++;
        if(frameCounter == 60){
            for(int i = activeSounds.Count - 1; i >= 0; i--)
            {
                AudioSource activeSound = activeSounds[i].source;
                if(activeSound != null && !activeSound.isPlaying){
                    activeSounds.RemoveAt(i);
                    Destroy(activeSound);
                }
            }
            frameCounter = 0;
        }
    }
}
