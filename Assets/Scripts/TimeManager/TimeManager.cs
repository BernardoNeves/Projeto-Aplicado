using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TimeManager : Progressive, IDamageable, IHealable, ITimeable
{
    [SerializeField] private bool isTimeCoroutineRunning = false;

    private void Start()
    {
        //StartTimeCoroutine();
    }

    private bool CheckMaxValue()
    {
        if (Value >= MaxValue) return true;
        return false;
    }

    private bool CheckMinValue()
    {
        if (Value <= 0) return true;
        return false;
    }

    public void StartTimeCoroutine()
    {
        if (isTimeCoroutineRunning)
            return;
        isTimeCoroutineRunning = true;
        StopAllCoroutines();
        StartCoroutine(TimeCoroutine());
    }

    private IEnumerator TimeCoroutine()
    {
        Value -= Time.deltaTime;
        if (CheckMinValue()) OnBreak();

        yield return new WaitForSeconds(0);
        StartCoroutine(TimeCoroutine());
    }
    public void Damage(float damageAmount)
    {
        if (damageAmount > Value)
            damageAmount = Value;

        Value -= damageAmount;

        if (CheckMinValue()) OnBreak();
    }

    public void Heal(float healAmount)
    {
        if (CheckMaxValue())
            return;

        if (healAmount > MaxValue - Value)
            healAmount = MaxValue - Value;
        Value += healAmount;
    }

    private void OnBreak()
    {
        StopAllCoroutines();
        isTimeCoroutineRunning = false;
        Value = 0f;
        Debug.Log("DEATH");
    }
}


public interface ITimeable
{
    void StartTimeCoroutine();
    void StopAllCoroutines();
}

public interface IDamageable
{
    void Damage(float amount);
}

public interface IHealable
{
    void Heal(float amount);
}