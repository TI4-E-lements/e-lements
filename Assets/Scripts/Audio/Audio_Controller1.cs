﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// It is used to manage the sounds and play the scene song
/// Also it plays sound when finish
/// </summary>
public class Audio_Controller1 : MonoBehaviour
{
    //public AudioClip SceneMusic;
    public AudioClip[] Physic_attack;                       //Sword attack
    public AudioClip[] Knights_Dying;                       //Knights dying
    public AudioClip[] Enemy_Dying;                         //Enemy dying
    public AudioClip[] KnightsDoor;                         //Knight tower door
    public AudioClip[] Arrows;
    public AudioClip start;
    public AudioClip exit;
    public AudioClip enemyOnLimit;
    public AudioClip[] magicExplosion;
    public AudioClip trap;
    public AudioClip PlaceTrap;
    public AudioClip Runes;
    public float effects_volume;
    private GameObject UI_Exit;
    private AudioSource audio;
    // Use this for initialization
    void Start()
    {
        audio = GetComponent<AudioSource>();
        if (!PlayerPrefs.HasKey("audio-volume"))
        {
            PlayerPrefs.SetFloat("audio-volume", 5);
            Load();
        }
        else
        {
            Load();
        }
        UI_Exit = GameObject.Find("UI_Exit");
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("UI_Exit"))
        {
            if (UI_Exit.GetComponent<Canvas>().enabled == true && audio.clip != exit)
            {
                audio.clip = exit;
                audio.loop = false;
                audio.Stop();
                Invoke("DelayExitAudio", 0.5f);
            }
        }
    }

    private void Load()
    {
        audio.volume = PlayerPrefs.GetFloat("audio-volume");
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("audio-volume", audio.volume);
    }

    public void SoundControl()
    {
        if (PlayerPrefs.HasKey("sound"))
        {
            switch (PlayerPrefs.GetInt("sound"))
            {
                case 0: AudioListener.volume = 1.0f; PlayerPrefs.SetInt("sound", 1); break;
                case 1: AudioListener.volume = 0.0f; PlayerPrefs.SetInt("sound", 0); break;
            }
        }
        else
        {
            PlayerPrefs.SetInt("sound", 0);
            AudioListener.volume = 0.0f;
        }
    }

    void DelayExitAudio()
    {
        audio.Play();
    }
}
