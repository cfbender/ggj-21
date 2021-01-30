using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{

    //MUSIC SPECIFIC
    private AudioSource musicAudioSource;
    private float defaultMusicVolume = 0.5f;

    private bool musicEnabled;
    private const string musicEnabledKey = "MusicEnabled";
    public event Action<bool> musicEnableDisable;

    public AudioClip music_pre;
    public AudioClip music_start;
    public AudioClip music_during;
    public AudioClip music_tallying;
    public AudioClip music_failure;
    public AudioClip music_success;
    public AudioClip music_credits;

    public enum MusicType
    {
        Pre,
        Start,
        During,
        Tally,
        Failure,
        Success,
        Credits
    }
    private Dictionary<MusicType, AudioClip> musicAudioClips;

    //SFX SPECIFIC

    private float defaultSFXVolume = 0.5f;

    private bool sfxEnabled;
    private const string sfxEnabledKey = "SFXEnabled";
    public event Action<bool> sfxEnableDisable;

    public AudioClip sfx_Profile_In;
    public AudioClip sfx_Profile_Out;
    public AudioClip sfx_Heart_Up;
    public AudioClip sfx_Heart_Down;
    public AudioClip sfx_InterestReveal;
    public AudioClip sfx_ChatBubble_popup;
    public AudioClip sfx_ChatBubble_convo;
    public AudioClip sfx_FailedMatch;
    public AudioClip sfx_SuccessfulMatch;
    public AudioClip sfx_TickTock;

    public enum SFXType
    {
        Profile_In,
        Profile_Out,

        Heart_Up,
        Heart_Down,

        InterestReveal,

        ChatBubble_popup,
        ChatBubble_convo,

        FailedMatch,
        SuccessfulMatch,

        TickTock
    }
    private Dictionary<SFXType, AudioClip> sfxAudioClips;


    private AudioSource NewAudioSource(string name)
    {
        GameObject audioObject = new GameObject(name);
        audioObject.transform.SetParent(this.transform);

        AudioSource audioSource = audioObject.AddComponent<AudioSource>();

        return audioSource;
    }


    private void InitSounds()
    {
        //Initiate music

        musicAudioClips = new Dictionary<MusicType, AudioClip>();
        musicAudioClips.Add(MusicType.Pre, music_pre);
        musicAudioClips.Add(MusicType.Start, music_start);
        musicAudioClips.Add(MusicType.During, music_during);
        musicAudioClips.Add(MusicType.Tally, music_tallying);
        musicAudioClips.Add(MusicType.Failure, music_failure);
        musicAudioClips.Add(MusicType.Success, music_success);
        musicAudioClips.Add(MusicType.Credits, music_credits);

        //Initiate sfx

        sfxAudioClips = new Dictionary<SFXType, AudioClip>();
        sfxAudioClips.Add(SFXType.Profile_In, sfx_Profile_In);
        sfxAudioClips.Add(SFXType.Profile_Out, sfx_Profile_Out);
        sfxAudioClips.Add(SFXType.Heart_Up, sfx_Heart_Up);
        sfxAudioClips.Add(SFXType.Heart_Down, sfx_Heart_Down);
        sfxAudioClips.Add(SFXType.InterestReveal, sfx_InterestReveal);
        sfxAudioClips.Add(SFXType.ChatBubble_popup, sfx_ChatBubble_popup);
        sfxAudioClips.Add(SFXType.ChatBubble_convo, sfx_ChatBubble_convo);
        sfxAudioClips.Add(SFXType.FailedMatch, sfx_FailedMatch);
        sfxAudioClips.Add(SFXType.SuccessfulMatch, sfx_SuccessfulMatch);
        sfxAudioClips.Add(SFXType.TickTock, sfx_TickTock);
    }

    private void InitAudioSources()
    {
        musicAudioSource = NewAudioSource("Music");

        musicAudioSource.loop = true;
        musicAudioSource.volume = defaultMusicVolume;
        musicAudioSource.playOnAwake = false;
    }

    private bool MusicEnabled
    {
        get { return PlayerPrefs.GetInt(musicEnabledKey, 1) == 1 ? true : false; }
        set
        {
            musicEnabled = value;
            PlayerPrefs.SetInt(musicEnabledKey, value ? 1 : 0);
        }
    }

    public void EnableDisabledMusic()
    {
        MusicEnabled = !musicEnabled;

        if (musicEnableDisable != null)
        {
            musicEnableDisable(musicEnabled);
        }
    }

    public void PlayMusic(MusicType musicType)
    {
        if (musicEnabled)
        {
            musicAudioSource.clip = musicAudioClips[musicType];
            musicAudioSource.Play();
        }
    }

    private bool SFXEnabled
    {
        get { return PlayerPrefs.GetInt(sfxEnabledKey, 1) == 1 ? true : false; }
        set
        {
            sfxEnabled = value;
            PlayerPrefs.SetInt(sfxEnabledKey, value ? 1 : 0);
        }
    }

    public void EnableDisabledSFX()
    {
        SFXEnabled = !sfxEnabled;

        if (sfxEnableDisable != null)
        {
            sfxEnableDisable(sfxEnabled);
        }
    }

    private float getSFXVolumeHelper(SFXType sfxType)
    {
        float volume = 1.0f;
        switch (sfxType)
        {

            default: break;
        }

        return volume;
    }

    private void AdjustSFXPitchHelper(AudioSource audioSource, SFXType sfxType)
    {
        float pitch = UnityEngine.Random.Range(0.75f, 1.0f);

        switch (sfxType)
        {

            default: break;
        }

        audioSource.pitch = pitch;
    }

    void AddEffect(string effect, GameObject parentObj)
    {
        string[] effectSplit = effect.Split('-');
        string effectName = effectSplit[0];

        switch (effectName)
        {
            case "echo":
            case "e":
                AudioEchoFilter echo = parentObj.AddComponent<AudioEchoFilter>();

                float delay = effectSplit[1] == null ? 4.2f : float.Parse(effectSplit[1]);
                float wet = effectSplit[2] == null ? 1 : float.Parse(effectSplit[2]);

                echo.delay = delay;
                echo.wetMix = wet;

                break;

            case "reverb":
            case "r":

                AudioReverbFilter reverb = parentObj.AddComponent<AudioReverbFilter>();

                string ReverbType = effectSplit[1] == null ? "ConcertHall" : effectSplit[1];
                switch (ReverbType)
                {
                    case "Cave":
                        reverb.reverbPreset = AudioReverbPreset.Cave;
                        break;
                    case "ConcertHall":
                        reverb.reverbPreset = AudioReverbPreset.Concerthall;
                        break;
                    case "Underwater":
                        reverb.reverbPreset = AudioReverbPreset.Underwater;
                        break;
                    default: break;
                }

                break;
            default:
                break;
        }
    }

    private void AdjustSFXEffects(string effects, GameObject sfxGameObject)
    {
        string[] effectsSplit = effects.Split('_');

        for (int i = 0; i < effectsSplit.Length; i++)
        {
            string effect = effectsSplit[i];
            AddEffect(effect, sfxGameObject);
        }
    }

    public void PlaySFX(SFXType sfxType, string effects = "")
    {
        AudioClip sfxAudioClip = sfxAudioClips[sfxType];

        if (!sfxEnabled || sfxAudioClip == null) return;

        AudioSource sfxAudioSource = NewAudioSource("SFX");
        GameObject sfxGameObject = sfxAudioSource.gameObject;

        AdjustSFXPitchHelper(sfxAudioSource, sfxType);
        float volume = getSFXVolumeHelper(sfxType);
        if (effects != "") AdjustSFXEffects(effects, sfxGameObject);

        sfxAudioSource.PlayOneShot(sfxAudioClip, volume);
        float clipLength = sfxAudioClip.length;
        Destroy(sfxAudioSource, clipLength);
    }


    private void Start()
    {
        musicEnabled = MusicEnabled;
        sfxEnabled = SFXEnabled;

        InitAudioSources();
        InitSounds();

        // PlayMusic(MusicType.Pre);
    }
}
