using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selections : MonoBehaviour {
    public static Selections singleton;
    [HideInInspector]
    public RuleTile[] RuleTiles;
    [HideInInspector]
    public GameObject[] Special;
    [HideInInspector]
    public GameObject[] Traps;

    void Awake () {
        singleton = this;
        LoadResources();
	}

    private void LoadResources()
    {
        RuleTiles = Resources.LoadAll<RuleTile>("RuleTiles");
        Special = Resources.LoadAll<GameObject>("Special");
        Traps = Resources.LoadAll<GameObject>("Traps");
    }
}
