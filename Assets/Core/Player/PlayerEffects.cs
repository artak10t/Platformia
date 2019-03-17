using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffects : MonoBehaviour {
    [Header("WalkingEffects")]
    public GameObject OnGroundWalkEffectPrefab;
    public float OnGroundWalkEffectFrequency = 0.1f;

    [Header("DashEffect")]
    public GameObject DashEffectPrefab;
    public float DashEffectFrequency = 1;

    [Header("JumpEffect")]
    public GameObject JumpEffectPrefab;

    private PlatformerMotor2D playerMotor;
    private bool OnGroundWalkEffect = true;
    private bool OnDashEffect = true;
    private bool OnJumpEffect = true;
    private Vector3 currentVelocity;

    private void Start()
    {
        playerMotor = gameObject.GetComponent<PlatformerMotor2D>();
        StartCoroutine(CalcVelocity());
    }

    private void Update()
    {
        OnGroundWalk();
        Dash();
        Jump();
        if (playerMotor.motorState == PlatformerMotor2D.MotorState.OnGround && !OnJumpEffect)
        {
            OnJumpEffect = true;
        }
    }

    private void OnGroundWalk()
    {
        if (currentVelocity.x > 3 && playerMotor.motorState == PlatformerMotor2D.MotorState.OnGround && OnGroundWalkEffect)
        {
            StartCoroutine(OnGroundWalkEffects(false));
        }
        else if (currentVelocity.x < -3 && playerMotor.motorState == PlatformerMotor2D.MotorState.OnGround && OnGroundWalkEffect)
        {
            StartCoroutine(OnGroundWalkEffects(true));
        }
    }

    private void Dash()
    {
        if (currentVelocity.x > 3 && playerMotor.motorState == PlatformerMotor2D.MotorState.Dashing && OnDashEffect)
        {
            StartCoroutine(DashEffect(false));
        }
        else if (currentVelocity.x < -3 && playerMotor.motorState == PlatformerMotor2D.MotorState.Dashing && OnDashEffect)
        {
            StartCoroutine(DashEffect(true));
        }
    }

    private void Jump()
    {
        if (playerMotor.motorState == PlatformerMotor2D.MotorState.Jumping && OnJumpEffect)
        {
            OnJumpEffect = false;
            Instantiate(JumpEffectPrefab, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 0.5f), Quaternion.Euler(new Vector3(0, 0, 0)));
        }
    }

    IEnumerator OnGroundWalkEffects(bool facingRight)
    {
        OnGroundWalkEffect = false;
        if (facingRight)
        {
            Instantiate(OnGroundWalkEffectPrefab, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 0.5f), Quaternion.Euler(new Vector3(0, 0, 0)));
        }
        else
        {
            Instantiate(OnGroundWalkEffectPrefab, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 0.5f), Quaternion.Euler(new Vector3(0, 180, 0)));
        }
        yield return new WaitForSeconds(OnGroundWalkEffectFrequency);
        OnGroundWalkEffect = true;
    }

    IEnumerator DashEffect(bool facingRight)
    {
        OnDashEffect = false;
        if (facingRight)
        {
            Instantiate(DashEffectPrefab, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), Quaternion.Euler(new Vector3(0, 0, 0)));
        }
        else
        {
            Instantiate(DashEffectPrefab, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), Quaternion.Euler(new Vector3(0, 180, 0)));
        }
        yield return new WaitForSeconds(DashEffectFrequency);
        OnDashEffect = true;
    }

    IEnumerator CalcVelocity()
    {
        while (Application.isPlaying)
        {
            Vector3 prevPos = transform.position;
            yield return new WaitForEndOfFrame();
            currentVelocity = (prevPos - transform.position) / Time.deltaTime;
        }
    }
}
