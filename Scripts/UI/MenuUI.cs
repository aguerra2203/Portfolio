using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{

    private VisualElement root;
    private Button startButton;
    private Button creditsButton;
    private Button quitButton;
    private TextField credits;

    private int numClicks;

    // Start is called before the first frame update
    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        startButton = root.Q<Button>("start-button");
        creditsButton = root.Q<Button>("credits-button");
        quitButton = root.Q<Button>("quit-button");
        credits = root.Q<TextField>("credits");

        startButton.clicked += StartButtonPressed;
        creditsButton.clicked += CreditsButtonPressed;
        quitButton.clicked += QuitButtonPressed;

        credits.style.visibility = Visibility.Hidden;

    }

    private void OnDestroy()
    {
        startButton.clicked -= StartButtonPressed;
        creditsButton.clicked -= CreditsButtonPressed;
        quitButton.clicked -= QuitButtonPressed;
    }

    private void StartButtonPressed()
    {
        SceneManager.LoadScene("2D Game");
        GameManager._instance.ResumeGame();
    }

    private void CreditsButtonPressed()
    {
        credits.style.visibility = Visibility.Visible;
        numClicks++;

        if (numClicks%2 == 0)
        {
            credits.style.visibility = Visibility.Hidden;
        }
    }

    private void QuitButtonPressed()
    {
        Application.Quit(0);
    }
}
