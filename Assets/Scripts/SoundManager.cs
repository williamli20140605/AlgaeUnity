using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public AudioSource BGM;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        BGM.volume = 0.75f;
    }

    public void Start()
    {
        PlayBGM();
    }

    public void PlayBGM()
    {
        // Start playing the BGM
        BGM.Play();
        BGM.loop = true;
    }
}
