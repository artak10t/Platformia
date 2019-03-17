using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileProperties : MonoBehaviour {
    public static TileProperties singleton;

    public Toggle BackLayer;

    private void Start()
    {
        singleton = this;
    }

    public void SetBackLayer(bool status)
    {
        BackLayer.isOn = status;
    }
}
