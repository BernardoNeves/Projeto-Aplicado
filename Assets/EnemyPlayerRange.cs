using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayerRange : MonoBehaviour
{
    [SerializeField] private GameObject center;
    [SerializeField] private float attackRange;

    public int circleSegments = 30;
    private GameObject enemy;

    private void Update()
    {
        enemy = gameObject;
        TimeZone();
    }

    public TimeManager timeManager
    {
        get
        {
            enemy = gameObject;
            Debug.Log("hello");
            return enemy.GetComponent<TimeManager>();
        }
    }

    public void TimeZone() 
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(center.transform.position, attackRange);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.tag == "Player")
            {
                timeManager.StartTimeCoroutine();
                Debug.Log("PLAYER FOUND");
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(center.transform.position, attackRange);

        //Draw a circle to visualize the attack range
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
}
