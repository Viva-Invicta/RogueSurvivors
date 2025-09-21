using DunDungeons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValeraPlayer : MonoBehaviour 
{
    [SerializeField]
    private InputService inputService;

    [SerializeField]
    private float valeraPower = 1f;

    [SerializeField]
    private ValeraScore scoreUI;

    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private float jumpCooldown;

    private int score = 0;
    private Rigidbody body;

    private bool isInCooldown = false;

    private void OnEnable()
    {
        body = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        var inputDirection = inputService.InputDirection;

        if (inputService.AttackPressed && !isInCooldown)
        {
            body.AddForce(Vector3.up * jumpForce);
            isInCooldown = true;

            StartCoroutine(Cooldown());
        }

        body.AddForce(inputDirection);
    }

    private void OnTriggerEnter(Collider collision)
    {
        scoreUI.SetScore(++score);

        var collectable = collision.GetComponent<ValeraCollectable>();

        if (collectable)
        {
            collectable.Kill();
        }
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSecondsRealtime(jumpCooldown);
        isInCooldown = false;
    }
}