using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    [Header("Enemy Starter")]
    public int enemyDamage;
    public float moveSpeed;

    [Header("Detection")]
    public float viewRange;
    public LayerMask playerLayer;

    [Header("Patrol Points")]
    public Transform[] patrolPoints;
    private int currentPoint;

    [Header("Weight")]
    public bool _isHeavy;

    //Enemy States
    private bool _isPatrol;
    private bool _isChase;

    //Player
    private Transform _playerTransform;

    public bool playerWasAttacked = false; 

    //Start to Initialize Variables
    void Start()
    {
        _isPatrol = true;
        _isChase = false;

        currentPoint = 0;
    }

    //Update to Update the Logic being used
    void Update()
    {
        CheckDetection();

        if (!_isPatrol)
        {
            Chase();
        } 
        else
        {
            Patrol();
        }
    }

    //Checks if the Player is in Ranged of the Enemy View
    private void CheckDetection()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, viewRange, playerLayer);

        //If yes the Enemy start Chasing the Player, if not the Enemy starts Patrolling
        if (playerCollider != null)
        {
            _isPatrol = false;
            _isChase = true;
            _playerTransform = playerCollider.transform;
        }
        else if (_isChase)
        {
            _isPatrol = true;
            _isChase = false;
        }
    }

    //Logic that the Enemy Uses to Chase the Enemy
    private void Chase()
    {
        Vector3 playerDirection = _playerTransform.position - transform.position;
        playerDirection.Normalize();
        transform.Translate(playerDirection * moveSpeed * Time.deltaTime);
    }

    //Logic that the Enemy Uses to Patrol Between Points
    private void Patrol()
    {
        Vector2 patrolPointPosition = patrolPoints[currentPoint].position;

        transform.position = Vector2.MoveTowards(transform.position, patrolPointPosition, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, patrolPointPosition) <= 0.2f) NextPoint();
    }

    //Changes the Patrol Point to the Next One
    public void NextPoint()
    {
        currentPoint = (currentPoint + 1) % patrolPoints.Length;
    }

    //Check if Collision with the Player Happend and Gives Damage to It
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            ApplyDamage(collision.gameObject.GetComponent<IDamageable>());
        }
    }

    protected void ApplyDamage(IDamageable damageable)
    {
        damageable.Damage(enemyDamage);
    }
}
