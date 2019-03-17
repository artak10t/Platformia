using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuffs : MonoBehaviour {

    public static PlayerBuffs singleton;

    [Header("Fire")]
    public ParticleSystem onFireEffect;
    public int onFireCircles = 3;
    public int onFireDamage = 1;
    public float onFireFrequency = 1;
    private bool onFire = false;
    private bool onFireNow = false;
    private int onFireCurrentCircle = 0;

    [Header("Acid")]
    public ParticleSystem onAcidEffect;
    public int onAcidCircles = 3;
    public int onAcidDamage = 1;
    public float onAcidFrequency = 1;
    private bool onAcid = false;
    private bool onAcidNow = false;
    private int onAcidCurrentCircle = 0;

    void Start () {
        singleton = this;
	}
    
    private IEnumerator OnFireTimer()
    {
        Debug.Log("Buff: Fire - Started");
        onFireEffect.Play();
        onFireNow = true;
        while (onFireCurrentCircle < onFireCircles)
        {
            yield return new WaitForSeconds(onFireFrequency);
            onFireCurrentCircle++;
            if (onFireCurrentCircle >= onFireCircles)
                break;
            PlayerHealth.singleton.OnDamage(onFireDamage, 0, Vector2.zero, onFireFrequency, true);
        }
        onFireCurrentCircle = 0;
        onFire = false;
        onFireNow = false;
        onFireEffect.Stop();
        Debug.Log("Buff: Fire - Stopped");
    }

    public bool OnFire
    {
        get
        {
            return onFire;
        }
        set
        {
            if (value)
            {
                onFire = true;
                onFireCurrentCircle = 0;
                if (!onFireNow)
                {
                    StartCoroutine(OnFireTimer());
                }
                else
                {
                    PlayerHealth.singleton.OnDamage(0, 4, Vector2.zero, onFireFrequency, true);
                }
            }
            else
            {
                onFire = false;
                onFireNow = false;
                if (onFireCurrentCircle > 0)
                {
                    onFireCurrentCircle = onFireCircles;
                }
                else
                {
                    onFireEffect.Stop();
                }
            }
        }
    }

    private IEnumerator OnAcidTimer()
    {
        Debug.Log("Buff: Acid - Started");
        onAcidEffect.Play();
        onAcidNow = true;
        while (onAcidCurrentCircle < onAcidCircles)
        {
            yield return new WaitForSeconds(onAcidFrequency);
            onAcidCurrentCircle++;
            if (onAcidCurrentCircle >= onAcidCircles)
                break;
            PlayerHealth.singleton.OnDamage(onAcidDamage, 0, Vector2.zero, onAcidFrequency, true);
        }
        onAcidCurrentCircle = 0;
        onAcid = false;
        onAcidNow = false;
        onAcidEffect.Stop();
        Debug.Log("Buff: Acid - Stopped");
    }

    public bool OnAcid
    {
        get
        {
            return onAcid;
        }
        set
        {
            if (value)
            {
                onAcid = true;
                onAcidCurrentCircle = 0;
                if (!onAcidNow)
                {
                    StartCoroutine(OnAcidTimer());
                }
                else
                {
                    PlayerHealth.singleton.OnDamage(0, 4, Vector2.zero, onAcidFrequency, true);
                }
            }
            else
            {
                onAcid = false;
                onAcidNow = false;
                if (onAcidCurrentCircle > 0)
                {
                    onAcidCurrentCircle = onAcidCircles;
                }
                else
                {
                    onAcidEffect.Stop();
                }
            }
        }
    }
}
