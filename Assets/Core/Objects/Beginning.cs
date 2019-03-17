using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beginning : MonoBehaviour {

    public void PlayAnimation()
    {
        if (gameObject.GetComponentInChildren<Animation>() != null)
        {
            gameObject.GetComponentInChildren<Animation>().Play();
        }
    }
}
