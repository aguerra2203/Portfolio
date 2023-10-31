using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class WinUI : MonoBehaviour
{
    private VisualElement root;
    private Button mainmenuButton;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip winClip;

    // Start is called before the first frame update
    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        mainmenuButton = root.Q<Button>("mainmenu-button");

        mainmenuButton.clicked += MainMenuButtonPressed;

        GameManager._instance.OnGameWon.AddListener(OnWin);


    }

    void Update()
    {
        if (GameManager._instance.currentGameState != GameManager.GameState.PlayerWon)
        {
            root.style.visibility = Visibility.Hidden;
        }
        else
        {
            root.style.visibility = Visibility.Visible;
        }
    }

    private void OnDestroy()
    {
        mainmenuButton.clicked -= MainMenuButtonPressed;
    }

    private void MainMenuButtonPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void OnWin()
    {
        root.style.visibility = Visibility.Visible;
        audioSource.clip = winClip;
        audioSource.Play();
    }
}
