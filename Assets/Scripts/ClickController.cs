using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickController : MonoBehaviour
{

    public int indiceObjeto;

    private void OnMouseEnter()
    {
        if (GameController.instance.playing)
            GameController.instance.resaltarObjeto(indiceObjeto);
    }

    private void OnMouseExit()
    {
        if (GameController.instance.playing)
            GameController.instance.StopResaltarObjeto(indiceObjeto);
    }

    private void OnMouseDown()
    {
        if (GameController.instance.playing)
            GameController.instance.UseObject(indiceObjeto);
    }
}
