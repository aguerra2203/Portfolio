using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip backgroundClip;

    // Start is called before the first frame update
    void Start()
    {
        GameManager._instance.OnGameWon.AddListener(Stop);
        GameManager._instance.OnGameLost.AddListener(Stop);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Stop()
    {
        audioSource.clip = backgroundClip;
        audioSource.Stop();
    }
}
