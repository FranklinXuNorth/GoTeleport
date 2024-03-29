using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
  Config config;
  private int health;

  public GameObject particleExplosion;

  private HealthManager healthManager;

  ParticleSystem particleSystem;

  bool canDrop = true;
  bool canGain = true;
  bool isDropChecked = false;
  bool isGainChecked = false;

  float dropTimer;
  float gainTimer;

  AudioSource audioSource;
  public AudioClip audioClip;

  private PlayerMovement playerMovement;

  // Start is called before the first frame update
  void Start()
  {
    dropTimer = Time.time;
    gainTimer = Time.time;
    config = FindAnyObjectByType<Config>();
    health = config.maxHealth;
    particleSystem = particleExplosion.GetComponent<ParticleSystem>();
    audioSource = GetComponent<AudioSource>();
    particleExplosion.SetActive(false);
    healthManager = FindAnyObjectByType<HealthManager>();

    playerMovement = GetComponent<PlayerMovement>();
  }

  private void Update()
  {
    if (!canDrop)
    {
      if (!isDropChecked)
      {
        isDropChecked = true;
        // what happens when health drops
        health--;
        audioSource.clip = audioClip;
        audioSource.Play();
        particleExplosion.SetActive(true);
        particleSystem.Play();
        dropTimer = Time.time;

        // reduce health display
        int index = playerMovement.currentPlayerIndex % 2;
        if (index == 0)
        {
          healthManager.reducePlayer1Health(health);
        }
        else
        {
          healthManager.reducePlayer2Health(health);
        }
      }

      if (Time.time - dropTimer >= config.invincibleTime)
      {
        dropTimer = Time.time;
        canDrop = true;
        isDropChecked = false;
      }
    }

    if (!canGain)
    {
      if (!isGainChecked)
      {
        isGainChecked = true;
        health++;
        gainTimer = Time.time;
      }

      if (Time.time - gainTimer >= config.invincibleTime)
      {
        gainTimer = Time.time;
        canGain = true;
        isGainChecked = false;

      }
    }
  }

  public void dropHealth()
  {
    if (health <= 1)
    {
      int index = (playerMovement.currentPlayerIndex + 1) % 2;
      // +1 to switch side
      // %2 to make sure it's 0 or 1 (since currentPlayerIndex might be greater than 1)
      SceneManager.LoadScene("SceneEnd_" + index);
    }
    canDrop = false;
  }

  public void gainHealth()
  {
    canGain = false;
  }

  public int GetHealth()
  {
    return health;
  }
}
