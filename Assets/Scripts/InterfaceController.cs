using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceController : MonoBehaviour
{

    public static InterfaceController instance;

    bool ObjectsMenuOn;
    bool AcceptButtonOn;

    public GameObject ObjetosMenu;
    public GameObject[] objetos;
    public Image[] iconObjetos;
    GameObject iconObjetosFather;
    public Image AcceptButton;
    public Text AcceptText;
    public Text Instructions;
    Vector2[] posIconObjetos = new Vector2[6];
    List<int> chosenObjects;
    [Tooltip("Maximo numero de objetos que el jugador puede elegir del menu")]
    public int maxObjects = 4;
    int currentObjects = 0;
    [Tooltip("radio alrededor de los objetos en el menu que se considera como un click valido")]
    public float clickRadio = 2;


    public struct mClick
    {
        public Vector2 pos;

        public mClick(Vector2 _pos)
        {
            pos = _pos;
        }
        //lastClicked?
    }


    private void Awake()
    {
        instance = this;

        ObjetosMenu.SetActive(true);
        ObjectsMenuOn = false;
        chosenObjects = new List<int>();

        for (int i = 0; i < iconObjetos.Length; i++)
        {
            objetos[i].SetActive(false);
        }
        for (int i = 0; i < iconObjetos.Length; i++)
        {
            //Debug.Log("POS DE ICONO " + iconObjetos[i].gameObject + " = " + iconObjetos[i].gameObject.GetComponent<RectTransform>().position);
            posIconObjetos[i] = iconObjetos[i].gameObject.GetComponent<RectTransform>().position;
        }
        AcceptButtonOn = false;
        Color aux = AcceptText.color;
        aux = new Color(aux.r, aux.g, aux.b, 0.7f);
        AcceptText.color = aux;
        Instructions.text = "Elige " + maxObjects + " objetos";

    }
    private void Start()
    {
        StartCoroutine("Prologo");
    }

    public void KonoMouseDown()
    {
        Debug.Log("click0");
        if (ObjectsMenuOn)
        {
            Vector3 mPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mClick click = new mClick(mPos);
            Debug.Log("click1: pos= " + mPos);
            int closestButton = -1;
            float closestDistance = float.MaxValue;
            for (int i = 0; i < iconObjetos.Length; i++)
            {
                float dist = Mathf.Abs(Vector2.Distance(posIconObjetos[i], click.pos));
                //Debug.Log("click2: pos de boton " + i + " = " + posIconObjetos[i] + "; Dist= " + dist);
                if (dist < clickRadio)
                {
                    if (dist < closestDistance)
                    {
                        closestButton = i;
                        closestDistance = dist;
                    }
                }
            }
            if (AcceptButtonOn)//si el boton accept esta ready, comprobamos tambien con el
            {
                float dist = Mathf.Abs(Vector2.Distance(AcceptButton.rectTransform.position, click.pos));
                if (dist < clickRadio)
                {
                    if (dist < closestDistance)
                    {
                        closestButton = -2;
                        closestDistance = dist;
                    }
                }
            }
            if (closestButton != -1)
            {
                //Debug.Log("DoClick");
                DoClick(closestButton);
            }
        }
        else
        {
            /*if (GameController.instance.playing)
            {
                if (GameController.instance.CurrentObject != -1)
                {
                    Debug.Log("Click 2: STOP USING OBJECT");
                    GameController.instance.StopUsingObject();
                }
            }*/
        }
    }

    void DoClick(int boton)
    {
        if (boton >= 0)
        {
            if (chosenObjects.Contains(boton))//ya estaba seleccionado, por lo que lo deseleccionamos
            {
                chosenObjects.Remove(boton);
                if (currentObjects == maxObjects)
                {
                    AcceptButtonOn = false;
                    Color aux1 = AcceptText.color;
                    aux1 = new Color(aux1.r, aux1.g, aux1.b, 0.5f);
                    AcceptText.color = aux1;
                }
                currentObjects--;
                objetos[boton].SetActive(false);
                Color aux = iconObjetos[boton].GetComponent<Image>().color;
                aux = new Color(aux.r, aux.g, aux.b, 1f);
                iconObjetos[boton].GetComponent<Image>().color = aux;
                //aclarar imagen
                //quitar objeto de la habitación
            }
            else if (currentObjects < maxObjects)
            {
                chosenObjects.Add(boton);
                currentObjects++;
                objetos[boton].SetActive(true);
                Color aux = iconObjetos[boton].GetComponent<Image>().color;
                aux = new Color(aux.r, aux.g, aux.b, 0.5f);
                iconObjetos[boton].GetComponent<Image>().color = aux;
                //oscurecer la imagen
                //poner objeto en la habitación
            }

            if (currentObjects == maxObjects)
            {
                //BOTON OK ILUMINADO
                AcceptButtonOn = true;
                Color aux = AcceptText.color;
                aux = new Color(aux.r, aux.g, aux.b, 1f);
                AcceptText.color = aux;
            }
        }
        else if (boton == -2)//Se ha pulsado aceptar
        {
            //quitar el menu y empezar el juego
            ObjetosMenu.SetActive(false);
            ObjectsMenuOn = false;
            GameController.instance.EndDay();
        }

    }

    bool difuminado = false;
    float maxTimeDifuminado = 2;
    float timeDifuminado = 0;
    IEnumerator Prologo()
    {
        GameController.instance.spritesCSFinal[1].enabled = true;
        AudioManager.instance.narraciones[0].Play();
        yield return new WaitForSeconds(9);
        difuminado = true;
        //ObjectsMenuOn = true;
    }
    private void Update()
    {
        if (difuminado)
        {
            timeDifuminado += Time.deltaTime;
            float prog = timeDifuminado / maxTimeDifuminado;
            float aux = Mathf.Lerp(1, 0, prog);
            GameController.instance.spritesCSFinal[1].color = new Color(1, 1, 1, aux);
            if(timeDifuminado>= maxTimeDifuminado)
            {
                GameController.instance.spritesCSFinal[1].enabled = false;
                GameController.instance.spritesCSFinal[1].color = new Color(1, 1, 1, 1);
                ObjectsMenuOn = true;
                difuminado = false;

            }
        }
    }
}
