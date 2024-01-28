using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchBack : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchToPlay()
    {
        SceneManager.LoadScene("Main Scene");
    }

    public void SwitchToEnd()
    {
        SceneManager.LoadScene("SceneEnd");
    }

    public void SwitchToStart()
    {
        SceneManager.LoadScene("SceneStart");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
