using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{

    public static SoundManager _instance;
    List<AudioSource> emitters = new List<AudioSource>();

    public enum SoundList
    {
        WALK,
        WOLF
    }

    public enum MusicList
    {
        NONE,
        MENU,
        OVERWORLD,
        HOUSE
    }

    public struct LoopedSound
    {
        public AudioSource audioSource;
        public float timeUntilStop;
    }
    List<LoopedSound> loopedSoundList = new List<LoopedSound>();

    MusicList currentMusicPlaying = MusicList.NONE;

    List<AudioClip> listWalkSounds = new List<AudioClip>();
    List<AudioClip> listDemonNoiseSounds = new List<AudioClip>();

    [Header("VolumeSounds")]
    [SerializeField] AudioMixer audioMixer;

    [Header("Sounds")]
    [SerializeField] AudioClip wolfClip;

    [Header("WalkClips")]
    [SerializeField] AudioClip walkClip1;
    [SerializeField] AudioClip walkClip2;
    [SerializeField] AudioClip walkClip3;

    [Header("Musics")]
    [SerializeField] AudioClip menuMusicClip;
    [SerializeField] AudioClip overworldMusicClip;
    [SerializeField] AudioClip houseMusicClip;

    [Header("Emmiters")]
    [SerializeField] GameObject emitterPrefab;
    [SerializeField] int emitterNumber;
    [SerializeField] AudioSource musicEmitter;

    private void Awake()
    {
        if(_instance)
            Destroy(gameObject);
        else
            _instance = this;
    }

    // Use this for initialization
    void Start ()
    {
        DontDestroyOnLoad(gameObject);

        for(int i = 0; i <= emitterNumber;i++)
        {
            GameObject audioObject = Instantiate(emitterPrefab, emitterPrefab.transform.position, emitterPrefab.transform.rotation);
            emitters.Add(audioObject.GetComponent<AudioSource>());
            DontDestroyOnLoad(audioObject);
        }

        listWalkSounds = new List<AudioClip>{walkClip1, walkClip2, walkClip3};
    }

    private void Update()
    {
        foreach(LoopedSound loopedSound in loopedSoundList)
        {
            if(Utility.IsOver(loopedSound.timeUntilStop))
            {
                loopedSound.audioSource.Stop();
                loopedSoundList.Remove(loopedSound);
                break;
            }
        }
    }

    public AudioSource PlaySound(SoundList sound, float timeToLoop = 0.0f)
    {
        //return null;
        AudioSource emitterAvailable = null;

        foreach(AudioSource emitter in emitters)
        {
            if(!emitter.isPlaying)
            {
                emitterAvailable = emitter;
            }
        }

        if (emitterAvailable != null)
        {
            emitterAvailable.loop = false;
            Debug.Log(sound.ToString());
            switch (sound)
            {
                case SoundList.WALK:
                    int indexWalk = Random.Range(0, listWalkSounds.Count);
                    emitterAvailable.clip = listWalkSounds[indexWalk];
                    emitterAvailable.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Player")[0];
                    break;
                case SoundList.WOLF:
                    emitterAvailable.clip = wolfClip;
                    emitterAvailable.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Environment")[0];
                    break;
            }

            if(timeToLoop > 0.0f)
            {
                emitterAvailable.loop = true;
                LoopedSound newLoopSound = new LoopedSound
                {
                    audioSource = emitterAvailable,
                    timeUntilStop = Utility.StartTimer(timeToLoop)
                };
                loopedSoundList.Add(newLoopSound);  
            }
            
            emitterAvailable.Play();
            return emitterAvailable;
        }
        else
        {
            Debug.Log("no emitter available");
            return null;
        }        
    }

    public void PlayMusic(MusicList music)
    {
        if (currentMusicPlaying != music)
        {
            musicEmitter.loop = true;

            switch (music)
            {
                case MusicList.NONE:
                    musicEmitter.Stop();
                    break;
                case MusicList.MENU:
                    musicEmitter.clip = menuMusicClip;
                    musicEmitter.Play();
                    break;
            }
            currentMusicPlaying = music;
        }
    }

    public void StopSound(AudioSource source)
    {
        source.Stop();
        foreach(LoopedSound looped in loopedSoundList)
        {
            if(looped.audioSource == source)
            {
                loopedSoundList.Remove(looped);
                break;
            }
        }
    }
}
