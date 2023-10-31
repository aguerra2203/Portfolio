using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HUD : MonoBehaviour
{
    private VisualElement root;
    private VisualElement heartContainer;

    [SerializeField] private Sprite heartSprite;

    private List<VisualElement> heartsList;

    // Start is called before the first frame update
    void Start()
    {
        //We get the root visual element from the UIDocument component so we reference 
        //other UI elements from it
        root = GetComponent<UIDocument>().rootVisualElement;
        heartContainer = root.Q<VisualElement>("heart-container");
        heartsList = new List<VisualElement>();

        heartsList.Add(heartContainer.Q<VisualElement>("heart1"));
        heartsList.Add(heartContainer.Q<VisualElement>("heart2"));
        heartsList.Add(heartContainer.Q<VisualElement>("heart3"));
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddHeart()
    {
        VisualElement newHeart = new VisualElement();

        newHeart.style.backgroundImage = new StyleBackground(heartSprite);
        newHeart.style.width = 50f;
        newHeart.style.height = 50f;
        newHeart.style.marginBottom = 5f;
        newHeart.style.marginTop = 5f;
        newHeart.style.marginLeft = 5f;
        newHeart.style.marginRight = 5f;

        heartContainer.Add(newHeart);
        heartsList.Add(newHeart);
    }

    public void RemoveHeart()
    {
        if (heartsList.Count > 0)
        {
            VisualElement heartToRemove = heartsList[0];
            heartsList.Remove(heartToRemove);
            heartContainer.Remove(heartToRemove);
        }

    }
}
