using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
  public TextMeshProUGUI text1;
  public TextMeshProUGUI text2;

  public TextMeshProUGUI text3;
  public TextMeshProUGUI text4;

  //public TextMeshProUGUI teleportChance1;
  //public TextMeshProUGUI teleportChance2;

  private bool isAllJoin = false;

  private GameObject[] players;
  private List<GameObject> playerList;

  public GameObject[] player1Health;
  public GameObject[] player2Health;

  // Start is called before the first frame update
  void Awake()
  {
    playerList = new List<GameObject>();

  }

  void Start()
  {
  }

  public void reducePlayer1Health(int health)
  {
    // set number of health to active, and the rest to inactive
    for (int i = 0; i < player1Health.Length; i++)
    {
      if (i < health)
      {
        // player1Health[i].GetComponent<SpriteRenderer>().enabled = true;
        player1Health[i].GetComponent<Image>().enabled = true;
      }
      else
      {
        // player1Health[i].GetComponent<SpriteRenderer>().enabled = false;
        player1Health[i].GetComponent<Image>().enabled = false;
      }
    }
  }

  public void reducePlayer2Health(int health)
  {
    // set number of health to active, and the rest to inactive
    for (int i = 0; i < player2Health.Length; i++)
    {
      if (i < health)
      {
        // player2Health[i].GetComponent<SpriteRenderer>().enabled = true;
        player2Health[i].GetComponent<Image>().enabled = true;
      }
      else
      {
        // player2Health[i].GetComponent<SpriteRenderer>().enabled = false;
        player2Health[i].GetComponent<Image>().enabled = false;
      }
    }
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
      //teleportChance1.text = "";
      //teleportChance2.text = "";
    }

    else if (playerList.Count == 1)
    {

      int health0 = playerList[0].GetComponent<Health>().GetHealth();
      text1.text = $"Player 1: Health {health0}";
      text2.text = "Waiting for Player 2";

      text3.text = $"Teleport Time: {playerList[0].GetComponent<PlayerMovement>().GetRestTeleportTime()}";

      // teleportChance1.text = $"{playerList[0].GetComponent<PlayerMovement>().GetRestTeleportTime()}";
      //teleportChance1.text = "";
      //teleportChance2.text = "";
    }

    else if (playerList.Count == 2)
    {
      int health0 = playerList[0].GetComponent<Health>().GetHealth();
      int health1 = playerList[1].GetComponent<Health>().GetHealth();

      text1.text = $"Player 1: Health {health0}";
      text2.text = $"Player 2: Health {health1}";

      //teleportChance1.text = $"{playerList[0].GetComponent<PlayerMovement>().GetRestTeleportTime()}";
      //teleportChance2.text = $"{playerList[1].GetComponent<PlayerMovement>().GetRestTeleportTime()}";

      text3.text = $"Teleport Time: {playerList[0].GetComponent<PlayerMovement>().GetRestTeleportTime()}";
      text4.text = $"Teleport Time: {playerList[1].GetComponent<PlayerMovement>().GetRestTeleportTime()}";
    }




  }



}
