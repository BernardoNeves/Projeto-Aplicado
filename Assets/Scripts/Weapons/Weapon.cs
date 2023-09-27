using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon/Create New weapon")]
public class Weapon : ScriptableObject
{
    public string weaponName;

    public float lightDamage;
    public float lightCooldown;
    public float lightAttackSpeed;
    public float lightKnockback;

    public float heavyDamage;
    public float heavyCooldown;
    public float heavyAttackSpeed;
    public float heavyKnockback;

    public float range;

}
