using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour {

    public static Fade singleton;
    private Image img;

    private void Awake()
    {
        img = GetComponent<Image>();
    }

    private void Start()
    {
        singleton = this;
        FadeOut(1);
    }

    public void FadeIn(float speed)
    {
        img.CrossFadeAlpha(1.0f, speed, true);
    }

    public void FadeOut(float speed)
    {
        img.CrossFadeAlpha(0.0f, speed, true);
    }
}
