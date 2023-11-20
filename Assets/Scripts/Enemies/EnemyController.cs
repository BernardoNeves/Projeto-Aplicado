using UnityEngine;
using System.Collections;

public abstract class EnemyController : MonoBehaviour
{
    private TimeManager timeManager;

    [Header("Enemy Starter")]
    public int enemyDamage;
    public float moveSpeed;
    public float attackRange;
    public float attackSpeed;
    public GameObject center;

    [Header("Enemy TimeOuts")]
    public float attackTimeOut;
    public float deathTimeOut;

    [Header("Player")]
    public LayerMask playerLayer;
    public int viewRange;

    [Header("Weight")]
    public bool _isHeavy;

    [HideInInspector] public bool seePlayer;

    public int circleSegments = 30;

    public Color attackColor = Color.red;
    private Color originalColor;
    public SpriteRenderer spriteRenderer;


    public virtual void Awake()
    {
        timeManager = GetComponent<TimeManager>();
        timeManager.MaxValue = Random.Range(15, 61);
        timeManager.Value = timeManager.MaxValue;
        originalColor = spriteRenderer.color;
        timeManager.OnDeathAction += Death; 
    }

    public virtual void Patrol()
    {

    }

    public virtual void Chase()
    {
        
    }

    public void Update()
    {
        CheckDetection();
    }

    public void CheckDetection()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, viewRange, playerLayer);

        seePlayer = (playerCollider != null);
    }

    //Check if Collision with the Player Happend and Gives Damage to It
    public virtual void Attack()
    {
        StartCoroutine(AttackDelay());
        StartCoroutine(FlashColor(Color.red, attackSpeed));
    }

    private IEnumerator AttackDelay() 
    {
        yield return new WaitForSeconds(attackSpeed);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(center.transform.position, attackRange);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.tag == "Player")
            {
                IDamageable damageable = colliders[i].GetComponent<IDamageable>();
                damageable?.Damage(enemyDamage);              
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center.transform.position, attackRange);

        // Draw a circle to visualize the attack range
        float angleStep = 360f / circleSegments;
        Vector3 prevPos = Vector3.zero;
        for (int i = 0; i <= circleSegments; i++)
        {
            float angle = Mathf.Deg2Rad * i * angleStep;
            Vector3 newPos = center.transform.position + new Vector3(Mathf.Cos(angle) * attackRange, Mathf.Sin(angle) * attackRange, 0f);
            if (i > 0)
            {
                Gizmos.DrawLine(prevPos, newPos);
            }
            prevPos = newPos;
        }
    }

    private IEnumerator FlashColor(Color flashColor, float duration)
    {
        // Store the original color including alpha
        Color startColor = spriteRenderer.color;

        // Change the color temporarily
        spriteRenderer.color = flashColor;

        // Wait for the next frame
        yield return null;

        // Wait for the specified duration
        yield return new WaitForSeconds(duration);

        // Restore the original color, including alpha
        spriteRenderer.color = startColor;
    }

    void Death()
    {
        Destroy(gameObject);
    }
}
