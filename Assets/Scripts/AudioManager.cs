using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager instance;
    public AudioSource[] Audios1;
    public AudioSource[] Audios2;
    public AudioSource[] Audios3;
    public AudioSource fotoAudio;
    public AudioSource[] efectos;//0: sorpresa, 1: amartillar, 2: disparo
    public AudioSource[] narraciones; //0 comienzo, 1 final

    private void Awake()
    {
        instance = this;
    }
}
