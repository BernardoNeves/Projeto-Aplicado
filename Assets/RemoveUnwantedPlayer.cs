using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveUnwantedPlayer : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        // Find all GameObjects with the "Player" tag
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        // If there is more than one player, destroy one of them
        if (players.Length > 1)
        {
            // Destroy the first player in the array
            Destroy(players[1]);

            Debug.Log("Multiple players found. One player has been destroyed.");
        }
    }
}
