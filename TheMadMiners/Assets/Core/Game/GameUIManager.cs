using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour {

    public static GameUIManager singleton;

    [Header("HealthUI")]
    public GameObject[] Hearts;

    private void Start()
    {
        singleton = this; 
    }

    public void RemoveHeart(int health) {
        if (health > 0)
        {
            for (int i = Hearts.Length - 1; i > health - 1; i--)
            {
                Hearts[i].SetActive(false);
            }
        }

        if (health <= 0)
        {
            for (int i = 0; i < Hearts.Length; i++)
            {
                Hearts[i].SetActive(false);
            }
        }
	}

    public void RestHearts()
    {
        for (int i = 0; i < Hearts.Length; i++)
        {
            Hearts[i].SetActive(true);
        }
    }
}
