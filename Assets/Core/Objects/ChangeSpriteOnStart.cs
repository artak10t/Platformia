using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSpriteOnStart : MonoBehaviour
{
    public Sprite newSprite;

    void Start()
    {
        GameManager.singleton.GameHasBeenStarted += Singleton_GameHasBeenStarted;
    }

    private void OnDestroy()
    {
        GameManager.singleton.GameHasBeenStarted -= Singleton_GameHasBeenStarted;
    }

    private void Singleton_GameHasBeenStarted()
    {
        GetComponent<SpriteRenderer>().sprite = newSprite;
    }
}
