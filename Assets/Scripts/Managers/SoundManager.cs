using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    static SoundManager _instance;
    public static SoundManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(gameObject);
    }
   
    List<AudioSource> emitters = new List<AudioSource>();
    public enum SoundList
    {
        PAS,
        BUISSON,
        LOUP_CRI,
        LOUP_GROGNEMENT,
        CLIC,
        DIALOGUE,
        FEUILLAGE,
        GRAB
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

    List<AudioClip> listPasSounds = new List<AudioClip>();
    List<AudioClip> listBuissonSounds = new List<AudioClip>();
    List<AudioClip> listFeuillageSounds = new List<AudioClip>();

    [Header("VolumeSounds")]
    [SerializeField] AudioMixer audioMixer;

    [Header("Sounds")]
    [SerializeField] AudioClip loupCriClip;
    [SerializeField] AudioClip loupGrognementClip;
    [SerializeField] AudioClip clicClip;
    [SerializeField] AudioClip dialogueClip;
    [SerializeField] AudioClip grabClip;

    [Header("pasClips")]
    [SerializeField] AudioClip pasClip1;
    [SerializeField] AudioClip pasClip2;
    [SerializeField] AudioClip pasClip3;
    [SerializeField] AudioClip pasClip4;

    [Header("Buisson")]
    [SerializeField] AudioClip buissonClip1;
    [SerializeField] AudioClip buissonClip2;

    [Header("Feuillage")]
    [SerializeField] AudioClip feuillageClip1;
    [SerializeField] AudioClip feuillageClip2;

    [Header("Musics")]
    [SerializeField] AudioClip menuMusicClip;
    [SerializeField] AudioClip overworldMusicClip;
    [SerializeField] AudioClip houseMusicClip;

    [Header("Emmiters")]
    [SerializeField] GameObject emitterPrefab;
    [SerializeField] int emitterNumber;
    [SerializeField] AudioSource musicEmitter;

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

        listPasSounds = new List<AudioClip>{ pasClip1, pasClip2, pasClip3, pasClip4};
        listBuissonSounds = new List<AudioClip>{ buissonClip1, buissonClip2};
        listFeuillageSounds= new List<AudioClip>{ feuillageClip1, feuillageClip2};
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
                case SoundList.PAS:
                    int indexWalk = Random.Range(0, listPasSounds.Count);
                    emitterAvailable.clip = listPasSounds[indexWalk];
                    emitterAvailable.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Marche")[0];
                    break;
                case SoundList.BUISSON:
                    int indexBuisson = Random.Range(0, listBuissonSounds.Count);
                    emitterAvailable.clip = listBuissonSounds[indexBuisson];
                    emitterAvailable.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Buisson")[0];
                    break;
                case SoundList.LOUP_CRI:
                    emitterAvailable.clip = loupCriClip;
                    emitterAvailable.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Cri")[0];
                    break;
                case SoundList.LOUP_GROGNEMENT:
                    emitterAvailable.clip = loupGrognementClip;
                    emitterAvailable.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Grognement")[0];
                    break;
                case SoundList.CLIC:
                    emitterAvailable.clip = clicClip;
                    emitterAvailable.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Clic")[0];
                    break;
                case SoundList.DIALOGUE:
                    emitterAvailable.clip = dialogueClip;
                    emitterAvailable.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Dialogue")[0];
                    break;
                case SoundList.FEUILLAGE:
                    int indexFeuillage = Random.Range(0, listFeuillageSounds.Count);
                    emitterAvailable.clip = listFeuillageSounds[indexFeuillage];
                    emitterAvailable.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Feuillage")[0];
                    break;
                case SoundList.GRAB:
                    emitterAvailable.clip = grabClip;
                    emitterAvailable.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Ramasser")[0];
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
                case MusicList.HOUSE:
                    musicEmitter.clip = houseMusicClip;
                    musicEmitter.Play();
                    break;
                case MusicList.OVERWORLD:
                    musicEmitter.clip = overworldMusicClip;
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
