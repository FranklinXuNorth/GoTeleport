using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Tutorial : MonoBehaviour
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

    public GameObject[] player1TeleportTime;
    public GameObject[] player2TeleportTime;

    // Start is called before the first frame update
    void Awake()
    {
        playerList = new List<GameObject>();

    }

    public bool KeyboardInputPlayer1()
    {
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

    public bool KeyboardInputPlayer2()
    {
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



    public void changePlayer1Teleport(int time)
    {

    }

    public void changePlayer2Teleport(int time)
    {

    }

    public void reducePlayer1Health(int health)
    {

    }

    public void reducePlayer2Health(int health)
    {

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

        
    }
}
