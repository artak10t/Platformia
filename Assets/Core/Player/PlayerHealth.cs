using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    public static PlayerHealth singleton;

    [Header("Audio")]
    public AudioClip[] damageAudio;

    [Header("Dialogue")]
    public string[] damageDialogue;

    [Header("Particles")]
    public GameObject damageBlood;
    public GameObject deathBlood;

    private int health = 10;
    private PlatformerMotor2D _motor;
    private SpriteAnimator _bubbleDialogue;
    [HideInInspector]
    public bool isAlive = true;
    [HideInInspector]
    public bool canTakeDamage = true;
    

    void Start () {
        singleton = this;
        _motor = gameObject.GetComponent<PlatformerMotor2D>();
        _bubbleDialogue = gameObject.GetComponentInChildren<SpriteAnimator>();
	}

    private void Update()
    {
        PlayerVisual.singleton.Renderer.color = Color.Lerp(PlayerVisual.singleton.Renderer.color, new Color(1, 1, 1), Time.deltaTime * 2);
        if (Input.GetKeyDown(KeyCode.H))
        {
            _motor.velocity = new Vector2(100, 0);
        }
    }

    public void OnDamage(int damage, float knockback, Vector2 targetPosition, float coolDown, bool isBuff)
    {
        if (isAlive && canTakeDamage)
        {
            if (damage != 0)
            {
                health -= damage;
                GameUIManager.singleton.RemoveHeart(health);
                if (health > 0)
                {
                    Debug.Log("Health: " + health);
                    _motor.ForceJump(knockback * _motor.jumpHeight * 10);
                    int rAudio = Random.Range(0, damageAudio.Length);
                    int rDialogue = Random.Range(0, damageDialogue.Length);
                    _bubbleDialogue.ForcePlay(damageDialogue[rDialogue], false, 0, damageAudio[rAudio]);
                    Instantiate(damageBlood, new Vector3(transform.position.x, transform.position.y + 0.5f), Quaternion.identity);
                }

                if (health <= 0)
                {
                    isAlive = false;
                    Debug.Log("Health: Dead");
                    Instantiate(deathBlood, new Vector3(transform.position.x, transform.position.y + 0.5f), Quaternion.identity);
                    Fade.singleton.FadeIn(3f);
                    GameManager.singleton.EndingGame(false);
                }
            }
            else
            {
                _motor.ForceJump(knockback * _motor.jumpHeight * 10);
            }
            if (isAlive)
                StartCoroutine(CoolDownWait(coolDown, damage, isBuff));
        }
    }

    private IEnumerator CoolDownWait(float coolDown, int damage, bool isBuff)
    {
        if (damage != 0)
        {
            PlayerVisual.singleton.Renderer.color = new Color(1, 0, 0);
        }
        canTakeDamage = false;
        if (!isBuff)
        {
            yield return new WaitForSeconds(coolDown);
        }
        else
        {
            yield return new WaitForSeconds(0);
        }
        canTakeDamage = true;
    }
}
