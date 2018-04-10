﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuButton : MonoBehaviour {

    public Text text;

    private void Awake()
    {
        text.color= new Color(1, 1, 1, 0.5f);
    }

    private void OnMouseEnter()
    {
        text.color = new Color(1, 1, 1, 0.8f);
    }

    private void OnMouseExit()
    {
        text.color = new Color(1, 1, 1, 0.5f);
    }

    private void OnMouseDrag()
    {
        text.color = new Color(1, 1, 1, 1f);
    }

    private void OnMouseUp()
    {
        text.color = new Color(1, 1, 1, 0.8f);
    }
}
