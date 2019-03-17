using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour {

    public static PlayerVisual singleton;
    [HideInInspector]
    public SpriteRenderer Renderer;

    void Start () {
        singleton = this;
        Renderer = GetComponent<SpriteRenderer>();
    }
}
