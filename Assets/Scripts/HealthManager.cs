using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
  public GameObject playerPrefab;
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

  public bool KeyboardInputPlayer1() {
    // player1 use WASD, LShift, F
    if (Input.GetKey(KeyCode.W))
    {
      return true;
    }
    if (Input.GetKey(KeyCode.S))
    {
      return true;
    }
    if (Input.GetKey(KeyCode.A))
    {
      return true;
    }
    if (Input.GetKey(KeyCode.D))
    {
      return true;
    }
    if (Input.GetKeyDown(KeyCode.LeftShift))
    {
      return true;
    }
    if (Input.GetKeyDown(KeyCode.F))
    {
      return true;
    }
    return false;
  }

  public bool KeyboardInputPlayer2() {
    // player2 use P;l', RShift, K
    if (Input.GetKey(KeyCode.P))
    {
      return true;
    }
    if (Input.GetKey(KeyCode.L))
    {
      return true;
    }
    if (Input.GetKey(KeyCode.Semicolon))
    {
      return true;
    }
    if (Input.GetKey(KeyCode.Quote))
    {
      return true;
    }
    if (Input.GetKeyDown(KeyCode.RightShift))
    {
      return true;
    }
    if (Input.GetKeyDown(KeyCode.K))
    {
      return true;
    }
    return false;
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
    // join player for keyboard input
    if (KeyboardInputPlayer1() && playerList.Count == 0)
    {
      // instantiate player 1
      GameObject player = Instantiate(playerPrefab, new Vector3(0, 2, 0), Quaternion.identity);
      PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
      playerMovement.isKeyboardPlayer = true;
    }
    if (KeyboardInputPlayer2() && playerList.Count == 1)
    {
      // instantiate player 2
      GameObject player = Instantiate(playerPrefab, new Vector3(0, 2, 0), Quaternion.identity);
      PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
      playerMovement.isKeyboardPlayer = true;
    }

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
