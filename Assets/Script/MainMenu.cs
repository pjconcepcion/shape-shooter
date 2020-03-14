using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _helpPanel;

    [SerializeField]
    private GameObject _startPanel;

    private string GAME = "Game";

    // Start is called before the first frame update
    private void Start()
    {        
        _helpPanel.SetActive(false);
        _startPanel.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(GAME);
    }

    public void Help()
    {
        _helpPanel.SetActive(true);
        _startPanel.SetActive(false);
    }

    public void HelpBack()
    {
        Start();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
