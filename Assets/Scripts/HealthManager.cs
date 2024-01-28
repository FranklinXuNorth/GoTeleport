using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    public TextMeshProUGUI text1;
    public TextMeshProUGUI text2;

    public TextMeshProUGUI teleportChance1;
    public TextMeshProUGUI teleportChance2;

    private bool isAllJoin = false;

    private GameObject[] players;
    private List<GameObject> playerList;

    public GameObject[] player1Health;
    public GameObject[] player2Health;

    // Start is called before the first frame update
    void Start()
    {
        playerList = new List<GameObject>();
    }

    private void Update()
    {
        // get 2 players
        if (!isAllJoin)
        {
            players = GameObject.FindGameObjectsWithTag("Player");
        }

        if (players.Length == 1)
        {
            if (!playerList.Contains(players[0]))
            {
                playerList.Add(players[0]);
            }
                
        }

        else if (players.Length == 2)
        {
            foreach (GameObject player in players)
            {
                if (!playerList.Contains(player))
                {
                    playerList.Add(player);
                }
            }
            isAllJoin = true;
        }

        // UI
        if (playerList.Count == 0)
        {
            text1.text = "Waiting for Player 1";
            text2.text = "Waiting for Player 2";
            teleportChance1.text = "";
            teleportChance2.text = "";
        }

        else if (playerList.Count == 1)
        {
            text1.text = $"Player 1\nHealth: {playerList[0].GetComponent<Health>().GetHealth()}";
            text2.text = "Waiting for Player 2";

            // teleportChance1.text = $"{playerList[0].GetComponent<PlayerMovement>().GetRestTeleportTime()}";
            teleportChance1.text = "";
            teleportChance2.text = "";
        }

        else if (playerList.Count == 2)
        {
            text1.text = $"Player 1\nHealth: {playerList[0].GetComponent<Health>().GetHealth()}";
            text2.text = $"Player 2\nHealth: {playerList[1].GetComponent<Health>().GetHealth()}";

            teleportChance1.text = $"{playerList[0].GetComponent<PlayerMovement>().GetRestTeleportTime()}";
            teleportChance2.text = $"{playerList[1].GetComponent<PlayerMovement>().GetRestTeleportTime()}";
        }


        

    }

    

}
