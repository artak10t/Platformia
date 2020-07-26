using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour {
    public GameObject WaterEffect;
    public GameObject SteamEffect;
    public float CoolDown = 0.1f;
    public float Knockback = 1;

    private bool canSpawnParticle = true;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            if (other.GetComponent<PlayerHealth>().canTakeDamage && canSpawnParticle)
            {
                canSpawnParticle = false;

                PlayerHealth.singleton.OnDamage(0, Knockback, transform.position, CoolDown, true);
                if (PlayerBuffs.singleton.OnFire)
                {
                    PlayerBuffs.singleton.OnFire = false;
                    Instantiate(SteamEffect, PlayerVisual.singleton.transform.position, Quaternion.identity);
                }
                Instantiate(WaterEffect, PlayerVisual.singleton.transform.position, Quaternion.identity);

                StartCoroutine(CoolDownWait(CoolDown));
            }
        }
    }

    private IEnumerator CoolDownWait(float coolDown)
    {
        yield return new WaitForSeconds(coolDown);
        canSpawnParticle = true;
    }
}
