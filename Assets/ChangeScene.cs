using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private Vector2 playerPosition = new Vector2(-8, -2);
    private GameObject player;
    

    private void Update()
    {
        player = GameObject.FindWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            DontDestroyOnLoad(player);

            player.transform.position = playerPosition;

            SceneManager.LoadScene(sceneName);
        }
    }
}
