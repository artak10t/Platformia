using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveDamageOnTouch : MonoBehaviour {
    public int DamageAmount = 1;
    public float CoolDown = 1;
    public float Knockback = 1;

    [Header("Buffs")]
    public bool OnFire = false;
    public bool OnAcid = false;

    [Header("AfterDamage")]
    public bool changeSpriteAfterDamage = true;
    public Sprite spriteAfterDamage;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerHealth>().canTakeDamage)
            {
                if(!OnFire && !OnAcid)
                    PlayerHealth.singleton.OnDamage(DamageAmount, Knockback, transform.position, CoolDown, false);
                if (OnFire)
                {
                    PlayerBuffs.singleton.OnFire = true;
                    PlayerHealth.singleton.OnDamage(DamageAmount, Knockback, transform.position, CoolDown, true);
                }
                if (OnAcid)
                {
                    PlayerBuffs.singleton.OnAcid = true;
                    PlayerHealth.singleton.OnDamage(DamageAmount, Knockback, transform.position, CoolDown, true);
                }
                if (changeSpriteAfterDamage)
                {
                    changeSpriteAfterDamage = false;
                    gameObject.GetComponent<SpriteRenderer>().sprite = spriteAfterDamage;
                }
            }
        }
    }
}
