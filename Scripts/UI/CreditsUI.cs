using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CreditsUI : MonoBehaviour
{

    private VisualElement root;

    // Start is called before the first frame update
    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager._instance.currentGameState != GameManager.GameState.Credits)
        {
            root.style.visibility = Visibility.Hidden;
        }
        else
        {
            root.style.visibility = Visibility.Visible;
        }
    }
}
