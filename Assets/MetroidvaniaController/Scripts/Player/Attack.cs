using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
	private float dmgValue;
	private float knockbackValue;
	private Vector2 knockbackDirection;
	public GameObject throwableObject;
	public Transform attackCheck;
	private Rigidbody2D m_Rigidbody2D;
	public Animator animator;
	public bool canAttack = true;
	public bool isTimeToCheck = false;
	public bool isAttacking = false;
	public Weapon pointer;

	public bool isAttackingLight = false;
	public bool isAttackingHeavy = false;

	public GameObject cam;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
	}

	// Start is called before the first frame update
	void Start()
    {
		
	}

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Mouse0) && canAttack && !isAttacking)
		{
			knockbackValue = pointer.lightKnockback;
			dmgValue = pointer.lightDamage;
			isAttacking = true;
			canAttack = false;
			animator.SetFloat("AttackSpeed", pointer.lightAttackSpeed);
			animator.SetBool("IsAttacking", true);
			StartCoroutine(LightAttackCooldown());
		}

		if (Input.GetKeyDown(KeyCode.Mouse1) && canAttack && !isAttacking)
		{
			knockbackValue = pointer.heavyKnockback;
			dmgValue = pointer.heavyDamage;
			isAttacking = true;
			canAttack = false;
			animator.SetFloat("AttackSpeed", pointer.heavyAttackSpeed);
			animator.SetBool("IsAttacking", true);
			StartCoroutine(HeavyAttackCooldown());
		}

	}

	IEnumerator LightAttackCooldown()
	{
		yield return new WaitForSeconds(pointer.lightCooldown);
		isAttacking = false;
		canAttack = true;
	}
	IEnumerator HeavyAttackCooldown()
	{
		yield return new WaitForSeconds(pointer.heavyCooldown);
		isAttacking = false;
		canAttack = true;
	}

	public void DoDashDamage()
	{
		dmgValue = Mathf.Abs(dmgValue);
		Collider2D[] collidersEnemies = Physics2D.OverlapCircleAll(attackCheck.position, pointer.range);
		for (int i = 0; i < collidersEnemies.Length; i++)
		{
			if (collidersEnemies[i].gameObject.tag == "Enemy")
			{
				/*
				if (collidersEnemies[i].transform.position.x - transform.position.x < 0)
				{
					dmgValue = -dmgValue;
				}
				collidersEnemies[i].gameObject.SendMessage("ApplyDamage", dmgValue);
				cam.GetComponent<CameraFollow>().ShakeCamera();
				*/

				ApplyDamage(collidersEnemies[i].gameObject.GetComponent<IDamageable>(), dmgValue);

				knockbackDirection = transform.position - collidersEnemies[i].transform.position;

				m_Rigidbody2D.AddForce(knockbackDirection, ForceMode2D.Impulse);
			}
		}
	}

	protected void ApplyDamage(IDamageable damageable, float dmgAmount)
	{
		damageable.Damage(dmgAmount);
	}
}
