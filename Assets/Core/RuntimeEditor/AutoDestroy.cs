using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour {

    public float Time = 3.5f;
    public bool DestroyOnGameEnd = false;

    private void Start()
    {
        if (DestroyOnGameEnd)
        {
            GameManager.singleton.GameHasBeenEnded += Singleton_GameHasBeenEnded;
        }
        StartCoroutine(destroy());
    }

    private void Singleton_GameHasBeenEnded()
    {
        Destroy(gameObject);
    }

    private IEnumerator destroy()
    {
        yield return new WaitForSeconds(Time);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GameManager.singleton.GameHasBeenEnded -= Singleton_GameHasBeenEnded;
    }
}
