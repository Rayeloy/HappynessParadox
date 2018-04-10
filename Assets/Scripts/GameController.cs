using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public float cycleTime;
    public SpriteRenderer Veil;
    public Image transicion;
    public Text numDia;
    public GameObject clickParaContinuar;

    public SpriteRenderer[] objetosNormal;
    public SpriteRenderer[] objetosEnUso;
    public SpriteRenderer[] luzObjetos;
    public SpriteRenderer player;
    public GameObject foto;
    public GameObject cajon;
    public GameObject menuPistola;
    public Image[] spritesCSFinal;

    public AudioSource music;

    [HideInInspector]
    public bool playing = false;

    private void Awake()
    {
        instance = this;
        //DontDestroyOnLoad(this);
        Physics.queriesHitTriggers = true;

        csFinal = CutsceneFinal.none;
        Veil.enabled = false;
        foto.SetActive(false);
        clickParaContinuar.SetActive(false);
        for (int i = 0; i < objetosEnUso.Length; i++)
        {
            objetosEnUso[i].enabled = false;
        }
        for (int i = 0; i < objetosNormal.Length; i++)
        {
            objetosNormal[i].enabled = true;
        }
        for (int i = 0; i < luzObjetos.Length; i++)
        {
            luzObjetos[i].enabled = false;
        }

        daysPlayed = -1;
    }

    bool nextDayTrans = false;
    float transTime = 0;
    float maxTransTime = 2;
    bool transitionFinished = false;

    float maxTBug = 0.1f;
    float tBug = 0;

    bool pausado = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            if (!pausado)
            {
                pausado = true;
                PauseMenu.instance.Pausar();
            }
            else
            {
                pausado = false;
                PauseMenu.instance.Resume();
            }
        }
        //para evitar colisión de click al usar y "parar de usar"
        if (tBug >= maxTBug)
        {
            if (playing && CurrentObject != -1 && Input.GetKeyDown(KeyCode.Mouse0))
            {
                StopUsingObject();
            }
        }
        else
        {
            tBug += Time.deltaTime;
        }
        //para la transición y luego avanzar
        if (nextDayTrans && !playing)
        {
            if (!transitionFinished)
            {
                transTime += Time.deltaTime;
                float prog = Mathf.Clamp01(transTime / maxTransTime);
                numDia.color = new Color(1, 1, 1, prog);
                transicion.color = new Color(0, 0, 0, prog);
                if (transTime >= maxTransTime)
                {
                    transTime = 0;
                    transitionFinished = true;
                    clickParaContinuar.SetActive(true);
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
                {
                    StartDay();
                }
            }
        }
        switch (csFinal)
        {
            case CutsceneFinal.none:
                break;
            case CutsceneFinal.amartillar:
                if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
                {
                    Debug.Log("amartillar");
                    csTimeLine = 0;
                    csFinal = CutsceneFinal.disparo;
                    Veil.enabled = false;
                    spritesCSFinal[0].enabled = false;
                    spritesCSFinal[1].enabled = true;
                    music.Stop();
                    //sonido disparo
                    AudioManager.instance.efectos[2].Play();
                }
                break;
            case CutsceneFinal.disparo:
                csTimeLine += Time.deltaTime;
                if (csTimeLine >= maxTimeDisparo)
                {
                    csTimeLine = 0;
                    csFinal = CutsceneFinal.narracion;
                    //narracion 1
                    AudioManager.instance.narraciones[1].Play();
                }
                break;
            case CutsceneFinal.narracion:
                csTimeLine += Time.deltaTime;
                if (csTimeLine >= maxTimeNarracion)
                {
                    csTimeLine = 0;
                    csFinal = CutsceneFinal.ojos1;
                    spritesCSFinal[1].enabled = false;
                    spritesCSFinal[2].enabled = true;
                    //narracion ojos1
                    AudioManager.instance.efectos[0].Play();
                }
                break;
            case CutsceneFinal.ojos1:
                csTimeLine += Time.deltaTime;
                if (csTimeLine >= maxTimeOjos1)
                {
                    csTimeLine = 0;
                    csFinal = CutsceneFinal.ojos2;
                    spritesCSFinal[2].enabled = false;
                    spritesCSFinal[3].enabled = true;
                    //narracion ojos2
                }
                break;
            case CutsceneFinal.ojos2:
                csTimeLine += Time.deltaTime;
                if (csTimeLine >= maxTimeOjos2)
                {
                    csTimeLine = 0;
                    csFinal = CutsceneFinal.ojos3;
                    spritesCSFinal[3].enabled = false;
                    spritesCSFinal[4].enabled = true;
                    //narracion ojos3
                }
                break;
            case CutsceneFinal.ojos3:
                csTimeLine += Time.deltaTime;
                if (csTimeLine >= maxTimeOjos3)
                {
                    csTimeLine = 0;
                    csFinal = CutsceneFinal.end;
                    spritesCSFinal[5].gameObject.SetActive(true);
                }
                break;
            case CutsceneFinal.end:
                csTimeLine += Time.deltaTime;
                float prog = csTimeLine / maxTimeEnd;
                float transp = Mathf.Lerp(1, 0, prog);
                spritesCSFinal[4].color = new Color(1, 1, 1, transp);
                if (csTimeLine >= maxTimeEnd)
                {
                    csTimeLine = 0;
                    csFinal = CutsceneFinal.none;
                    spritesCSFinal[4].gameObject.SetActive(false);
                }
                break;
        }
    }


    [HideInInspector]
    public int daysPlayed;//le aumentamos 1 nada más empezar, para evitar esto empieza en -1

    public void EndDay()
    {
        Debug.Log(daysPlayed);
        StopUsingObject();
        playing = false;
        numDia.text = "Día " + (daysPlayed + 2);
        numDia.color = new Color(1, 1, 1, 0);
        transicion.color = new Color(0, 0, 0, 0);
        //cutscene cambio de día
        nextDayTrans = true;
        transitionFinished = false;
        transTime = 0;
    }

    public void StartDay()
    {
        clickParaContinuar.SetActive(false);
        transitionFinished = false;
        nextDayTrans = false;
        numDia.color = new Color(1, 1, 1, 0);
        transicion.color = new Color(0, 0, 0, 0);
        daysPlayed++;
        switch (daysPlayed)
        {
            case 1:
                foto.SetActive(true);
                break;
            case 2:
                music.pitch = 0.8f;
                cajon.SetActive(true);
                break;
        }
        if (daysPlayed > 2)
        {
            music.pitch -= 0.05f;
        }
        playing = true;
        AnimacionReloj.instance.EmpezarAnimacion();
    }
    [HideInInspector]
    public int CurrentObject = -1;//-1= sin usar nada, 0:tele, 1:consola....
    [HideInInspector]
    public void UseObject(int n)
    {
        if (playing && tBug>maxTBug && CurrentObject==-1)
        {
            Debug.Log("Use Object " + n);
            player.enabled = false;
            CurrentObject = n;
            switch (n)
            {
                case 0:
                    Debug.Log("usando el objeto 0;" + "CurrentObject: " + CurrentObject);
                    objetosEnUso[0].enabled = true;
                    switch (daysPlayed)
                    {
                        case 0:
                            AudioManager.instance.Audios1[0].Play();
                            break;
                        case 1:
                            AudioManager.instance.Audios2[0].Play();
                            break;
                        default:
                            AudioManager.instance.Audios3[0].Play();
                            break;
                    }
                    break;
                case 1:
                    objetosEnUso[1].enabled = true;
                    switch (daysPlayed)
                    {
                        case 0:
                            AudioManager.instance.Audios1[1].Play();
                            break;
                        case 1:
                            AudioManager.instance.Audios2[1].Play();
                            break;
                        default:
                            AudioManager.instance.Audios3[1].Play();
                            break;
                    }
                    break;
                case 2:
                    objetosNormal[2].enabled = false;
                    objetosEnUso[2].enabled = true;
                    switch (daysPlayed)
                    {
                        case 0:
                            AudioManager.instance.Audios1[2].Play();
                            break;
                        case 1:
                            AudioManager.instance.Audios2[2].Play();
                            break;
                        default:
                            AudioManager.instance.Audios3[2].Play();
                            break;
                    }
                    break;
                case 3:
                    objetosEnUso[3].enabled = true;
                    switch (daysPlayed)
                    {
                        case 0:
                            AudioManager.instance.Audios1[3].Play();
                            break;
                        case 1:
                            AudioManager.instance.Audios2[3].Play();
                            break;
                        default:
                            AudioManager.instance.Audios3[3].Play();
                            break;
                    }
                    break;
                case 4:
                    objetosEnUso[4].enabled = true;
                    switch (daysPlayed)
                    {
                        case 0:
                            AudioManager.instance.Audios1[4].Play();
                            break;
                        case 1:
                            AudioManager.instance.Audios2[4].Play();
                            break;
                        default:
                            AudioManager.instance.Audios3[4].Play();
                            break;
                    }
                    break;
                case 5:
                    objetosEnUso[5].enabled = true;
                    switch (daysPlayed)
                    {
                        case 0:
                            AudioManager.instance.Audios1[5].Play();
                            break;
                        case 1:
                            AudioManager.instance.Audios2[5].Play();
                            break;
                        default:
                            AudioManager.instance.Audios3[5].Play();
                            break;
                    }
                    break;
                case 6://foto
                    Veil.enabled = true;
                    objetosEnUso[6].enabled = true;
                    player.enabled = false;
                    AudioManager.instance.fotoAudio.Play();
                    break;
                case 7://cajon
                    if (!menuPistola.activeInHierarchy)
                    {
                        playing = false;
                        Veil.enabled = true;
                        menuPistola.SetActive(true);
                        Time.timeScale = 0;
                    }
                    break;

            }
            tBug = 0;
        }
    }

    public void StopUsingObject()
    {
        Debug.Log("StopUsingObject " + CurrentObject);
        switch (CurrentObject)
        {
            case 0:
                objetosEnUso[0].enabled = false;
                switch (daysPlayed)
                {
                    case 0:
                        AudioManager.instance.Audios1[0].Stop();
                        break;
                    case 1:
                        AudioManager.instance.Audios2[0].Stop();
                        break;
                    default:
                        AudioManager.instance.Audios3[0].Stop();
                        break;
                }
                break;
            case 1:
                objetosNormal[1].enabled = true;
                objetosEnUso[1].enabled = false;
                switch (daysPlayed)
                {
                    case 0:
                        AudioManager.instance.Audios1[1].Stop();
                        break;
                    case 1:
                        AudioManager.instance.Audios2[1].Stop();
                        break;
                    default:
                        AudioManager.instance.Audios3[1].Stop();
                        break;
                }
                break;
            case 2:
                objetosNormal[2].enabled = true;
                objetosEnUso[2].enabled = false;
                switch (daysPlayed)
                {
                    case 0:
                        AudioManager.instance.Audios1[2].Stop();
                        break;
                    case 1:
                        AudioManager.instance.Audios2[2].Stop();
                        break;
                    default:
                        AudioManager.instance.Audios3[2].Stop();
                        break;
                }
                break;
            case 3:
                objetosNormal[3].enabled = true;
                objetosEnUso[3].enabled = false;
                switch (daysPlayed)
                {
                    case 0:
                        AudioManager.instance.Audios1[3].Stop();
                        break;
                    case 1:
                        AudioManager.instance.Audios2[3].Stop();
                        break;
                    default:
                        AudioManager.instance.Audios3[3].Stop();
                        break;
                }
                break;
            case 4:
                objetosNormal[4].enabled = true;
                objetosEnUso[4].enabled = false;
                switch (daysPlayed)
                {
                    case 0:
                        AudioManager.instance.Audios1[4].Stop();
                        break;
                    case 1:
                        AudioManager.instance.Audios2[4].Stop();
                        break;
                    default:
                        AudioManager.instance.Audios3[4].Stop();
                        break;
                }
                break;
            case 5:
                objetosNormal[5].enabled = true;
                objetosEnUso[5].enabled = false;
                switch (daysPlayed)
                {
                    case 0:
                        AudioManager.instance.Audios1[5].Stop();
                        break;
                    case 1:
                        AudioManager.instance.Audios2[5].Stop();
                        break;
                    default:
                        AudioManager.instance.Audios3[5].Stop();
                        break;
                }
                break;
            case 6://foto
                Veil.enabled = false;
                player.enabled = true;
                objetosEnUso[6].enabled = false;
                AudioManager.instance.fotoAudio.Stop();
                break;
            case 7://pistola
                break;
        }
        player.enabled = true;
        CurrentObject = -1;
        tBug = 0;
    }

    public void resaltarObjeto(int n)
    {
        luzObjetos[n].enabled = true;
    }
    public void StopResaltarObjeto(int n)
    {
        luzObjetos[n].enabled = false;
    }
    CutsceneFinal csFinal;
    enum CutsceneFinal
    {
        none,
        amartillar,
        disparo,
        narracion,
        ojos1,
        ojos2,
        ojos3,
        end
    }

    public float maxTimeDisparo = 2;
    public float maxTimeNarracion = 16;
    public float maxTimeOjos1 = 4;
    public float maxTimeOjos2 = 4;
    public float maxTimeOjos3 = 5;
    public float maxTimeEnd = 2;
    float csTimeLine = 0;

    public void PistolaYes()
    {
        //muerte
        AnimacionReloj.instance.StopAnimacion();
        Time.timeScale = 1;
        csFinal = CutsceneFinal.amartillar;
        spritesCSFinal[0].enabled = true;
        menuPistola.SetActive(false);
        StopUsingObject();
        player.enabled = false;
        //sonido amartillar
        AudioManager.instance.efectos[1].Play();

    }

    public void PistolaNo()
    {
        playing = true;
        Veil.enabled = false;
        menuPistola.SetActive(false);
        Time.timeScale = 1;
        player.enabled = true;
        StopUsingObject();
    }
}
