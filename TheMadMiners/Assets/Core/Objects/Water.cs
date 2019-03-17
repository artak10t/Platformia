using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour {
    public GameObject WaterEffect;
    public float Knockback = 1;
    public float CoolDown = 0.1f;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerHealth>().canTakeDamage)
            {
                PlayerHealth.singleton.OnDamage(0, Knockback, transform.position, CoolDown, true);
                if (PlayerBuffs.singleton.OnFire)
                {
                    PlayerBuffs.singleton.OnFire = false;
                }
                Instantiate(WaterEffect, PlayerVisual.singleton.transform.position, Quaternion.identity);
            }
        }
    }
}
