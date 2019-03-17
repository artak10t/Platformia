using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSpriteOnStart : MonoBehaviour {
    private bool _rotate = false;
    public Vector3 Rotation;

	void Update () {
        if (_rotate)
        {
            transform.Rotate(Rotation * Time.deltaTime);
        }
	}

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
        _rotate = true;
    }
}
