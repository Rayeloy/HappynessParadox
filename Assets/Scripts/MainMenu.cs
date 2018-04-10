using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public Text text1;
    public float maxAnimTime;
    float animTime;
    bool difuminando = true;
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene("Room");
        }

        if (difuminando)
        {
            animTime += Time.deltaTime;
            float prog = animTime / maxAnimTime;
            float aux = Mathf.Lerp(1, 0, prog);
            text1.color = new Color(text1.color.g, text1.color.b, text1.color.a, prog);
            if (animTime >= maxAnimTime)
            {
                animTime = maxAnimTime;
                difuminando = false;
            }
        }
        else
        {
            animTime -= Time.deltaTime;
            float prog = Mathf.Abs(animTime / maxAnimTime);
            float aux = Mathf.Lerp(0, 1, prog);
            text1.color = new Color(text1.color.g, text1.color.b, text1.color.a, prog);
            if (animTime <=0)
            {
                animTime = 0;
                difuminando = true;
            }
        }


    }
}
