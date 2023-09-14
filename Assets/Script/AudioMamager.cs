using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMamager : MonoBehaviour
{
    public static AudioMamager instance;

    [Header("#BGM")]
    public AudioClip bgmClip;
    public float bgmVol;
    AudioSource bmgPlayer;
    AudioHighPassFilter bgmEffect;

    [Header("#SFX")]
    public AudioClip[] sfxClip;
    public float sfxVol;
    public int channel;
    AudioSource[] sfxPlayer;
    int channelIndex;

    public enum Sfx { Dead,Hit,LevelUp=3,Lose,Melee,Range = 7,Select,Win}
    private void Awake()
    {
        instance = this;
        Init();
    }

    void Init()
    {
        GameObject bgmObject = new GameObject("BgmPlater");
        bgmObject.transform.parent = this.transform;
        bmgPlayer = bgmObject.AddComponent<AudioSource>();
        bmgPlayer.playOnAwake = false;
        bmgPlayer.loop = true;
        bmgPlayer.volume = bgmVol;
        bmgPlayer.clip = bgmClip;
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

        GameObject sfxObject = new GameObject("SfxPlater");
        sfxObject.transform.parent = this.transform;
        sfxPlayer = new AudioSource[channel];

        for(int i = 0; i < sfxPlayer.Length; i++) 
        {
            sfxPlayer[i] = sfxObject.AddComponent<AudioSource>();
            sfxPlayer[i].playOnAwake = false;
            sfxPlayer[i].bypassListenerEffects = true;
            sfxPlayer[i].volume = sfxVol;
        }
    }

    public void PlayBgm(bool isPlay)
    {
        if(isPlay)
        {
            bmgPlayer.Play();
        }
        else
        {
            bmgPlayer.Stop();
        }
    }

    public void EffectyBgm(bool isPlay)
    {
        bgmEffect.enabled = isPlay;
    }

    public void PlaySFX(Sfx sfx)
    {
        for(int i = 0;i < sfxPlayer.Length;i++) 
        {
            int loopindex = (i + channelIndex) % sfxPlayer.Length;

            if (sfxPlayer[loopindex].isPlaying)
                continue;

            int ran = 0;
            if(sfx == Sfx.Hit || sfx == Sfx.Melee)
            {
                ran = Random.Range(0, 2);
            }

            channelIndex = loopindex;
            sfxPlayer[loopindex].clip = sfxClip[(int)sfx + ran];
            sfxPlayer[loopindex].Play();
            break;
        }
    }
}
