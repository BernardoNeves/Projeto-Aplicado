using System;
using UnityEditor.Tilemaps;
using UnityEngine;

public class EnemyCharger : EnemyController
{
    [Header("Enemy")]
    public GameObject checkGround;
    public LayerMask groundLayer;

    [Header("Charger")]
    public float chargingTime = 1f;

    private float chargeTime = 0f;

    //States
    private bool _isGround;
    private bool _facingRight;

    private Transform _playerTransform;

    public void Start()
    {
        _playerTransform = GameManager.instance._player.transform;
    }

    //Patrol Logic
    public override void Patrol()
    {
        _isGround = Physics2D.OverlapCircle(checkGround.transform.position, 0.1f, groundLayer);

        float checkRotation = _facingRight ? 1f : -1f;

        Vector3 moveDirection = new Vector3(checkRotation, 0f, 0f);
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        if (!_isGround)
            Flip();
    }

    public override void Chase()
    {
        if (chargeTime < chargingTime)
        {
            chargeTime += Time.deltaTime;
        }
        else
        {
            float originalSpeed = moveSpeed;
            moveSpeed *= 2;

            Vector3 playerDirection = _playerTransform.position - transform.position;
            playerDirection.Normalize();
            transform.Translate(playerDirection * moveSpeed * Time.deltaTime);

            moveSpeed = originalSpeed;
        }
    }

    public override void Attack()
    {
        base.Attack();

        chargeTime = 0f;
    }

    void Flip()
    {
        _facingRight = !_facingRight;

        transform.Rotate(new Vector3(0, 180, 0));

        moveSpeed = -moveSpeed;
    }

   

}
