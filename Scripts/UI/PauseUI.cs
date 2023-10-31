using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PauseUI : MonoBehaviour
{
    private VisualElement root;
    private Button resumeButton;
    private Button quitButton;

    // Start is called before the first frame update
    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        resumeButton = root.Q<Button>("resume-button");
        quitButton = root.Q<Button>("quit-button");

        resumeButton.clicked += ResumeButtonPressed;
        quitButton.clicked += QuitButtonPressed;

        GameManager._instance.OnGamePaused.AddListener(OnPauseReceived);
        GameManager._instance.OnGameResumed.AddListener(OnResumeRecevied);

        OnResumeRecevied();
    }

    private void OnDestroy()
    {
        resumeButton.clicked -= ResumeButtonPressed;
        quitButton.clicked -= QuitButtonPressed;
    }

    private void ResumeButtonPressed()
    {
        Debug.Log("Resume Game!");
        GameManager._instance.ResumeGame();
    }

    private void QuitButtonPressed()
    {
        Debug.Log("Quit");
        Application.Quit(0);
    }

    private void OnPauseReceived()
    {
        root.style.visibility = Visibility.Visible;
    }

    private void OnResumeRecevied()
    {
        root.style.visibility = Visibility.Hidden;
    }
}
