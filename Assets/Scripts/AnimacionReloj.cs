using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimacionReloj : MonoBehaviour
{

    public static AnimacionReloj instance;
    public GameObject Aguja;

    bool empezarAnim;
    //Vector3 eje;
    float prog = 0;
    float animTime = 0;
    float maxTime = 10;

    private void Awake()
    {
        instance = this;
        empezarAnim = false;
        //Vector3 auxCentro = new Vector3(transform.position.x, transform.position.y, 0);
        //Vector3 auxCentro2 = new Vector3(transform.position.x, transform.position.y, 2);
        //eje = auxCentro2-auxCentro;
    }

    public void EmpezarAnimacion()
    {
        maxTime = GameController.instance.cycleTime;
        animTime = 0;
        empezarAnim = true;
        prog = 0;
    }

    public void StopAnimacion()
    {
        empezarAnim = false;
    }

    private void Update()
    {
        if (empezarAnim)
        {
            //animacion
            animTime += Time.deltaTime;
            prog = animTime / maxTime;
            float angle = Mathf.Lerp(0, -360, prog);
            Aguja.transform.rotation = Quaternion.Euler(0, 0, angle);
            if (prog >= 1)
            {
                animTime = 0;
                empezarAnim = false;
                prog = 0;
                GameController.instance.EndDay();
            }
        }
    }
}
